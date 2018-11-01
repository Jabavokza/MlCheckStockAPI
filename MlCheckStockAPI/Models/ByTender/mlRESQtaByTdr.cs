using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlCheckStockAPI.Models.ByTender
{
    public class mlRESQtaByTdr
    {
        [JsonProperty("ResCode")]
        public string tML_ResCode { get; set; }
        [JsonProperty("Message")]
        public string tML_Message { get; set; }
        [JsonProperty("DateIns")]
        public string tML_DateIns { get; set; }
        [JsonProperty("TimeIns")]
        public string tML_TimeIns { get; set; }

    }
}