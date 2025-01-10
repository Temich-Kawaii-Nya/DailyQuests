using GameCore.EventBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCore.EventBus
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<WeakReference<IBaseEventReceiver>>> _eventReceivers = new();
        private readonly Dictionary<int, WeakReference<IBaseEventReceiver>> _eventReceiverHash = new();

        private readonly Dictionary<Type, List<WeakReference<IBaseRequestHandler>>> _requestHandlers = new();
        private readonly Dictionary<int, WeakReference<IBaseRequestHandler>> _requestHandlerHash = new();

        public void RegisterEventReceiver<T>(IEventReceiver<T> receiver) where T : struct, IEvent
        {
            AddReceiver(_eventReceivers, _eventReceiverHash, typeof(T), receiver);
        }

        public void UnregisterEventReceiver<T>(IEventReceiver<T> receiver) where T : struct, IEvent
        {
            RemoveReceiver(_eventReceivers, _eventReceiverHash, typeof(T), receiver);
        }

        public void RegisterRequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            AddReceiver(_requestHandlers, _requestHandlerHash, typeof(TRequest), handler);
        }

        public void UnregisterRequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            RemoveReceiver(_requestHandlers, _requestHandlerHash, typeof(TRequest), handler);
        }

        public Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            if (_requestHandlers.TryGetValue(typeof(TRequest), out var handlers))
            {
                foreach (var reference in handlers)
                {
                    if (reference.TryGetTarget(out var handler))
                    {
                        return ((IRequestHandler<TRequest, TResponse>)handler).HandleAsync(request);
                    }
                }
            }
            return Task.FromResult(default(TResponse));
        }
        public void SendEvent<T>(T eventMessage) where T : struct, IEvent
        {
            if (_eventReceivers.TryGetValue(typeof(T), out var receivers))
            {
                foreach (var reference in receivers)
                {
                    if (reference.TryGetTarget(out var receiver))
                    {
                        ((IEventReceiver<T>)receiver).OnEvent(eventMessage);
                    }
                }
            }
        }
        private void AddReceiver<T>(
            Dictionary<Type, List<WeakReference<T>>> receiverDict,
            Dictionary<int, WeakReference<T>> hashDict,
            Type keyType,
            T receiver) where T : class
        {
            if (!receiverDict.TryGetValue(keyType, out var receivers))
            {
                receivers = new List<WeakReference<T>>();
                receiverDict[keyType] = receivers;
            }

            var weakReference = new WeakReference<T>(receiver);
            receivers.Add(weakReference);
            hashDict[receiver.GetHashCode()] = weakReference;
        }

        private void RemoveReceiver<T>(
            Dictionary<Type, List<WeakReference<T>>> receiverDict,
            Dictionary<int, WeakReference<T>> hashDict,
            Type keyType,
            T receiver) where T : class
        {
            if (receiverDict.TryGetValue(keyType, out var receivers) && hashDict.TryGetValue(receiver.GetHashCode(), out var weakReference))
            {
                receivers.Remove(weakReference);
                hashDict.Remove(receiver.GetHashCode());

                if (!receivers.Any())
                {
                    receiverDict.Remove(keyType);
                }
            }
        }
    }
}
