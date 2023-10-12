using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Core.Utilities;
using DataAccess.Repository;
using DataAccess.Repository.RefreshTokenRepository;
using Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class RefreshTokenResult
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class TokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenSettings _tokenSettings;

        public TokenService(IOptions<TokenSettings> tokenSettings, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _tokenSettings = tokenSettings.Value;
        }

        public string GenerateToken(User user, params Claim[] claims)
        {
            var constClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString().Substring(0,16)),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            };
            constClaims.AddRange(claims);
            var appSettings = _tokenSettings;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddMinutes(appSettings.JwtExpireMinutes);


            var token = new JwtSecurityToken(appSettings.JwtIssuer, appSettings.JwtIssuer, constClaims, expires: expires,
                signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
        public bool ValidateToken(string token)
        {
            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.JwtKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                // return user id from JWT token if validation successful
                return !string.IsNullOrEmpty(userId);
            }
            catch
            {
                // return null if validation fails
                return false;
            }
        }




        public async Task<RefreshToken> AddRefreshTokenAsync(string userId, TokenClaim tokenClaim = default)
        {
            var isRememberMe = tokenClaim?.RememberMe != null && tokenClaim.RememberMe.Value.ToLower() == true.ToString().ToLower();
            var refreshToken = GenerateRefreshToken(isRememberMe);

            var data = new RefreshToken()
            {
                Token = refreshToken.Token,
                ExpireDate = refreshToken.ExpireDate,
                ImpersonatorId = tokenClaim?.ImpersonatorId?.Value,
                UserId = userId,
                CreatedById = userId
            };
            await _refreshTokenRepository.AddAsync(data);
            await _unitOfWork.CompleteAsync();

            return data;
        }
        public async Task RemoveOldRefreshTokensAsync(string userId, bool removeAll = default, Expression<Func<RefreshToken, bool>> extraPredicate = default)
        {

            Expression<Func<RefreshToken, bool>> predicate = x => x.UserId == userId &&
                                                                  (removeAll || x.DateCreated.AddDays(_tokenSettings.RefreshTokenExpireMinutes) <= DateTime.Now);
            predicate = predicate.And(extraPredicate);

            _refreshTokenRepository.DeleteWhere(predicate);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RevokeRefreshTokensAsync(string token, string reason = "Attempted reuse of revoked ancestor token")
        {
            var refreshToken = await _refreshTokenRepository.GetFirstAsync(c => c.Token == token);
            if (refreshToken != null)
            {
                var isReplaced = !string.IsNullOrEmpty(refreshToken.ReplacedByToken);
                if (isReplaced)
                {
                    await RevokeRefreshTokenAsync(refreshToken.ReplacedByToken, reason);
                }
                else
                {
                    return;

                }

            }

            throw new RecordNotFoundException("refresh token not found");
        }

        public async Task<RefreshToken> RevokeRefreshTokenAsync(string token, string reason, string replacedByToken = null)
        {
            var childToken = await _refreshTokenRepository.GetFirstAsync(c => c.Token == token);
            if (childToken == null) return null;

            if (childToken.IsActive)
            {
                //revoke
                childToken.RevokeDate = DateTime.Now;
                childToken.ReasonRevoked = reason;
                childToken.ReplacedByToken = replacedByToken;

            }

            await _unitOfWork.CompleteAsync();

            return childToken;
        }

        public async Task<RefreshToken> RotateRefreshTokenAsync(string token)
        {
            var revokedToken = await _refreshTokenRepository.GetFirstAsync(c => c.Token == token);
            if (revokedToken != null)
            {

                var newToken = await AddRefreshTokenAsync(revokedToken.UserId);
                await _unitOfWork.CompleteAsync();

                await RevokeRefreshTokenAsync(revokedToken.Token, "Replaced by new token", newToken.Token);

                return newToken;

            }

            return null;
        }

        public RefreshTokenResult GenerateRefreshToken(bool rememberMe = false)
        {
            // generate token that is valid for 7 days
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            DateTime expireDate = DateTime.Now;
            if (rememberMe)
            {
                expireDate = expireDate.AddDays(_tokenSettings.RememberMeRefreshTokenExpireDays);
            }
            else
            {
                expireDate = expireDate.AddMinutes(_tokenSettings.RefreshTokenExpireMinutes);
            }
            var claim = new
            {
                expireDate = expireDate
            };
            string token = Convert.ToBase64String(randomBytes) + "." + claim.ToBase64();
            var refreshToken = new RefreshTokenResult
            {
                Token = token,
                ExpireDate = expireDate,
                CreatedDate = DateTime.Now,
            };

            return refreshToken;
        }
    }
}
