namespace iMotto.Data.Entities
{
    public class UserAddress
    {
        public long ID { get; set; }
        public string UID { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string Contact { get; set; }
        public string Mobile { get; set; }
        public bool IsDefault { get; set; }
    }
}
