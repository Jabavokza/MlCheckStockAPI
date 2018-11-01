using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using MlCheckStockAPI.ST_Class;

namespace MlCheckStockAPI.ST_Class
{
    public static class cCNSP
    {
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section,
          string key, string def, StringBuilder retVal,
          int size, string filePath);

        public static DataTable SP_SQLoExecute(string ptQeury, string ptConStr)
        {
            SqlDataAdapter oDbAdt;
            DataTable oDbTblDt = new DataTable();
            //  SP_SETxConClient();
            try
            {
                SqlConnection oConstr = new SqlConnection(ptConStr);
                {
                    oConstr.Open();
                    oDbAdt = new SqlDataAdapter(ptQeury, oConstr);
                    oDbAdt.Fill(oDbTblDt);
                    oConstr.Dispose();
                    oDbAdt.Dispose();
                    return oDbTblDt;
                }
            }
            catch (Exception ex)
            {
                //SP_SETxMessage("cCNSP:SP_SQLvExecute\n" + ex.Message);
                cCNSP.ADDxLog("API:Excute DataTable  = " + ex.Message);
                throw ex;
            }
            finally
            {
                oDbAdt = null;
            }

        }

        public static void SP_SETxConClient()
        {
            string tCon = null;
            tCon = "Data Source = ADA160\\SQL2012";
            tCon = tCon + Environment.NewLine + ";Initial Catalog = ProjectControl";
            tCon = tCon + Environment.NewLine + ";Persist Security Info=True;User ID = sa";
            tCon = tCon + Environment.NewLine + ";Password =adasoft";
            // cCNVB.tVB_ConStr = tCon;
        }

        public static string DTEtByFormat(string ptDate, string ptFormat)
        {
            string tRet = "";
            DateTime odt = Convert.ToDateTime(ptDate);
            switch (ptFormat.ToUpper())
            {
                case "DD/MM/YYYY":
                    tRet = int.Parse(odt.Day.ToString()).ToString("00") + "/" + odt.Month + "/" + odt.Year;
                    break;
                case "YYYY/MM/DD":
                    tRet = odt.Year + "/" + int.Parse(odt.Month.ToString()).ToString("00") + "/" + int.Parse(odt.Day.ToString()).ToString("00");
                    break;
                case "YYMMDD":
                    tRet = "" + odt.Year + int.Parse(odt.Month.ToString()).ToString("00") + int.Parse(odt.Day.ToString()).ToString("00");
                    break;
                case "YYYYMM":
                    tRet = "" + odt.Year + odt.Month;
                    break;
                case "YYYY-MM-DD":
                    //   { 02 / 11 / 2016 0:00:00}
                    tRet = odt.Year + "-" + int.Parse(odt.Month.ToString()).ToString("00") + "-" + int.Parse(odt.Day.ToString()).ToString("00");
                    break;
                case "HH:MM:SS":
                    tRet = odt.Hour + ":" + odt.Minute + ":" + odt.Second;
                    break;
                case "YYYY/MM/DD HH:MM:SS":
                    tRet = odt.Year + "/" + odt.Month + "/" + int.Parse(odt.Day.ToString()).ToString("00") + " " + odt.Hour + ":" + odt.Minute + ":" + odt.Second;
                    break;
                case "DD/MM/YYYY HH:MM:SS":
                    tRet = int.Parse(odt.Day.ToString()).ToString("00") + "/" + int.Parse(odt.Month.ToString()).ToString("00") + "/" + odt.Year + " " + odt.Hour + ":" + odt.Minute + ":" + odt.Second;
                    break;
                case "YYYYMMDDHHMMSS":
                    //ใช้ตั้งชื่อไฟล์
                    //tRet = odt.Year.ToString() + odt.Month.ToString() + int.Parse(odt.Day.ToString()).ToString("00") + odt.Hour.ToString() + odt.Minute.ToString() + odt.Second.ToString();
                    tRet = odt.Year.ToString() + int.Parse(odt.Month.ToString()).ToString("00") + int.Parse(odt.Day.ToString()).ToString("00") + int.Parse(odt.Hour.ToString()).ToString("00") + int.Parse(odt.Minute.ToString()).ToString("00") + int.Parse(odt.Second.ToString()).ToString("00");
                    break;
                default:
                    tRet = ptDate;
                    break;
            }
            return tRet;
        }

