using Newtonsoft.Json;

namespace fourplaces.Models
{
	public class ImageItem2
	{
		public int Id { get; set; }
        public string Url { get; set; }

        public ImageItem2(int id, string url)
        {
            Id = id;
            Url = url;
        }
    }
}