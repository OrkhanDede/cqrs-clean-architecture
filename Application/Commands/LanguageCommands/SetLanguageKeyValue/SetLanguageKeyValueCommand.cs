using Infrastructure.Configurations.Commands;

namespace Application.Commands.LanguageCommands.SetLanguageKeyValue
{
    public class SetLanguageKeyValueCommand : CommandBase<SetLanguageKeyValueResponse>
    {
        public SetLanguageKeyValueCommand(SetLanguageKeyValueRequest request)
        {
            Request = request;
        }

        public SetLanguageKeyValueRequest Request { get; set; }
    }
}
