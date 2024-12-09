using System.Threading.Tasks;

namespace GameCore.EventBus.Messaging
{
    public interface IRequestHandler<TRequest, TResponse> : IBaseRequestHandler where TRequest : IRequest
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}

