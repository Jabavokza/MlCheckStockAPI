using log4net;
using MlCheckStock.Models.Quota;
using MlCheckStock.X_Class;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MlCheckStock.X_Class
{
    #region "COM"
    [Guid("A09025A8-F498-4A74-B8AF-A1B1D7367D11")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    [ComVisibleAttribute(true)]
    public interface IQuota
    {
        //Method
        [DispId(1)] string GETtVertionDll();
        [DispId(2)] bool GETtQuota(string ptUrl, string ptPlantCode, string ptBByProfID, string ptBByNo, string ptSkuCode, string ptStartDate, string ptEndDate);
        [DispId(3)] bool GETbResult();
        [DispId(4)] string GETtResult();
        [DispId(5)] string GETtResCode();
        [DispId(6)] string GETtResMessage();

    }
    #endregion
    public class cCallCheckStock
    {
        private cPOSTJsonToAPI oC_POSTJsonToAPI = new cPOSTJsonToAPI();
        private mlRESCheckStock oRESCheckStock = new mlRESCheckStock();

        public string GETxQuota(string ptUrl, string ptPlantCode, string ptBByProfID, string ptBByNo, string ptSkuCode, string ptStartDate, string ptEndDate)
        {
            try
            {
                mlREQCheckStock oREQCheckStock = new mlREQCheckStock()
                {
                    tML_PlantCode = ptPlantCode,
                    tML_BByProfID = ptBByProfID,
                    tML_BByNo = ptBByNo,
                    tML_SKUCode = ptSkuCode,
                    tML_StartDate = ptStartDate,
                    tML_EndDate = ptEndDate
                };
                var tJson = JsonConvert.SerializeObject(oREQCheckStock, Formatting.Indented);
                var tResultMsg = oC_POSTJsonToAPI.C_POSTtHTTPCliant(ptUrl, tJson);
                oRESCheckStock = JsonConvert.DeserializeObject<mlRESCheckStock>(tResultMsg);
                var tResult = oRESCheckStock.tML_Result + "|" + oRESCheckStock.tML_Code + "|" + oRESCheckStock.tML_Message;
                return tResult;
            }
            catch (Exception oEx)
            {
                return oEx.Message;
            }

        }

    }
}
