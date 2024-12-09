using GameCore.EventBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCore.EventBus 
{
    public class EventBus
    {
        private Dictionary<Type, List<WeakReference<IBaseEventReceiver>>> _eventReceivers;
        private Dictionary<int, WeakReference<IBaseEventReceiver>> _referencesHash;

        private Dictionary<Type, List<WeakReference<IBaseRequestHandler>>> _requestReceivers;
        private Dictionary<int, WeakReference<IBaseRequestHandler>> _requestRefsHash;
        public EventBus()
        {
            _eventReceivers = new Dictionary<Type, List<WeakReference<IBaseEventReceiver>>>();
            _referencesHash = new Dictionary<int, WeakReference<IBaseEventReceiver>>();
            _requestReceivers = new Dictionary<Type, List<WeakReference<IBaseRequestHandler>>>();
            _requestRefsHash = new Dictionary<int, WeakReference<IBaseRequestHandler>>();
        }
        public void Register<T>(IEventReceiver<T> receiver) where T : struct, IEvent
        {
            Type eventType = typeof(T);

            if (!_eventReceivers.ContainsKey(eventType))
                _eventReceivers[eventType] = new List<WeakReference<IBaseEventReceiver>>();

            WeakReference<IBaseEventReceiver> reference = new(receiver);

            _eventReceivers[eventType].Add(reference);
            _referencesHash[receiver.GetHashCode()] = reference;
        }
        public void Unregister<T>(IEventReceiver<T> receiver) where T : struct, IEvent
        {
            Type eventType = typeof(T);
            int hash = receiver.GetHashCode();

            if (!(_eventReceivers.ContainsKey(eventType) || _referencesHash.ContainsKey(hash)))
                return;

            WeakReference<IBaseEventReceiver> reference = _referencesHash[receiver.GetHashCode()];
            _eventReceivers[eventType].Remove(reference);
            _referencesHash.Remove(hash);
        }
        public void RegisterRequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest
        {
            var type = typeof(TRequest);

            if (!_requestReceivers.ContainsKey(type))
                _requestReceivers[type] = new List<WeakReference<IBaseRequestHandler>>();
            WeakReference<IBaseRequestHandler> reference = new(handler);

            _requestReceivers[type].Add(reference);
            _requestRefsHash[handler.GetHashCode()] = reference;
        }
        public void UnregisterRequestHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest
        {
            var type = typeof(TRequest);
            var hash = handler.GetHashCode();

            if (!_requestReceivers.ContainsKey(type) || !_requestRefsHash.ContainsKey(hash))
                return;
            WeakReference<IBaseRequestHandler> reference = new(handler);

            _requestReceivers[type].Remove(reference);
            _requestRefsHash.Remove(hash);
        }
        public Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            var type = typeof(TRequest);
            if (_requestReceivers.ContainsKey(type))
            {
                Debug.Log("ddS");
                foreach (var reference in _requestReceivers[type])
                {
                    Debug.Log("ssS");
                    if (reference.TryGetTarget(out var handler))
                    {
                        Debug.Log("HES");
                        return ((IRequestHandler<TRequest, TResponse>)handler).HandleAsync(request);
                    }
                }
            }
            return Task.FromResult(default(TResponse));
        }
        public void TriggerEvent<T>(T eventMessage) where T : struct, IEvent
        {
            Type eventType = typeof(T);

            if (!_eventReceivers.ContainsKey(eventType))
                return;

            foreach (var reference in _eventReceivers[eventType])
            {
                if (reference.TryGetTarget(out IBaseEventReceiver receiver))
                {
                    ((IEventReceiver<T>)receiver).OnEvent(eventMessage);
                }
            }
        }
    }
}

