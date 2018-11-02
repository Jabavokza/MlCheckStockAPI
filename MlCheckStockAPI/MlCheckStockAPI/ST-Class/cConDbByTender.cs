using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;

namespace MlCheckStockAPI.ST_Class
{
    public static class cConDbByTender
    {
        #region DbByTdr
        /// <summary>
        /// cConnectDbByTdr
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public static DataTable C_GEToDbTbl(string ptSql)
        {
            SqlConnection oDbCon = new SqlConnection();
            DataTable oDbTbl = new DataTable();
            SqlDataAdapter oDbAdt;
            try
            {
                oDbCon.ConnectionString = C_GETtDBByTdr();
                oDbCon.Open();
                oDbAdt = new SqlDataAdapter(ptSql, oDbCon);
                oDbAdt.Fill(oDbTbl);
                oDbCon.Close();
                return oDbTbl;
            }
            catch (Exception oEx)
            {
                cCNSP.ADDxLog("API:Excute DataTable  = " + oEx.Message);
                throw oEx;
            }
            finally
            {
                oDbCon = null;
                oDbAdt = null;
            }
        }
        public static int C_SETnDbTbl(string ptSql)
        {
            SqlConnection oDbCon = new SqlConnection();
            SqlCommand oDbCmd = new SqlCommand();
            DataTable oDbTbl = new DataTable();
            try
            {
                oDbCon.ConnectionString = C_GETtDBByTdr();
                oDbCon.Open();
                oDbCmd.Connection = oDbCon;
                oDbCmd.CommandText = ptSql;
                var nResult = oDbCmd.ExecuteNonQuery();
                oDbCon.Close();
                return nResult;
            }
            catch (Exception oEx)
            {
                cCNSP.ADDxLog("API:Excute DataTable  = " + oEx.Message);
                throw oEx;
            }
            finally
            {
                oDbCon = null;
                oDbCmd = null;
            }
        }
        public static string C_GETtDBByTdr()
        {
            string tServer = "";
            string tLogIn = "";
            string tPws = "";
            string tDBName = "";
            string tPath = "~/Config/DBByTdr.xml";
            try
            {
                XmlDocument oDoc = new XmlDocument();
                FileStream oRead = new FileStream(HttpContext.Current.Server.MapPath(tPath), FileMode.Open, FileAccess.Read, FileShare.Read);

                oDoc.Load(oRead);
                XmlNodeList oElemList = oDoc.GetElementsByTagName("DB");
                foreach (XmlNode node in oElemList)
                {
                    XmlElement oXmlEmt = (XmlElement)node;
                    tServer = oXmlEmt.GetElementsByTagName("ServerName")[0].InnerText;
                    tLogIn = oXmlEmt.GetElementsByTagName("User")[0].InnerText;
                    tPws = oXmlEmt.GetElementsByTagName("Password")[0].InnerText;
                    tDBName = oXmlEmt.GetElementsByTagName("DataBaseName")[0].InnerText;
                    cCNVB.tConDBName = "DB Main:" + tDBName;
                    cCNVB.tDBNameM = tDBName;
                }
                oRead.Flush();
                oRead.Close();
                string tCon = "";
                tCon = "Data Source =" + tServer;
                tCon = tCon + Environment.NewLine + ";Initial Catalog = " + tDBName;
                tCon = tCon + Environment.NewLine + ";Connection Timeout=30;Connection Lifetime=0;Min Pool Size=0;Max Pool Size=30;Pooling=true";
                tCon = tCon + Environment.NewLine + ";Persist Security Info=True;User ID = " + tLogIn;
                tCon = tCon + Environment.NewLine + ";Password =" + tPws;
                return tCon;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }

        }
        #endregion

    }
}