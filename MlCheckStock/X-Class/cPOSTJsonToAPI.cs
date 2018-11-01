using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MlCheckStock.X_Class
{
    public class cPOSTJsonToAPI
    {
        public string C_POSTtHTTPCliant(string ptUrl, string ptJson)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                // SET Head
                HttpWebRequest oREQ = (HttpWebRequest)WebRequest.Create(ptUrl);
                oREQ.Method = "POST";
                oREQ.ContentType = "application/json";
                // oREQ.Timeout = 9000; //*Tao 61-03-19 เพิ่มการกำหนด TimeOut
                byte[] aReq = new UTF8Encoding().GetBytes(ptJson);
                oREQ.ContentLength = aReq.Length;

                //--------------------------------- REQ Quota -----------------------------------
                using (Stream oREQStream = oREQ.GetRequestStream())
                {
                    oREQStream.Write(aReq, 0, aReq.Length);
                }
                //--------------------------------- RES Quota -----------------------------------
                try
                {
                    HttpWebResponse oRES = (HttpWebResponse)oREQ.GetResponse();
                    using (StreamReader sr = new StreamReader(oRES.GetResponseStream()))
                    {
                        var tRES = sr.ReadToEnd();
                        return tRES;
                    }
                }
                catch (WebException ex)
                {
                    //*Tao 61-03-19 เก็บ Error
                    int nResCode = (int)ex.Status;
                    string tResMsg = ex.Message;
                    string tShwMsg = ex.Message;

                    if (ex.Status == WebExceptionStatus.ProtocolError || ex.Status == WebExceptionStatus.ConnectFailure || ex.Status == WebExceptionStatus.KeepAliveFailure)
                    {
                        var response = ex.Response as HttpWebResponse;
                        if (response != null)
                        {
                            nResCode = (int)response.StatusCode;
                        }
                        else
                        {
                            //oW_Log.FTResCode = nResCode.ToString();
                            //oW_Log.FTResMsg = tResMsg;
                            //oW_Log.FTResShwMsg = tShwMsg;
                        }
                    }
                    else
                    {
                        //oW_Log.FTResCode = nResCode.ToString();
                        //oW_Log.FTResMsg = tShwMsg;
                        //oW_Log.FTResShwMsg = tShwMsg;
                    }
                    //======================
                    throw ex;
                }
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
        }
    }
}
