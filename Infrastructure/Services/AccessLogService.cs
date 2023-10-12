using System;
using System.Threading.Tasks;
using Core.Extensions;
using DataAccess.Repository;
using DataAccess.Repository.AccessLogRepository;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class AccessLogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthService _authService;
        private readonly IAccessLogRepository _accessLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccessLogService(IHttpContextAccessor httpContextAccessor, AuthService authService, IAccessLogRepository accessLogRepository, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
            _accessLogRepository = accessLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAccessLogAsync(string userId)
        {
            await this._accessLogRepository.AddAsync(new AccessLog()
            {
                UserId = userId,
                IpAddress = _httpContextAccessor.HttpContext?.GetRequestIp(),
                Host = _httpContextAccessor.HttpContext?.Request.Host.Host,
                Date = DateTime.Now
            });
            await _unitOfWork.CompleteAsync();

        }

    }
}
