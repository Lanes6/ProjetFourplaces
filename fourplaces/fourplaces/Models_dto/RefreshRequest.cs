using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class RefreshRequest
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        public RefreshRequest(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}