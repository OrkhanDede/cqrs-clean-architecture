using Infrastructure.Configurations.Commands;

namespace Application.Commands.InitializeCommands.SetUuidToAllRecord
{
    public class SetUuidToAllRecordCommand : CommandBase<SetUuidToAllRecordResponse>
    {
        public SetUuidToAllRecordCommand(SetUuidToAllRecordRequest request)
        {
            Request = request;
        }

        public SetUuidToAllRecordRequest Request { get; set; }
    }
}
