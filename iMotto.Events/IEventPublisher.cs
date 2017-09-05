using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Events
{
    public interface IEventPublisher
    {
        void RegisterEventHandler<T>(Action<T> handler) where T : IEvent;

        void Publish<T>(T @event) where T : IEvent;
    }
}
