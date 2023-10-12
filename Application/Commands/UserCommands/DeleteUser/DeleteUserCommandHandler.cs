using System.Threading;
using System.Threading.Tasks;
using DataAccess.Repository.UserRepository;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;

namespace Application.Commands.UserCommands.DeleteUser
{
    public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly AuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ExceptionService _exceptionService;

        public DeleteUserCommandHandler(AuthService authService, IUserRepository userRepository , ExceptionService exceptionService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _exceptionService = exceptionService;
        }
        public async Task<DeleteUserResponse> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.Request.UserId).ConfigureAwait(false);
            var isAdmin = await _authService.IsAdminAsync();


            var canEditRecord = isAdmin;
            if (!canEditRecord)
                throw _exceptionService.RecordNotEditableException();
            if (user == null)
                throw _exceptionService.RecordNotFoundException();

            await _userRepository.Delete(user);
            return new DeleteUserResponse();

        }
    }
}
