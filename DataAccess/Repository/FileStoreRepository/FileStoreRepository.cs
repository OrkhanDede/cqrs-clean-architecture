using Data;
using Domain.Entities;

namespace DataAccess.Repository.FileStoreRepository
{
    public class FileStoreRepository : Repository<FileStore>, IFileStoreRepository, IRepositoryIdentifier
    {
        public FileStoreRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
