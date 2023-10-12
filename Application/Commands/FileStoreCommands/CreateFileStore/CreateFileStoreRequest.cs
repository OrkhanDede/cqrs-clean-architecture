using Microsoft.AspNetCore.Http;

namespace Application.Commands.FileStoreCommands.CreateFileStore
{
    public class CreateFileStoreRequest
    {
        public IFormFile File { get; set; }
        public string FolderDir { get; set; }
    }
}
