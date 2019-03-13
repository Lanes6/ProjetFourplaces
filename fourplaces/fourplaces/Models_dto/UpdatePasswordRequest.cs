using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class UpdatePasswordRequest
    {
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }
        
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }

        public UpdatePasswordRequest(string oldMdp, string mdp)
        {
            this.OldPassword = oldMdp;
            this.NewPassword = mdp;
        }
    }
}