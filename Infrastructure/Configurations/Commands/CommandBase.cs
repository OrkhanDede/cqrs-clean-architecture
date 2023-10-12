using System;

namespace Infrastructure.Configurations.Commands
{
    public class CommandBase : ICommand
    {
        public Guid CommandId { get; }

        public CommandBase()
        {
            this.CommandId = Guid.NewGuid();
        }

        protected CommandBase(Guid commandId)
        {
            this.CommandId = commandId;
        }
    }

    public abstract class CommandBase<TResult> : ICommand<TResult>
    {
        public Guid CommandId { get; }

        protected CommandBase()
        {
            this.CommandId = Guid.NewGuid();
        }

        protected CommandBase(Guid commandId)
        {
            this.CommandId = commandId;
        }
    }
}
