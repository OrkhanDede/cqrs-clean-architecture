using System;

namespace Infrastructure.Configurations.Commands
{
    public abstract class InternalCommandBase : ICommand
    {
        public Guid CommandId { get; }

        protected InternalCommandBase(Guid id)
        {
            this.CommandId = id;
        }
    }

    public abstract class InternalCommandBase<TResult> : ICommand<TResult>
    {
        public Guid CommandId { get; }

        protected InternalCommandBase()
        {
            this.CommandId = Guid.NewGuid();
        }

        protected InternalCommandBase(Guid id)
        {
            this.CommandId = id;
        }
    }
}
