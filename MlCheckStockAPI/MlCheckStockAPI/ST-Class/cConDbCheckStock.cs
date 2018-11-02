using CacheManager.Core;
using MlCheckStockAPI.Models;
using MlCheckStockAPI.Models.Quota;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace MlCheckStockAPI.ST_Class
{
    public static class cConDbCheckStock
    {

        public static DataTable C_SQLoExecute(string ptQeury, string ptPlantCode)
        {
            SqlConnection oConstr = new SqlConnection();
            SqlDataAdapter oDbAdt;
            DataTable oDbTblDt = new DataTable();
            try
            {
                oConstr.ConnectionString = C_GETtDBCheckStock(ptPlantCode);
                oConstr.Open();
                oDbAdt = new SqlDataAdapter(ptQeury, oConstr);
                oDbAdt.Fill(oDbTblDt);
                oConstr.Dispose();
                oDbAdt.Dispose();
                return oDbTblDt;
            }
            catch (Exception ex)
            {
                cCNSP.ADDxLog("API:Excute DataTable  = " + ex.Message);
                throw ex;
            }
            finally
            {
                oDbAdt = null;
            }
        }
        public static string C_GETtDBCheckStock(string ptPlantCode)
        {
            StringBuilder oSql = new StringBuilder();
            try
            {
                var tPath = "~/Config/DBStock.json";
                var oDBStk = new mlDBStk();
                oDBStk = JsonConvert.DeserializeObject<mlDBStk>(File.ReadAllText(HttpContext.Current.Server.MapPath(tPath)));

                //Memcach init
                var oMC_Usr = new BaseCacheManager<mlDBStk>(cCNVB.oCMConfig);
                oMC_Usr.AddOrUpdate("key", oDBStk, _ => oDBStk);
                oDBStk = oMC_Usr.Get("key");

                //Select DB form Brance json
                var aDB = (from oResultDBStk in oDBStk.DatabaseStock
                           where oResultDBStk.Brance == ptPlantCode.Substring(2, 2)
                           select oResultDBStk).ToList();

                oSql.AppendLine("Data Source = " + aDB[0].ServerName + " ");
                oSql.AppendLine(";Initial Catalog =  " + aDB[0].DabaseName + " ");
                oSql.AppendLine(";Persist Security Info=True;User ID =" + aDB[0].User + " ");
                oSql.AppendLine(";Password = " + aDB[0].Pasword + " ");
                oSql.AppendLine(";Connection Timeout=15;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=30;Pooling=true");
                return oSql.ToString();
            }
            catch (Exception oEx)
            {
                throw oEx;
            }

        }

    }
}