using System.Collections.Generic;

namespace iMotto.Data.Entities
{
    public class PrepareExchangeResult
    {
        public int Balance { get; set; }
        public int ReqInfoType { get; set; }
        public string ReqInfoHint { get; set; }
        public List<RelAccount> Accounts { get; set; }
        public List<UserAddress> Addresses { get; set; }
    }
}
