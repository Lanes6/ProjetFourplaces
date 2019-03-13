using Newtonsoft.Json;

namespace fourplaces.Models
{
	public class RegisterRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
		
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		
		[JsonProperty("last_name")]
		public string LastName { get; set; }
		
		[JsonProperty("password")]
		public string Password { get; set; }

        public RegisterRequest(string mail, string fisrtName, string lastName, string mdp)
        {
            Email = mail;
            FirstName = fisrtName;
            LastName = lastName;
            Password = mdp;
        }
    }
}