﻿namespace iMotto.Adapter
{
    public class HandleResult
    {
        public int State { get; set; }

        public string Msg { get; set; }
    }

    public class HandleResult<T>:HandleResult
    {
        public T Data { get; set; }
    }
}