using MlCheckStockAPI.Models.ByTender;
using MlCheckStockAPI.ST_Class;
using System;
using System.Data.SqlClient;
using System.Text;

namespace MlCheckStockAPI.X_Class
{
    public class cInsQtaByTdr
    {
        public mlRESQtaByTdr C_INSoTPSTSalBBYTdr(mlREQQtaByTdrReserve poREQQtaByTdrReserve)
        {
            StringBuilder oSql = new StringBuilder();
            cResQtaByTdrcs oRESQtaByTdrcs = new cResQtaByTdrcs();
            string tC_ResCod;
            string tC_Message;
            string tTransStatus = "R";
            try
            {
                string tTransDate = "'" + cCNSP.DTEtByFormat(poREQQtaByTdrReserve.tML_TransDate, "YYYY-MM-DD") + "'";
                oSql.Clear();
                oSql.AppendLine("INSERT INTO TPSTSalBBYTdr WITH(ROWLOCK) (");
                oSql.AppendLine("FTStmCode");
                oSql.AppendLine(",FTTmnNum");
                oSql.AppendLine(",FTShdTransNo");
                oSql.AppendLine(",FTShdTransType");
                oSql.AppendLine(",FDShdTransDate");
                oSql.AppendLine(",FTShdPlantCode");
                oSql.AppendLine(",FTSbyStatus");
                oSql.AppendLine(",FTScdCardID");
                oSql.AppendLine(",FTMcdCardType");
                oSql.AppendLine(",FTScdBBYProfID");
                oSql.AppendLine(",FTScdBBYNo");
                oSql.AppendLine(",FTSbyQuotaType");
                oSql.AppendLine(",FTSbyDayName");
                oSql.AppendLine(",FTSbyQuota");
                oSql.AppendLine(",FNPpmCrdQuota");
                oSql.AppendLine(",FTTdmCode");
                oSql.AppendLine(",FTCdcCreditType");
                oSql.AppendLine(",FCScdAmtB4Avi");
                oSql.AppendLine(",FTWhoIns");
                oSql.AppendLine(",FDDateIns");
                oSql.AppendLine(",FTTimeIns");
                oSql.AppendLine(",FNSrcSeqNo");
                oSql.AppendLine(")VALUES(");
                oSql.AppendLine("'" + poREQQtaByTdrReserve.tML_StmCode + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_POSNo + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_TransNo + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_TransType + "'");
                oSql.AppendLine(",CONVERT(VARCHAR(10), " + tTransDate + ", 121)");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_PlantCode + "'");
                oSql.AppendLine(",'" + tTransStatus + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_McardNo + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_McardType + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_BBYProfID + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_BBYNo + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_BBYQuotaType + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_BBYDayName + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_BBYQuota + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_CrdQuota + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_TdmCode + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_TdmCardType + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_AmtB4Disc + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_UserName + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_DateIns + "'");
                oSql.AppendLine(",'" + poREQQtaByTdrReserve.tML_TimeIns + "'");
                oSql.AppendLine(",'" + int.Parse(poREQQtaByTdrReserve.tML_SrcSeqNo) + "'");
                oSql.AppendLine(")");
                int nResult = cCNSP.SP_SETnDbTbl(oSql.ToString());
                if (nResult > 0)
                {
                    tC_ResCod = "00";
                    tC_Message = "จองสิทธิ์ส่วนลด สำเร็จ";
                    return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message, poREQQtaByTdrReserve.tML_DateIns, poREQQtaByTdrReserve.tML_TimeIns);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (SqlException)
            {
                tC_ResCod = "44";
                tC_Message = "จองสิทธิ์ส่วนลด ไม่สำเร็จ อาจจะมีอยู่แล้ว หรือใช้ไปแล้ว";
                return oRESQtaByTdrcs.C_SEToRESQtaByTdr(tC_ResCod, tC_Message, poREQQtaByTdrReserve.tML_DateIns, poREQQtaByTdrReserve.tML_TimeIns);
            }
        }
    }
}