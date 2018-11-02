using MlCheckStockAPI.Models.ByTender;
using MlCheckStockAPI.ST_Class;
using System;
using System.Text;

namespace MlCheckStockAPI.X_Class.QtaByTdr
{
    public class cCancel
    {

        public mlRESQtaByTdr C_SEToCancel(mlREQQtaByTdrCancel poREQQtaByTdrCancel)
        {
            var oRESQtaByTdrcs = new cResMsgQtaByTdrcs();
            var oSql = new StringBuilder();
            string tResCode;
            string tMessage;
            string tTransStatus = "U";
            try
            {
                var tTransDate = "'" + cCNSP.DTEtByFormat(poREQQtaByTdrCancel.tML_TransDate, "YYYY-MM-DD") + "'";
                oSql.Clear();
                oSql.AppendLine("UPDATE TPSTSalBBYTdr WITH(ROWLOCK)");
                oSql.AppendLine("SET FTSbyStatus='" + tTransStatus + "' ");
                oSql.AppendLine("WHERE (FTShdPlantCode = '" + poREQQtaByTdrCancel.tML_PlantCode + "')");
                oSql.AppendLine("AND (FTTmnNum = '" + poREQQtaByTdrCancel.tML_POSNo + "')");
                oSql.AppendLine("AND (FTShdTransNo = '" + poREQQtaByTdrCancel.tML_TransNo + "')");
                oSql.AppendLine("AND (FTShdTransType = '" + poREQQtaByTdrCancel.tML_TransType + "')");
                oSql.AppendLine("AND (FDShdTransDate = " + tTransDate + " )");
                oSql.AppendLine("AND (FTScdBBYNo = '" + poREQQtaByTdrCancel.tML_BBYNo + "')");
                oSql.AppendLine("AND (FDDateIns = '" + poREQQtaByTdrCancel.tML_DateIns + "')");
                oSql.AppendLine("AND (FTTimeIns = '" + poREQQtaByTdrCancel.tML_TimeIns + "')");
                oSql.AppendLine("AND (FNSrcSeqNo = '" + poREQQtaByTdrCancel.tML_SrcSeqNo + "')");
                oSql.AppendLine("AND (FTSbyStatus = 'R')");
                int nResult = cConDbByTender.C_SETnDbTbl(oSql.ToString());
                if (nResult > 0)
                {
                    tResCode = "01";
                    tMessage = "ยกเลิกการจอง สำเร็จ";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tResCode, tMessage);
                }
                else
                {
                    tResCode = "44";
                    tMessage = "ยกเลิกการจอง ไม่สำเร็จ";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tResCode, tMessage);
                }
            }
            catch (Exception ex)
            {
                tResCode = "44";
                tMessage = "[Cancel] Error=" + ex.Message;
                return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tResCode, tMessage);
            }
        }
    }
}