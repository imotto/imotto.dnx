namespace iMotto.Adapter.Users.Requests
{
    class ModifyPasswordRequest:AuthedRequest
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
