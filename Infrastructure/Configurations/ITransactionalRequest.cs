using MediatR;

namespace Infrastructure.Configurations
{
    public interface ITransactionalRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
