using System;
using System.Collections.Generic;

namespace iMotto.Data.Entities
{
    public class IdentityUser
    {
        public virtual string Id { get; set; }
        public virtual string UserName { get; set; }
        public string DisplayName { get; set; }

        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        public virtual bool PhoneNumberConfirmed { get; set; }

        public int AccessFailedCount { get; set; }

        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTime? LockoutEndDate { get; set; }
        public bool TwoFactorAuthEnabled { get; set; }

        public List<string> Roles { get; set; }

        public string Thumb { get; set; }

        public int Sex { get; set; }
    }
}
