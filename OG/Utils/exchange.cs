using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public class exchange
    {
        [JsonProperty("expiresInSeconds")]
        public int ExpiresInSeconds { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("creatingClientId")]
        public string CreatingClientId { get; set; }
    }
}
