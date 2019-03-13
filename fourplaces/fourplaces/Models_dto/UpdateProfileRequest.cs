using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class UpdateProfileRequest
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        
        [JsonProperty("image_id")]
        public int? ImageId { get; set; }

        public UpdateProfileRequest(string firstname, string lastName, int? imageId)
        {
            FirstName = firstname;
            LastName = lastName;
            ImageId = imageId;
        }
    }
}