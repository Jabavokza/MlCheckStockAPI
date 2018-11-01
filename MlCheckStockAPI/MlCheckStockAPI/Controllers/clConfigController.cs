using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using log4net;
using MlCheckStockAPI.Models;
using MlCheckStockAPI.ST_Class;
using Newtonsoft.Json;

namespace MlCheckStockAPI.Controllers
{
    public class clConfigController : Controller
    {
        private readonly ILog oC_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private mlDBStk oC_DBStk = null;
        // GET: cvcConfig
        [Route("SetConfig")]
        public ActionResult wConfig()
        {
            try
            {
                if (Session["sMainName"] == null)
                {
                    return RedirectToRoute("Default", new { controller = "clHome", action = "wHome" });
                }
                Session["sMainName"] = "admin";
                SETxReadDBQuota();
                SETxReadDBByTdr();

            }
            catch (Exception ex)
            {
                cCNSP.ADDxLog("clConfigController:wConfig = " + ex.Message);
            }
            return View(oC_DBStk);

        }
        public JsonResult CHKbSession()
        {
            bool bSession = false;
            if (Session["sMainName"] == null)
            {

                Session["sMainName"] = "admin";
                bSession = true;
            }
            else
            {
                Session["sMainName"] = null;
                bSession = false;
            }
            return Json(bSession, JsonRequestBehavior.AllowGet);
        }
        #region "ByTender"
        public void SETxReadDBByTdr()
        {
            try
            {
                string tPath1 = "";
                string tPath2 = "";
                tPath1 = "~/Config/DBByTdr.xml";
                tPath2 = HttpContext.Server.MapPath(tPath1);
                XmlDocument doc = new XmlDocument();
                FileStream read = new FileStream(tPath2, FileMode.Open, FileAccess.Read, FileShare.Read);

                doc.Load(read);
                XmlNodeList elemList = doc.GetElementsByTagName("DB");

                foreach (XmlNode node in elemList)
                {
                    XmlElement element = (XmlElement)node;

                    ViewData["db_ServerNameM"] = element.GetElementsByTagName("ServerName")[0].InnerText;
                    ViewData["db_DBNameM"] = element.GetElementsByTagName("DataBaseName")[0].InnerText;
                    ViewData["db_UserM"] = element.GetElementsByTagName("User")[0].InnerText;
                    ViewData["db_PasswordM"] = element.GetElementsByTagName("Password")[0].InnerText;
                }
                read.Flush();
                read.Close();
                // read.Dispose();
            }
            catch (Exception ex)
            {
                //cCNSP.ADDxLog("cvcConfigController:SETxReadDBMain = " + ex.Message);
                oC_Log.Error(ex.Message);
            }
        }
        public ActionResult UPDxDBByTdr(string otbDBSrvM, string otbDBNameM, string otbDBUsrM, string otbDBPwdM, string otbModeBtn)
        {
            XmlWriter oWr = null;
            string tMsgSuccess = "";
            try
            {
                if (otbModeBtn == "SAVE")
                {
                    try
                    {
                        string tPath1 = "";
                        string tPath2 = "";
                        tPath1 = "~/Config/DBByTdr.xml";
                        tPath2 = HttpContext.Server.MapPath(tPath1);

                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.IndentChars = ("\t");
                        settings.OmitXmlDeclaration = true;


                        oWr = XmlWriter.Create(tPath2, settings);

                        oWr.WriteStartDocument();
                        oWr.WriteStartElement("DataBase");

                        oWr.WriteStartElement("DB");
                        oWr.WriteElementString("ServerName", otbDBSrvM);
                        oWr.WriteElementString("DataBaseName", otbDBNameM);
                        oWr.WriteElementString("User", otbDBUsrM);
                        oWr.WriteElementString("Password", otbDBPwdM);
                        oWr.WriteEndElement();

                        oWr.WriteEndElement();
                        oWr.WriteEndDocument();

                        oWr.Flush();
                    }
                    catch (Exception ex)
                    {
                        // cCNSP.ADDxLog("clConfigController:UPDxDBMain = " + ex.Message);
                        oC_Log.Error(ex.Message);
                    }
                    finally
                    {
                        if (oWr != null) { oWr.Close(); }
                    }
                }
                else if (otbModeBtn == "CHECKDB")
                {
                    string tCon = "";
                    SqlDataAdapter oDbAdt = null;
                    SqlCommand oCmdSql = new SqlCommand();

                    tCon = "Data Source = " + otbDBSrvM;
                    tCon = tCon + Environment.NewLine + ";Initial Catalog = " + otbDBNameM;
                    tCon = tCon + Environment.NewLine + ";Persist Security Info=True;User ID = " + otbDBUsrM;
                    tCon = tCon + Environment.NewLine + ";Password =" + otbDBPwdM;
                    tCon = tCon + Environment.NewLine + ";Connection Lifetime = 0; Min Pool Size = 0; Max Pool Size = 1500; Pooling = true";

                    SqlConnection oDbCon = new SqlConnection(tCon);
                    try
                    {
                        oDbCon.Open();
                        oCmdSql.Connection = oDbCon;
                        // MessageBox.Show("Data base name " + otbDBHQ.Text + " HQ successful.");
                        tMsgSuccess = "Data base name " + otbDBNameM + "  Database ready.";
                    }
                    catch (Exception ex)
                    {
                        tMsgSuccess = "Data base name " + otbDBNameM + "  Database = " + ex.Message;
                    }
                    finally
                    {
                        if (oDbAdt != null) { oDbAdt.Dispose(); }
                    }

                    ViewData["con_DBMsgByTdr"] = tMsgSuccess;
                }

                //   return RedirectToAction("wConfig");


            }
            catch (Exception ex)
            {
                oC_Log.Error(ex.Message);
            }

            if (otbModeBtn == "SAVE")
            {
                return RedirectToAction("wConfig");
            }
            else
            {
                SETxReadDBQuota();
                SETxReadDBByTdr();
                return View("wConfig", oC_DBStk);
            }
        }
        #endregion "ByTender"
        #region "Quota"
        private void SETxReadDBQuota()
        {
            try
            {
                string tPath1 = "";
                string tPath2 = "";
                tPath1 = "~/Config/DBStock.json";
                tPath2 = HttpContext.Server.MapPath(tPath1);
                oC_DBStk = JsonConvert.DeserializeObject<mlDBStk>(System.IO.File.ReadAllText(tPath2));

            }
            catch (Exception ex)
            {
                oC_Log.Error(ex.Message);
            }
        }
        public ActionResult UPDxDBQuota(mlDBStk poDBStk)
        {
            try
            {
                WRIxJSON(poDBStk);
                SETxReadDBQuota();
                SETxReadDBByTdr();
                return View("wConfig", oC_DBStk);
            }
            catch (Exception ex)
            {
                oC_Log.Error(ex.Message);
            }
            return View("wConfig", oC_DBStk);
        }

