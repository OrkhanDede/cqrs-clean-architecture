using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;

namespace DataAccess.Repository.RefreshTokenRepository
{
    public interface IRefreshTokenRepository:IRepository<RefreshToken>
    {
    }
}
