namespace iMotto.Data.Entities.Models
{
    public class VerifyCode
    {
        public const int TYPE_REGISTER = 1;

        public const int TYPE_RESETPWD = 2;

        public string Mobile { get; set; }

        public string Code { get; set; }

        public int Type { get; set; }
    }
}