        public ActionResult CHKxDBQuota(string tBrance, string tServerName, string tDabaseName, string tUser, string tPasword)
        {
            string tCon = "";
            string tMsgSuccess = "";
            SqlDataAdapter oDbAdt = null;
            SqlCommand oCmdSql = new SqlCommand();


            tCon = "Data Source = " + tServerName;
            tCon = tCon + Environment.NewLine + ";Initial Catalog = " + tDabaseName;
            tCon = tCon + Environment.NewLine + ";Persist Security Info=True;User ID = " + tUser;
            tCon = tCon + Environment.NewLine + ";Password =" + tPasword;
            tCon = tCon + Environment.NewLine + ";Connection Lifetime = 0; Min Pool Size = 0; Max Pool Size = 1500; Pooling = true";

            SqlConnection oDbCon = new SqlConnection(tCon);
            try
            {
                oDbCon.Open();
                oCmdSql.Connection = oDbCon;
                // MessageBox.Show("Data base name " + otbDBHQ.Text + " HQ successful.");
                tMsgSuccess = "Brance " + tBrance + " Data base name " + tDabaseName + " Database ready.";
            }
            catch (Exception ex)
            {
                tMsgSuccess = "Brance " + tBrance + " Data base name " + tDabaseName + " Database = " + ex.Message;
            }
            finally
            {
                if (oDbAdt != null) { oDbAdt.Dispose(); }
            }

            ViewData["con_DBMsgQuota"] = tMsgSuccess;

            SETxReadDBQuota();
            SETxReadDBByTdr();
            return View("wConfig", oC_DBStk);
        }
        private void WRIxJSON(mlDBStk poDBStk)
        {
            try
            {
                string tPath1 = "";
                string tPath2 = "";
                tPath1 = "~/Config/DBStock.json";
                tPath2 = HttpContext.Server.MapPath(tPath1);
                string tJson = JsonConvert.SerializeObject(poDBStk, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(tPath2, tJson);

            }
            catch (Exception ex)
            {
                oC_Log.Error(ex.Message);
            }
        }
        #endregion "Quota"
    }

}

