using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace iMotto.Events
{
    public class DefaultEventPublisher : IEventPublisher
    {
        private readonly ILogger _logger;
        
        public DefaultEventPublisher(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DefaultEventPublisher>();
        }


        private static readonly Dictionary<Type, List<Action<IEvent>>> _routes = new Dictionary<Type, List<Action<IEvent>>>();

        public void Publish<T>(T @event) where T : IEvent
        {
            List<Action<IEvent>> handlers;

            if (!_routes.TryGetValue(@event.GetType(), out handlers))
            {
                throw new ArgumentOutOfRangeException(
                      string.Format("No handler registed for type: {0}.", @event.GetType()));
            }

            foreach (var handler in handlers)
            {
                var handler1 = handler;
                ThreadPool.QueueUserWorkItem(x => {
                    try
                    {
                        handler1(@event);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("处理{0}事件时发生异常:{1}", @event.GetType().Name, ex);
                    }
                });
            }
        }

        public void RegisterEventHandler<T>(Action<T> handler) where T : IEvent
        {

            List<Action<IEvent>> handlers;

            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IEvent>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }
    }
}
