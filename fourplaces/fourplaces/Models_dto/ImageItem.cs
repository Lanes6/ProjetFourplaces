using Newtonsoft.Json;

namespace fourplaces.Models
{
	public class ImageItem
	{
		[JsonProperty("id")]
		public int Id { get; set; }
	}
}