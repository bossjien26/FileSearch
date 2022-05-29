using Enums;

namespace Models.Token
{
    public class IdentityAuthenticate
    {
        public string GroupId { get; set; }

        public string Customer { get; set; }

        public RoleEnum Role { get; set; } = RoleEnum.Customer;

        public string Password { get; set; }
    }
}