        public static string GETtLangINI(int nLang, string tPath, string ptSection, string ptKey)
        {
            string[] aLang;
            string tLang;
            StringBuilder tSB = new StringBuilder(255);
            GetPrivateProfileString(ptSection, ptKey, "", tSB, 255, tPath);

            aLang = tSB.ToString().Split(';');
            tLang = (nLang == 0 ? aLang[0] : aLang[1]);

            return tLang;
        }

        public static string USEtEnCrypt(string ptType, string ptTxt)
        {
            string tResult = "";
            switch (ptType)
            {
                case "SHA512":
                    tResult = SETtSHA512(ptTxt);
                    break;
                case "MD5":
                    tResult = SETxMD5Hash(ptTxt);
                    break;
            }
            return tResult;
        }

        public static string SETtSHA512(string ptTxt)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(ptTxt);
            byte[] hash = sha512.ComputeHash(bytes);
            // return GetStringFromHash(hash);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }

        public static string SETxMD5Hash(string ptTxt)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(ptTxt));
            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static void ADDxLog(string ptLogMsg)
        {
            // Call SP_ADDxLog(tVB_LogPath & "\Error" & Format(Date.Now, "yyyyMMdd"), Format(Date.Now, "HH:mm:ss") & "XXX") 'เรียกใช้
            // SP_WRITExLog(ptPath & ".txt", ptLog)
            SETxWRITExLog(cCNSP.DTEtByFormat(DateTime.Now.ToString(), "YYYY/MM/DD HH:MM:SS") + ":MsgLog:" + ptLogMsg);
        }

        public static void SETxWRITExLog(string ptLog)
        {
            // string tPath = Directory.GetCurrentDirectory();
            string tPath1 = "";
            // string tPath2 = "";
            string tName = cCNSP.DTEtByFormat(DateTime.Now.ToString(), "YYYY-MM-DD");
            tName = tName + "_SAPMInterface";
            // tPath1 = "~/Log/" + tName + ".txt";
            // tPath2 = HttpContext.Current.Server.MapPath(tPath1);
            tPath1 = "Log\\" + tName + ".txt";

            cCNVB.tPath2 = cCNVB.tPath2 + tPath1;
            //  tPath2 = tPath + "\\Log\\" + tName + ".txt";
            try
            {
                if (!(File.Exists(cCNVB.tPath2)))
                {
                    using (StreamWriter oSw = File.CreateText(cCNVB.tPath2))
                    {
                        oSw.Close();
                    }
                }
                using (StreamWriter oSw = File.AppendText(cCNVB.tPath2))
                {
                    var _with1 = oSw;
                    _with1.WriteLine(ptLog);
                    _with1.Flush();
                    _with1.Close();
                }
            }
            catch (Exception ex)
            {

                // SP_ADDxLog("mCNSP:SP_WRITExLog:" + ex.Message);
            }
        }

        public static string GETtCutStr(string ptText, int pnLengFix)
        {
            //(value.Length <= 32 ? value.Substring(0, value.Length) : value.Substring(0, 32));
            string tStr = "";
            if (ptText.Length <= pnLengFix)
            {
                tStr = ptText.Substring(0, ptText.Length);
            }
            else
            {
                tStr = ptText.Substring(0, pnLengFix);
            }
            return tStr;
        }

        #region DbByTdr
        /// <summary>
        /// cConnectDbByTdr
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public static DataTable SP_GEToDbTbl(string ptSql)
        {
            SqlConnection oDbCon = new SqlConnection();
            DataTable oDbTbl = new DataTable();
            SqlDataAdapter oDbAdt;
            try
            {
                oDbCon.ConnectionString = SP_GETtDBByTdr();
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
        public static int SP_SETnDbTbl(string ptSql)
        {
            SqlConnection oDbCon = new SqlConnection();
            SqlCommand oDbCmd = new SqlCommand();
            DataTable oDbTbl = new DataTable();
            try
            {
                oDbCon.ConnectionString = SP_GETtDBByTdr();
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
        public static string SP_GETtDBByTdr()
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
                XmlNodeList elemList = oDoc.GetElementsByTagName("DB");
                foreach (XmlNode node in elemList)
                {
                    XmlElement InformationElement = (XmlElement)node;
                    tServer = InformationElement.GetElementsByTagName("ServerName")[0].InnerText;
                    tLogIn = InformationElement.GetElementsByTagName("User")[0].InnerText;
                    tPws = InformationElement.GetElementsByTagName("Password")[0].InnerText;
                    tDBName = InformationElement.GetElementsByTagName("DataBaseName")[0].InnerText;
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