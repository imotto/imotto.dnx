using iMotto.Events;
using iMotto.Data.Entities.Models;

namespace iMotto.Cache
{
    public interface IDeviceSignatureCache : IEventConsumer<DeviceRegEvent>, IEventConsumer<DisplayNoticeEvent>
    {
        bool IsDeviceActive(string sign);

        DisplayNotice ShouldDisplayNotice(string sign, int theMonth);
    }
}
