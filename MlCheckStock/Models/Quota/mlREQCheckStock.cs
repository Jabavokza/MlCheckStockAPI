using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MlCheckStock.Models.Quota
{
    public class mlREQCheckStock
    {
        [JsonProperty("PlantCode")]
        public string tML_PlantCode { get; set; }

        [JsonProperty("BByProfID")]
        public string tML_BByProfID { get; set; }

        [JsonProperty("BByNo")]
        public string tML_BByNo { get; set; }

        [JsonProperty("SKUCode")]
        public string tML_SKUCode { get; set; }

        [JsonProperty("StartDate ")]
        public string tML_StartDate { get; set; }

        [JsonProperty("EndDate")]
        public string tML_EndDate { get; set; }
    }
}
