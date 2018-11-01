using log4net;
using MlCheckStockAPI.Models.ByTender;
using MlCheckStockAPI.ST_Class;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace MlCheckStockAPI.X_Class
{
    public class cUsed
    {

        public mlRESQtaByTdr C_SEToUsed(mlREQQtaByTdrUsed poREQQtaByTdrUsed)
        {
            var oRESQtaByTdrcs = new cResQtaByTdrcs();
            var oSql = new StringBuilder();
            string tResCode;
            string tMessage;
            string tTransStatus = "U";
            try
            {
                var tTransDate = "'" + cCNSP.DTEtByFormat(poREQQtaByTdrUsed.tML_TransDate, "YYYY-MM-DD") + "'";
                oSql.Clear();
                oSql.AppendLine("UPDATE TPSTSalBBYTdr WITH(ROWLOCK)");
                oSql.AppendLine("SET FTSbyStatus='"+ tTransStatus + "' ");
                oSql.AppendLine(",FCScdAmt = '" + poREQQtaByTdrUsed.tML_DiscAmt + "' ");
                oSql.AppendLine("WHERE (FTShdPlantCode = '" + poREQQtaByTdrUsed.tML_PlantCode + "' )");
                oSql.AppendLine("AND (FTTmnNum = '" + poREQQtaByTdrUsed.tML_POSNo + "')");
                oSql.AppendLine("AND (FTShdTransNo = '" + poREQQtaByTdrUsed.tML_TransNo + "' )");
                oSql.AppendLine("AND (FTShdTransType = '" + poREQQtaByTdrUsed.tML_TransType + "' )");
                oSql.AppendLine("AND (FDShdTransDate = " + tTransDate + " )");
                oSql.AppendLine("AND (FTScdBBYNo = '" + poREQQtaByTdrUsed.tML_BBYNo + "' )");
                oSql.AppendLine("AND (FDDateIns = '" + poREQQtaByTdrUsed.tML_DateIns + "' )");
                oSql.AppendLine("AND (FTTimeIns = '" + poREQQtaByTdrUsed.tML_TimeIns + "' )");
                oSql.AppendLine("AND (FNSrcSeqNo = '" + poREQQtaByTdrUsed.tML_SrcSeqNo + "' )");
                oSql.AppendLine("AND (FTSbyStatus = 'R')");
                var nResult = cCNSP.SP_SETnDbTbl(oSql.ToString());
                if (nResult > 0)
                {
                    tResCode = "04";
                    tMessage = "ยืนยันการใช่สิทธิ์ สำเร็จ";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tResCode, tMessage);
                }
                else
                {
                    tResCode = "44";
                    tMessage = "ยืนยันการใช่สิทธิ์ ไม่สำเร็จ";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tResCode, tMessage);
                }
            }
            catch (Exception oEx)
            {
                tResCode = "44";
                tMessage = "Used Error=" + oEx.Message;
                return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tResCode, tMessage);
            }
        }
    }
}