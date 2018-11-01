using MlCheckStockAPI.Models.ByTender;
using MlCheckStockAPI.ST_Class;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace MlCheckStockAPI.X_Class
{
    public class cReServe
    {
        //private cResQtaByTdrcs oC_RESQtaByTdrcs = new cResQtaByTdrcs();
        //private cInsQtaByTdr oC_INSQtaByTdr = new cInsQtaByTdr();
        //private StringBuilder oC_Sql = new StringBuilder();
        //private DataTable oC_DtQuota = new DataTable();
        private string tC_ResCod;
        private string tC_Message;
        public mlRESQtaByTdr C_SEToReServe(mlREQQtaByTdrReserve poREQQtaByTdrReserve)
        {
            var oRESQtaByTdrcs = new cResQtaByTdrcs();
            var oSql = new StringBuilder();
            try
            {               
                if (poREQQtaByTdrReserve.tML_BBYQuota == "0" || poREQQtaByTdrReserve.tML_BBYQuota == "")// กันเหนียวไว้ก่อน
                {
                    tC_ResCod = "03";
                    tC_Message = "จำนวน Quota เกิน Limit ที่กำหนด";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
                }
                switch (poREQQtaByTdrReserve.tML_BBYQuotaType)
                {
                    case "1": //Per Bonus Buy

                        oSql = C_GEToCountTransNo(poREQQtaByTdrReserve);
                        oSql.AppendLine("GROUP BY FTScdBBYNo");
                        return C_CHKoQuota(oSql.ToString(), poREQQtaByTdrReserve);

                    case "2": //Per Store    

                        oSql = C_GEToCountTransNo(poREQQtaByTdrReserve);
                        oSql.AppendLine("AND (FTStmCode = '" + poREQQtaByTdrReserve.tML_StmCode + "')");
                        oSql.AppendLine("GROUP BY FTStmCode, FTScdBBYNo");
                        return C_CHKoQuota(oSql.ToString(), poREQQtaByTdrReserve);

                    case "3": //per Plant

                        oSql = C_GEToCountTransNo(poREQQtaByTdrReserve);
                        oSql.AppendLine("AND (FTShdPlantCode = '" + poREQQtaByTdrReserve.tML_PlantCode + "')");
                        oSql.AppendLine("GROUP BY FTShdPlantCode, FTScdBBYNo");
                        return C_CHKoQuota(oSql.ToString(), poREQQtaByTdrReserve);

                    case "4": //per Day 

                        oSql = C_GEToCountTransNo(poREQQtaByTdrReserve);
                        oSql.AppendLine("AND (FTShdPlantCode = '" + poREQQtaByTdrReserve.tML_PlantCode + "')");
                        oSql.AppendLine("AND (DATENAME(dw, FDShdTransDate) = '" + poREQQtaByTdrReserve.tML_BBYDayName + "')");
                        oSql.AppendLine("GROUP BY FTScdBBYNo, DATENAME(dw, FDShdTransDate)");
                        return C_CHKoQuota(oSql.ToString(), poREQQtaByTdrReserve);

                    default:
                        tC_ResCod = "44";
                        tC_Message = "ข้อมูล BBYQuotaType ไม่ถูกต้อง";
                        return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
                }
            }
            catch (Exception oEx)
            {
                tC_ResCod = "44";
                tC_Message = "[ReServe : C_SEToReServe] Error=" + oEx.Message;
                return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
            }
        }
        private StringBuilder C_GEToCountTransNo(mlREQQtaByTdrReserve poREQQtaByTdrReserve)
        {
            var oSql = new StringBuilder();
            try
            {
                oSql.AppendLine("SELECT COUNT(FTShdTransNo)  AS FNCnt ");
                oSql.AppendLine(",SUM (CASE WHEN ''='" + poREQQtaByTdrReserve.tML_McardNo + "' THEN 0");
                oSql.AppendLine("WHEN FTScdCardID = '" + poREQQtaByTdrReserve.tML_McardNo + "' THEN 1 ELSE 0 END) AS FNIDCnt ");
                oSql.AppendLine("FROM TPSTSalBBYTdr WITH(NOLOCK)");
                oSql.AppendLine("WHERE (FTScdBBYNo = '" + poREQQtaByTdrReserve.tML_BBYNo + "')");
                oSql.AppendLine("AND (FDShdTransDate BETWEEN '" + poREQQtaByTdrReserve.tML_BBYStartDate + "' AND '" + poREQQtaByTdrReserve.tML_BBYEndDate + "' )");
                oSql.AppendLine("AND (FTSbyStatus <> 'C')");
                return oSql;
            }
            catch (SqlException oEx)
            {
                throw oEx;
            }
        }
        private mlRESQtaByTdr C_CHKoQuota(string ptSql, mlREQQtaByTdrReserve poREQQtaByTdrReserve)
        {
            var oRESQtaByTdrcs = new cResQtaByTdrcs();
            var oINSQtaByTdr = new cInsQtaByTdr();
            try
            {
                // Check Quota By Exprie
                var oTransDate = DateTime.Parse(poREQQtaByTdrReserve.tML_TransDate);
                var oStartDate = DateTime.Parse(poREQQtaByTdrReserve.tML_BBYStartDate);
                var oEndDate = DateTime.Parse(poREQQtaByTdrReserve.tML_BBYEndDate);
                if (oTransDate < oStartDate || oTransDate > oEndDate)
                {
                    tC_ResCod = "45";
                    tC_Message = "ไม่อยู่ในช่วงโปรโมชั่น";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
                }

                //Check Quota By Count
                var oDtQuota = cCNSP.SP_GEToDbTbl(ptSql);
                if (oDtQuota.Rows.Count != 0)
                {
                    // Count By CrdQuota
                    if (int.Parse(oDtQuota.Rows[0]["FNIDCnt"].ToString()) >= int.Parse(poREQQtaByTdrReserve.tML_CrdQuota))
                    {
                        tC_ResCod = "03";
                        tC_Message = "CrdQuota Limit  บัตรใช้งาน ครบจำนวนแล้ว";
                        return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
                    }
                    // Count By TransNo
                    else if (int.Parse(oDtQuota.Rows[0]["FNCnt"].ToString()) >= int.Parse(poREQQtaByTdrReserve.tML_BBYQuota))
                    {
                        tC_ResCod = "03";
                        tC_Message = "จำนวน Quota เกิน Limit";
                        return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
                    }
                    else
                    {
                        //เจอรหัสโปรโมรชั่น และยังไม่หมดโครต้า ให้ทำการจอง
                        return oINSQtaByTdr.C_INSoTPSTSalBBYTdr(poREQQtaByTdrReserve);
                    }
                }
                else
                {
                    //ไม่เจอรหัสโปรโมรชั่น ให้ทำการจอง
                    return oINSQtaByTdr.C_INSoTPSTSalBBYTdr(poREQQtaByTdrReserve);
                }
            }
            catch (Exception oEx)
            {
                tC_ResCod = "44";
                tC_Message = "[ReServe : C_CHKoQuota] Error=" + oEx.Message;
                return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message);
            }
        }
    }
}

