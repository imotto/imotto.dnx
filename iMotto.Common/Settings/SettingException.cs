using System;

namespace iMotto.Common.Settings
{
    public class SettingException:Exception
    {
        public SettingException() : base()
        {

        }

        public SettingException(string msg) : base(msg)
        {

        }

        public SettingException(string msg, Exception innerException) : base(msg, innerException)
        {

        }
    }
}
