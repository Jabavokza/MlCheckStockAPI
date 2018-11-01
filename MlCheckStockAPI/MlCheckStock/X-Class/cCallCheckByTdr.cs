using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Net.Security;
using log4net;
using log4net.Config;
using MlCheckStock.X_Class;
using MlCheckStock.Models.RESMsg;
using MlCheckStock.Models.ByTender;

namespace MlCheckStock.X_Class
{
    #region "COM"
    [Guid("50293EFA-984E-4B61-821B-2C1D526592D6")]

    [ComVisibleAttribute(true)]
    #endregion
    public class cCallCheckByTdr
    {
        private static readonly ILog oC_Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private cPOSTJsonToAPI oC_POSTJsonToAPI = new cPOSTJsonToAPI();
        public string GETtVertionDll()
        {

            //var oLogRes = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(oLogRes, new FileInfo("log4net.config"));

            oC_Log.Debug("TestNutto");
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public string GETtReserve(string ptUrl,string ptStmCode, string ptPOSNo, string ptTransNo, string ptTransType, string ptTransDate
            , string ptPlantCode, string ptMcardNo, string ptMcardType, string ptBBYProfID
            , string ptBBYNo, string ptBBYStartDate, string ptBBYEndDate, string ptBBYQuotaType, string ptBBYDayName
            , string ptBBYQuota, string ptCrdQuota, string ptTdmCode, string ptTdmCardType, string ptAmtB4Disc
            , string ptUserName, string ptDateIns, string ptTimeIns, string ptSrcSeqNo
            )
        {
            try
            {
                //-----------------------------------------------Reserve------------------------------------------//
                mlREQQtaByTdrReserve oREQQtaByTdrReserve = new mlREQQtaByTdrReserve()
                {
                    tML_StmCode = ptStmCode,
                    tML_POSNo = ptPOSNo,
                    tML_TransNo = ptTransNo,
                    tML_TransType = ptTransType,
                    tML_TransDate = ptTransDate,

                    tML_PlantCode = ptPlantCode,
                    tML_McardNo = ptMcardNo,
                    tML_McardType = ptMcardType,
                    tML_BBYProfID = ptBBYProfID,

                    tML_BBYNo = ptBBYNo,
                    tML_BBYStartDate = ptBBYStartDate,
                    tML_BBYEndDate = ptBBYEndDate,
                    tML_BBYQuotaType = ptBBYQuotaType,
                    tML_BBYDayName = ptBBYDayName,

                    tML_BBYQuota = ptBBYQuota,
                    tML_CrdQuota = ptCrdQuota,

                    tML_TdmCode = ptTdmCode,
                    tML_TdmCardType = ptTdmCardType,
                    tML_AmtB4Disc = ptAmtB4Disc,

                    tML_UserName = ptUserName,

                    tML_DateIns = ptDateIns,
                    tML_TimeIns = ptTimeIns,

                    tML_SrcSeqNo = ptSrcSeqNo
                };
                var tJson = JsonConvert.SerializeObject(oREQQtaByTdrReserve, Formatting.Indented);
                var tResultMsg = oC_POSTJsonToAPI.C_POSTtHTTPCliant(ptUrl + "/Reserve", tJson);
                var oRESQtaByTdr = JsonConvert.DeserializeObject<mlRESQtaByTdr>(tResultMsg);
                var tResult = oRESQtaByTdr.tML_ResCode + "|" + oRESQtaByTdr.tML_Message + "|" + oRESQtaByTdr.tML_DateIns + "|" + oRESQtaByTdr.tML_TimeIns;
                return tResult;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
        }

        public string GETtUsed(string ptUrl, string ptPOSNo, string ptTransNo, string ptTransType, string ptTransDate
           , string ptPlantCode, string ptBBYNo, string ptDiscAmt, string ptDateIns, string ptTimeIns, string ptSrcSeqNo)
        {
            //-------------------------------------------Used------------------------------------------------//
            try
            {
                mlREQQtaByTdrUsed oREQQtaByTdrUsed = new mlREQQtaByTdrUsed()
                {
                    tML_POSNo = ptPOSNo,
                    tML_TransNo = ptTransNo,
                    tML_TransType = ptTransType,
                    tML_TransDate = ptTransDate,
                    tML_PlantCode = ptPlantCode,
                    tML_BBYNo = ptBBYNo,
                    tML_DiscAmt = ptDiscAmt,

                    tML_DateIns = ptDateIns,
                    tML_TimeIns = ptTimeIns,

                    tML_SrcSeqNo = ptSrcSeqNo
                };
                var tJson = JsonConvert.SerializeObject(oREQQtaByTdrUsed, Formatting.Indented);
                var tResultMsg = oC_POSTJsonToAPI.C_POSTtHTTPCliant(ptUrl + "/Used", tJson);
                var oRESQtaByTdr = JsonConvert.DeserializeObject<mlRESQtaByTdr>(tResultMsg);
                var tResult = oRESQtaByTdr.tML_ResCode + "|" + oRESQtaByTdr.tML_Message + "|" + oRESQtaByTdr.tML_DateIns + "|" + oRESQtaByTdr.tML_TimeIns;
                return tResult;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }

        }

        public string GETtCancel(string ptUrl, string ptPOSNo, string ptTransNo, string ptTransType, string ptTransDate
         , string ptPlantCode, string ptBBYNo, string ptDateIns, string ptTimeIns, string ptSrcSeqNo)
        {
            //-------------------------------------------------Cancel-----------------------------------------------//
            try
            {
                mlREQQtaByTdrCancel oREQQtaByTdrCancel = new mlREQQtaByTdrCancel()
                {
                    tML_POSNo = ptPOSNo,
                    tML_TransNo = ptTransNo,
                    tML_TransType = ptTransType,
                    tML_TransDate = ptTransDate,
                    tML_PlantCode = ptPlantCode,
                    tML_BBYNo = ptBBYNo,

                    tML_DateIns = ptDateIns,
                    tML_TimeIns = ptTimeIns,

                    tML_SrcSeqNo = ptSrcSeqNo
                };
                var tJson = JsonConvert.SerializeObject(oREQQtaByTdrCancel, Formatting.Indented);
                var tResultMsg = oC_POSTJsonToAPI.C_POSTtHTTPCliant(ptUrl + "/Cancel", tJson);
                var oRESQtaByTdr = JsonConvert.DeserializeObject<mlRESQtaByTdr>(tResultMsg);
                var tResult = oRESQtaByTdr.tML_ResCode + "|" + oRESQtaByTdr.tML_Message + "|" + oRESQtaByTdr.tML_DateIns + "|" + oRESQtaByTdr.tML_TimeIns;
                return tResult;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
        }
    }
}
