using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace fourplaces.Models
{
    public class FormFile
    {
        [JsonProperty("contentType")]
        public string contentType { get; set; }
        [JsonProperty("contentDisposition")]
        public string contentDisposition { get; set; }
        [JsonProperty("headers")]
        public List<string> headers { get; set; }
        [JsonProperty("length")]
        public long length { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("fileName")]
        public string fileName { get; set; }
    }
}
