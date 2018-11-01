using log4net;
using MlCheckStockAPI.Models.ByTender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MlCheckStockAPI.X_Class
{
    public class cResQtaByTdrcs
    {
        private readonly ILog oC_Log = LogManager.GetLogger("Err");

        public mlRESQtaByTdr C_SEToRESQtaByTdr(string ptResCode,string ptMessage)
        {
            try
            {
                var oC_RESQtaByTdrcs = new mlRESQtaByTdr();
                oC_RESQtaByTdrcs.tML_ResCode = ptResCode;
                oC_RESQtaByTdrcs.tML_Message = ptMessage;
                oC_RESQtaByTdrcs.tML_DateIns = DateTime.Now.ToString("dd/MM/yyyy");
                oC_RESQtaByTdrcs.tML_TimeIns = DateTime.Now.ToString("HH:mm:ss");
                oC_Log.Error(ptMessage);

                return oC_RESQtaByTdrcs;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
           
        }
        public mlRESQtaByTdr C_SEToRESQtaByTdr(string ptResCode, string ptMessage,string ptDateIns,string ptTimeIns)
        {
            try
            {
                var oC_RESQtaByTdrcs = new mlRESQtaByTdr();
                oC_RESQtaByTdrcs.tML_ResCode = ptResCode;
                oC_RESQtaByTdrcs.tML_Message = ptMessage;
                oC_RESQtaByTdrcs.tML_DateIns = ptDateIns;
                oC_RESQtaByTdrcs.tML_TimeIns = ptTimeIns;
                oC_Log.Error(ptMessage);
                return oC_RESQtaByTdrcs;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }

        }
    }
}