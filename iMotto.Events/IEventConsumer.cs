using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMotto.Events
{
    public interface IEventConsumer<T> where T : IEvent
    {
        void HandleEvent(T @event);
    }
}
