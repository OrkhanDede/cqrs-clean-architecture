using Infrastructure.Configurations.Commands;

namespace Application.Commands.FileStoreCommands.CreateFileStore
{
    public class CreateFileStoreCommand : CommandBase<CreateFileStoreResponse>
    {
        public CreateFileStoreCommand(CreateFileStoreRequest request)
        {
            Request = request;
        }

        public CreateFileStoreRequest Request { get; set; }
    }
}
