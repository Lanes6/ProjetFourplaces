using Newtonsoft.Json;

namespace fourplaces.Models
{
	public class CreateCommentRequest
	{
		[JsonProperty("text")]
		public string Text { get; set; }

        public CreateCommentRequest(string text)
        {
            Text = text;
        }
    }
}