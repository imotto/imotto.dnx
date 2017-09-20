using iMotto.Data.Entities;

namespace iMotto.Adapter.Readers
{
    public class VoteModel:Vote
    {
        public VoteModel(Vote vote)
        {
            ID = vote.ID;
            MID = vote.MID;
            Oppose = vote.Oppose;
            Support = vote.Support;
            TheDay = vote.TheDay;
            UID = vote.UID;
            VoteTime = vote.VoteTime;
            UserName = "佚名";
            UserThumb = string.Empty;
        }

        public string UserName { get; set; }

        public string UserThumb { get; set; }
    }
}
