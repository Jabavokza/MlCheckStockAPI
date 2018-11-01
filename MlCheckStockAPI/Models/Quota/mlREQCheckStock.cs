using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MlCheckStockAPI.Models.Quota
{
    public class mlREQCheckStock
    {
        [JsonProperty("PlantCode")]
        [Required]
        public string tML_PlantCode { get; set; }

        [JsonProperty("BByProfID")]
        [Required]
        public string tML_BByProfID { get; set; }

        [JsonProperty("BByNo")]
        [Required]
        public string tML_BByNo { get; set; }

        [JsonProperty("SKUCode")]
        [Required]
        public string tML_SKUCode { get; set; }

        [JsonProperty("StartDate ")]
        [Required]
        public string tML_StartDate { get; set; }

        [JsonProperty("EndDate")]
        [Required]
        public string tML_EndDate { get; set; }
    }
}
