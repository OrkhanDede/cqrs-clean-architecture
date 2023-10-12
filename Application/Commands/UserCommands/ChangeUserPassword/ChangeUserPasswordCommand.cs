using Infrastructure.Configurations.Commands;

namespace Application.Commands.UserCommands.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : CommandBase<ChangeUserPasswordResponse>
    {
        public ChangeUserPasswordCommand(string requestedUserId,
            bool changeWithoutOldPassword, ChangeUserPasswordRequest request)
        {
            RequestedUserId = requestedUserId;
            Request = request;
            ChangeWithoutOldPassword = changeWithoutOldPassword;
        }     
        public ChangeUserPasswordCommand(string requestedUserId,
            ChangeUserPasswordRequest request)
        {
            RequestedUserId = requestedUserId;
            Request = request;
        }
        public string RequestedUserId { get; set; }
        public bool ChangeWithoutOldPassword { get; set; }
        public ChangeUserPasswordRequest Request { get; set; }
    }
}
