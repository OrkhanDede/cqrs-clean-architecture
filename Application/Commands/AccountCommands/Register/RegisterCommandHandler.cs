using System.Threading;
using System.Threading.Tasks;
using Application.Commands.UserCommands.CreateUser;
using Infrastructure.Configurations.Commands;
using MediatR;

namespace Application.Commands.AccountCommands.Register
{
    public class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IMediator _mediator;

        public RegisterCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RegisterResponse> Handle(RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateUserCommand(new CreateUserRequest()
            {
                UserName = command.Request.UserName,
                Email = command.Request.Email,
                //Name = command.Request.Name,
                //Surname = command.Request.Surname,
                Password = command.Request.Password,
                ConfirmPassword = command.Request.ConfirmPassword,
            }), cancellationToken);
            return new RegisterResponse();
        }
    }
}
