using System;
using Enums;

namespace Models.Token
{
    public class IdentityAuthenticate
    {
        public string GroupId { get; set; }

        public string Project { get; set; }

        public string Password { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        public RoleEnum Role { get; set; } = RoleEnum.Customer;
    }
}