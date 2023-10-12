using System.Threading.Tasks;

namespace DataAccess.Repository.AccessLogRepository
{
    public interface IAccessLogRepository
    {
        Task AddAsync(Domain.Entities.Identity.AccessLog entity);
    }
}
