using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Core.Extensions;
using DataAccess.Repository;
using DataAccess.Repository.FileStoreRepository;
using Domain.Entities;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;

namespace Application.Commands.FileStoreCommands.CreateFileStore
{
    public class CreateFileStoreCommandHandler : ICommandHandler<CreateFileStoreCommand, CreateFileStoreResponse>
    {
        private readonly FileService _fileService;
        private readonly IFileStoreRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateFileStoreCommandHandler(FileService fileService, IFileStoreRepository repository, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CreateFileStoreResponse> Handle(CreateFileStoreCommand command, CancellationToken cancellationToken)
        {
            PathResponse pathResponse;
            try
            {
                pathResponse = await this._fileService.SaveAsync(command.Request.FolderDir, command.Request.File);

            }
            catch (Exception e)
            {
                throw new ApiException(e.Message);
            }

            var fileInfo = command.Request.File.ToFileInfo();
            var fileStore = new FileStore()
            {
                FileExtension = fileInfo.FileExtension,
                ContentType = fileInfo.ContentType,
                FileName = fileInfo.FileName,
                UniqueFileName = Path.GetFileName(pathResponse.PhysicalPath),
                Name = fileInfo.Name,
                Path = pathResponse.PhysicalPath,
                ProjectPath = pathResponse.VirtualPath,
                SizeInBytes = fileInfo.Size
            };
            await _repository.AddAsync(fileStore);
            await _unitOfWork.CompleteAsync(cancellationToken);
            return new CreateFileStoreResponse()
            {
                Response = fileStore
            };
        }
    }
}
