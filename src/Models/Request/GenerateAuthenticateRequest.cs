using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class GenerateAuthenticateRequest
    {

        [Required]
        public string GroupId { get; set; }

        [Required]
        public string Project { get; set; }
    }
}