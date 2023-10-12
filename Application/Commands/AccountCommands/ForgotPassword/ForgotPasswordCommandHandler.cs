using System.Threading;
using System.Threading.Tasks;
using Application.Commands.UserCommands.ChangeUserPassword;
using Core.Extensions;
using Core.Resources;
using DataAccess.Repository.UserRepository;
using Domain.Entities.Identity;
using Infrastructure.Configurations.Commands;
using Infrastructure.Services;
using MediatR;

namespace Application.Commands.AccountCommands.ForgotPassword
{
    public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, ForgotPasswordResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly ExceptionService _exceptionService;
        public ForgotPasswordCommandHandler(IUserRepository userRepository, IMediator mediator, ExceptionService exceptionService)
        {
            _userRepository = userRepository;

            _mediator = mediator;
            _exceptionService = exceptionService;
        }
        public async Task<ForgotPasswordResponse> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
        {

            var emailOrUsername = command.Request.EmailOrUsername;
            var password = command.Request.Password;
            var isEmail = emailOrUsername.IsEmail();

            User user;
            if (isEmail)
                user = await _userRepository.GetUserByEmailAsync(emailOrUsername)
                    .ConfigureAwait(false);
            else
                user = await _userRepository.GetUserByNameAsync(emailOrUsername)
                    .ConfigureAwait(false);

            if (user == null)
                throw _exceptionService.RecordNotFoundException(ResourceKey.UserNotFoundContactToAdmin);

            var requestChangePassword = new ChangeUserPasswordRequest()
            {
                Password = password
            };

            await _mediator.Send(new ChangeUserPasswordCommand(user.Id, true, requestChangePassword)
            {
            }, cancellationToken);
            return new ForgotPasswordResponse();
        }
    }
}
