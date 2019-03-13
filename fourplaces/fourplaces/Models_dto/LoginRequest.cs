using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class LoginRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginRequest(string mail, string mdp)
        {
            Email = mail;
            Password = mdp;
        }
    }
}