﻿namespace iMotto.Adapter.Readers.Requests
{
    class ReadRecentTalkRequest:AuthedRequest
    {
        public int PIndex { get; set; }

        public int PSize { get; set; }
    }
}
