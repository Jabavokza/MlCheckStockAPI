using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlCheckStockAPI.Models.ByTender
{
 
    public class mlREQQtaByTdrReserve
    {
        //[JsonProperty("TransStatus")]
        //public string tML_TransStatus { get; set; }
        [JsonProperty("StmCode")]
        public string tML_StmCode { get; set; }

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

        [JsonProperty("McardNo")]
        public string tML_McardNo { get; set; }

        [JsonProperty("McardType")]
        public string tML_McardType { get; set; }

        [JsonProperty("BBYProfID")]
        public string tML_BBYProfID { get; set; }

        [JsonProperty("BBYNo")]
        public string tML_BBYNo { get; set; }

        [JsonProperty("BBYStartDate")]
        public string tML_BBYStartDate { get; set; }

        [JsonProperty("BBYEndDate")]
        public string tML_BBYEndDate { get; set; }

        [JsonProperty("BBYQuotaType")]
        public string tML_BBYQuotaType { get; set; }

        [JsonProperty("BBYDayName")]
        public string tML_BBYDayName { get; set; }

        [JsonProperty("BBYQuota")]
        public string tML_BBYQuota { get; set; }

        [JsonProperty("CrdQuota")]
        public string tML_CrdQuota { get; set; }

        [JsonProperty("TdmCode")]
        public string tML_TdmCode { get; set; }

        [JsonProperty("TdmCardType")]
        public string tML_TdmCardType { get; set; }

        [JsonProperty("AmtB4Disc")]
        public string tML_AmtB4Disc { get; set; }

        [JsonProperty("UserName")]
        public string tML_UserName { get; set; }

        [JsonProperty("DateIns")]
        public string tML_DateIns { get; set; }

        [JsonProperty("TimeIns")]
        public string tML_TimeIns { get; set; }

        [JsonProperty("SrcSeqNo")]
        public string tML_SrcSeqNo { get; set; }
    }
}