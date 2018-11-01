using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using CacheManager.Core;
using Newtonsoft.Json;
using log4net;
using MlCheckStockAPI.Models.Quota;
using MlCheckStockAPI.ST_Class;
using MlCheckStockAPI.Models;
using MlCheckStockAPI.X_Class;
using System.Text;

namespace MlCheckStockAPI.Controllers
{
    [RoutePrefix("api")]
    public class clQuotaController : ApiController
    {
        private readonly ILog oC_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private mlRESCheckStock oC_RESQta;
        private mlDBStk oC_DBStk = null;

        #region "Quota"
        [Route("Quota")]
        [ResponseType(typeof(mlRESCheckStock))]
        [HttpPost]
        public IHttpActionResult INSxMC([FromBody]mlREQCheckStock poREQCheckStock)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                var tPath = "~/Config/DBStock.json";
                oC_DBStk = new mlDBStk();
                oC_DBStk = JsonConvert.DeserializeObject<mlDBStk>(File.ReadAllText(HttpContext.Current.Server.MapPath(tPath)));

                //Memcach init
                var oMC_Usr = new BaseCacheManager<mlDBStk>(cCNVB.oCMConfig);
                oMC_Usr.AddOrUpdate("key", oC_DBStk, _ => oC_DBStk);
                oC_DBStk = oMC_Usr.Get("key");

                if (ModelState.IsValid)
                {
                    return Json(CNTxBill(poREQCheckStock));
                }
                return Json(ModelState.C_GETtErrorModaleSta());
            }
            catch (Exception oEx)
            {
                return BadRequest(oEx.Message);
            }
        }
        private mlRESCheckStock CNTxBill(mlREQCheckStock poREQCheckStock)
        {
            DataTable oDtCnt = new DataTable();
            StringBuilder oSql = new StringBuilder();
            string tBrc;
            try
            {
                tBrc = poREQCheckStock.tML_PlantCode;
                //Select DB form Brance json
                var aDB = (from oDBStk in oC_DBStk.DatabaseStock
                           where oDBStk.Brance == tBrc
                           select oDBStk).ToList();

                //SET DB string with plant ex='DP'
                cCNVB.tConStr = "Data Source =" + aDB[0].ServerName;
                cCNVB.tConStr = cCNVB.tConStr + Environment.NewLine + ";Initial Catalog = " + aDB[0].DabaseName;
                cCNVB.tConStr = cCNVB.tConStr + Environment.NewLine + ";Persist Security Info=True;User ID = " + aDB[0].User;
                cCNVB.tConStr = cCNVB.tConStr + Environment.NewLine + ";Password =" + aDB[0].Pasword;
                cCNVB.tConStr = cCNVB.tConStr + Environment.NewLine + ";Connection Timeout=15";

                oSql.AppendLine("IF '" + poREQCheckStock.tML_BByProfID + "' = 'P001'");
                oSql.AppendLine("BEGIN ");
                oSql.AppendLine("SELECT ");
                oSql.AppendLine("SUM (CASE FTShdTransType");
                oSql.AppendLine("WHEN '03' THEN FCScdQty");
                oSql.AppendLine("WHEN '07' THEN FCScdQty");
                oSql.AppendLine("WHEN '10' THEN FCScdQty");
                oSql.AppendLine("WHEN '11' THEN FCScdQty");
                oSql.AppendLine("WHEN '13' THEN FCScdQty");
                oSql.AppendLine("WHEN '05' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '17' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '26' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '27' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '28' THEN (-1) * FCScdQty");
                oSql.AppendLine("END)");
                oSql.AppendLine("AS FCSumScdQty");
                oSql.AppendLine("FROM TPSTSalCD");
                oSql.AppendLine("WHERE FNDctNo IN(2,3)"); // Promotion
                oSql.AppendLine("AND FTShdPlantCode  = '" + poREQCheckStock.tML_PlantCode + "'");
                oSql.AppendLine("AND FDShdTransDate BETWEEN '" + poREQCheckStock.tML_StartDate + "' AND '" + poREQCheckStock.tML_EndDate + "' ");
                oSql.AppendLine("AND FTScdBBYNo = '" + poREQCheckStock.tML_BByNo + "'");
                oSql.AppendLine("AND FTSkuCode = '" + poREQCheckStock.tML_SKUCode + "'");
                oSql.AppendLine("GROUP BY FTShdPlantCode,FTScdBBYNo,FTSkuCode");
                oSql.AppendLine("END ");
                oSql.AppendLine("ELSE ");
                oSql.AppendLine("BEGIN ");
                oSql.AppendLine("SELECT ");
                oSql.AppendLine("SUM (CASE FTShdTransType");// SUM(FCScdQty) -- จำนวนสินค้าที่ได้รับ Promition
                oSql.AppendLine("WHEN '03' THEN FCScdQty");// รายการขาย
                oSql.AppendLine("WHEN '07' THEN FCScdQty");
                oSql.AppendLine("WHEN '10' THEN FCScdQty");
                oSql.AppendLine("WHEN '11' THEN FCScdQty");
                oSql.AppendLine("WHEN '13' THEN FCScdQty");
                oSql.AppendLine("WHEN '05' THEN (-1) * FCScdQty"); // รายการยกเลิกบิลขาย ต้อง ลบออก
                oSql.AppendLine("WHEN '17' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '26' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '27' THEN (-1) * FCScdQty");
                oSql.AppendLine("WHEN '28' THEN (-1) * FCScdQty");
                oSql.AppendLine("END)");
                oSql.AppendLine("AS FCSumScdQty");
                oSql.AppendLine("FROM TPSTSalCD");
                oSql.AppendLine("WHERE FNDctNo = 21");// Member Price
                oSql.AppendLine("AND FTShdPlantCode  = '" + poREQCheckStock.tML_PlantCode + "'");
                oSql.AppendLine("AND FDShdTransDate BETWEEN '" + poREQCheckStock.tML_StartDate + "' AND '" + poREQCheckStock.tML_EndDate + "' ");
                oSql.AppendLine("AND FTScdBBYNo = '" + poREQCheckStock.tML_BByNo + "'");
                oSql.AppendLine("AND FTSkuCode = '" + poREQCheckStock.tML_SKUCode + "'");
                oSql.AppendLine("GROUP BY FTShdPlantCode,FTScdBBYNo,FTSkuCode");
                oSql.AppendLine("End");
                oDtCnt = cCNSP.SP_SQLoExecute(oSql.ToString(), cCNVB.tConStr);

                if (oDtCnt.Rows.Count == 0)
                {
                    oC_RESQta = new mlRESCheckStock()
                    {
                        tML_Result = "0",
                        tML_Code = "403",
                        tML_Message = "ค่าเป็น 0"
                    };
                    return oC_RESQta;
                }
                else
                {
                    oC_RESQta = new mlRESCheckStock()
                    {
                        tML_Result = oDtCnt.ToString(),
                        tML_Code = "200",
                        tML_Message = "Success"
                    };
                    return oC_RESQta;
                }
            }
            catch (Exception oEx)
            {
                oC_Log.Error(oEx.Message);
                oC_RESQta = new mlRESCheckStock() { tML_Message = oEx.Message };
                return oC_RESQta;
            }
        }
        #endregion "Quota"
    }
}