using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace og.Utils
{
    public class ItemShopStorage
    {
        [JsonProperty("daily1")]
        public ItemInfo daily1 { get; set; }
        public ItemInfo daily2 { get; set; }
        public ItemInfo daily3 { get; set; }
        public ItemInfo daily4 { get; set; }
        public ItemInfo daily5 { get; set; }
        public ItemInfo daily6 { get; set; }
        public ItemInfo featured1 { get; set; }
        public ItemInfo featured2 { get; set; }

        public class ItemInfo
        {
            [JsonProperty("itemGrants")]
            public string[] ItemGrants { get; set; }

            [JsonProperty("price")]
            public int Price { get; set; }

            [JsonProperty("imageUrl")]
            public string ImageUrl { get; set; }
        }
    }
}
