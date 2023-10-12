using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Domain.Entities.Identity;

namespace DataAccess.Repository.RefreshTokenRepository
{
    public class RefreshTokenRepository:Repository<RefreshToken>,IRefreshTokenRepository,IRepositoryIdentifier
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
