using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace MlCheckStock.Models.RESMsg
{
    public class mlREQQtaByTdrUsed
    {
        //[JsonProperty("TransStatus")]
        //public string tML_TransStatus { get; set; }

        [JsonProperty("POSNo")]
        public string tML_POSNo { get; set; }

        [JsonProperty("TransNo")]
        public string tML_TransNo { get; set; }

        [JsonProperty("TransType")]
        public string tML_TransType { get; set; }

        [JsonProperty("TransDate")]
        public string tML_TransDate { get; set; }

        [JsonProperty("PlantCode")]
        public string tML_PlantCode { get; set; }

        [JsonProperty("BBYNo")]
        public string tML_BBYNo { get; set; }

        [JsonProperty("DiscAmt")]
        public string tML_DiscAmt { get; set; }

        [JsonProperty("DateIns")]
        public string tML_DateIns { get; set; }

        [JsonProperty("TimeIns")]
        public string tML_TimeIns { get; set; }

        [JsonProperty("SrcSeqNo")]
        public string tML_SrcSeqNo { get; set; }
    }
}