using System;
using System.Collections.Generic;
using System.Data;
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
    public class clHomeController : Controller
    {
        private readonly ILog oC_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private mlDBStk oC_DBStk = null;
        public ActionResult wHome()
        {
            ViewBag.Title = "MlInterfaceAPI";
            Session["sMainName"] = null;
            SETxReadDByTdr();
            SETxReadDBQuota();

            return View(oC_DBStk);
        }

        private void SETxReadDByTdr()
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
                // cCNSP.ADDxLog("clHomeController:SETxReadDByTdr = " + ex.Message);
                oC_Log.Error(ex.Message);
            }
        }

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
        #endregion "Quota"
    }
}

