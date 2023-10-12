using MediatR;

namespace Infrastructure.Configurations.Queries
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {

    }
}
