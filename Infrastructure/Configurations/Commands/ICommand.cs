using System;
using MediatR;

namespace Infrastructure.Configurations.Commands
{
    public interface ICommand : IRequest
    {
        Guid CommandId { get; }
    }

    public interface ICommand<out TResult> : IRequest<TResult>
    {
        Guid CommandId { get; }
    }
}
