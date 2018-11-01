using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlCheckStockAPI.Models.Quota
{

    public class mlRESCheckStock
    {

        [JsonProperty("Result")]
        public string tML_Result { get; set; }

        [JsonProperty("Code")]
        public string tML_Code { set; get; }

        [JsonProperty("Message")]
        public string tML_Message { set; get; }
    }
}