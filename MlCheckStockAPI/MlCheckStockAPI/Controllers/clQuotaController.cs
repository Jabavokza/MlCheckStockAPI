using System;
using System.Globalization;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using log4net;
using MlCheckStockAPI.Models.Quota;
using MlCheckStockAPI.ST_Class;
using MlCheckStockAPI.X_Class;
using System.Text;

namespace MlCheckStockAPI.Controllers
{
    [RoutePrefix("api")]
    public class clQuotaController : ApiController
    {
        private readonly ILog oC_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private mlRESCheckStock oC_RESQta;

        #region "Quota"
        [Route("Quota")]
        [ResponseType(typeof(mlRESCheckStock))]
        [HttpPost]
        public IHttpActionResult INSoMC([FromBody]mlREQCheckStock poREQCheckStock)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                if (ModelState.IsValid)
                {
                    return Json(CNToBill(poREQCheckStock));
                }
                return Json(ModelState.C_GETtErrorModaleSta());
            }
            catch (Exception oEx)
            {
                return BadRequest(oEx.Message);
            }
        }
        private mlRESCheckStock CNToBill(mlREQCheckStock poREQCheckStock)
        {
            StringBuilder oSql = new StringBuilder();
            try
            {
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
                var oDtCnt = cConDbCheckStock.C_SQLoExecute(oSql.ToString(),poREQCheckStock.tML_PlantCode);

                if (oDtCnt.Rows.Count == 0)
                {
                    oC_RESQta = new mlRESCheckStock()
                    {
                        tML_Result = "0",
                        tML_Code = "403",
                        tML_Message = "ไม่พบข้อมูล NULL"
                    };
                    return oC_RESQta;
                }
                else
                {
                    oC_RESQta = new mlRESCheckStock()
                    {
                        tML_Result = oDtCnt.Rows[0]["FCSumScdQty"].ToString(),
                        tML_Code = "200",
                        tML_Message = "สำเร็จ Success"
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