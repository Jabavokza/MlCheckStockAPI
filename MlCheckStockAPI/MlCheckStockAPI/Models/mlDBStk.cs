using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MlCheckStockAPI.Models
{
    public class mlDBStk
    {
        [JsonProperty("DatabaseStock")]
        public IList<DatabaseStock> DatabaseStock { get; set; }
    }
    public class DatabaseStock
    {
        [JsonProperty("Brance")]
        public string Brance { get; set; }

        [JsonProperty("ServerName")]
        public string ServerName { get; set; }

        [JsonProperty("DabaseName")]
        public string DabaseName { get; set; }

        [JsonProperty("User")]
        public string User { get; set; }

        [JsonProperty("Pasword")]
        public string Pasword { get; set; }
    }


}