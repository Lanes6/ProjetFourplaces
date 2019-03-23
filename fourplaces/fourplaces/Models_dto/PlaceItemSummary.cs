using System;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;

namespace fourplaces.Models
{
	public class PlaceItemSummary : IComparable<PlaceItemSummary>
    {
		[JsonProperty("id")]
		public int Id { get; set; }
		
		[JsonProperty("title")]
		public string Title { get; set; }
		
		[JsonProperty("description")]
		public string Description { get; set; }
		
		[JsonProperty("image_id")]
		public int ImageId { get; set; }
		
		[JsonProperty("latitude")]
		public double Latitude { get; set; }
		
		[JsonProperty("longitude")]
		public double Longitude { get; set; }

        public string ImageUrl { get; set; }

        public double Distance { get; set; }

        public PlaceItemSummary(int id, string title, string description, int image, double longitude, double latitude)
        {
            Id = id;
            Title = title;
            Description = description;
            ImageId = image;
            Latitude = latitude;
            Longitude = longitude;
            Distance = 1.0;
        }

        public void calculPos()
        {
            Distance = Barrel.Current.Get<Position>(key: "Localisation").CalculateDistance(new Position(Latitude, Longitude));
        }

        public int CompareTo(PlaceItemSummary other)
        {
            if (other == null)
                return 1;
            else
                return this.Distance.CompareTo(other.Distance);
        }
    }
}