using CacheManager.Core;
using log4net;
using MlCheckStockAPI.Models.ByTender;
using MlCheckStockAPI.ST_Class;
using MlCheckStockAPI.X_Class.QtaByTdr;
using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Web.Http;

namespace MlCheckStockAPI.Controllers
{
    /// <summary>
    /// 1.0.0
    /// </summary>
    [RoutePrefix("api")]
    public class clByTdrController : ApiController
    {
        private readonly ILog oC_Log = LogManager.GetLogger("Err");
        private mlRESQtaByTdr oC_RESQtaByTdrcs;
        [Route("promobytender/Reserve")]
        [HttpPost]
        public IHttpActionResult SEToPromoByTdrReserve([FromBody]mlREQQtaByTdrReserve pmlREQQtaByTdrReserve)
        {
            try
            {
                oC_Log.Debug("-------------------------------START------------------------------------");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

               // C_CHKxMemcach();
                var oReServe = new cReServe();
                oC_RESQtaByTdrcs = new mlRESQtaByTdr();
                oC_RESQtaByTdrcs = oReServe.C_SEToReServe(pmlREQQtaByTdrReserve);
                oC_Log.Debug("-------------------------------End------------------------------------");
                return Json(oC_RESQtaByTdrcs);
            }
            catch (Exception oEx)
            {
                return BadRequest(oEx.Message);
            }
        }
        [Route("promobytender/Used")]
        [HttpPost]
        public IHttpActionResult SEToPromoByTdrUsed([FromBody]mlREQQtaByTdrUsed pmlREQQtaByTdrUsed)
        {
            try
            {
                oC_Log.Debug("-------------------------------START------------------------------------");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

              //  C_CHKxMemcach();
                var oUsed = new cUsed();
                oC_RESQtaByTdrcs = new mlRESQtaByTdr();
                oC_RESQtaByTdrcs = oUsed.C_SEToUsed(pmlREQQtaByTdrUsed);
                oC_Log.Debug("-------------------------------End------------------------------------");
                return Json(oC_RESQtaByTdrcs);
            }
            catch (Exception oEx)
            {
                return BadRequest(oEx.Message);
            }
        }
        [Route("promobytender/Cancel")]
        [HttpPost]
        public IHttpActionResult SEToPromoByTdrCancel([FromBody]mlREQQtaByTdrCancel pmlREQQtaByTdrCancel)
        {
            try
            {
                oC_Log.Debug("-------------------------------START------------------------------------");
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                var oCancel = new cCancel();
              //  C_CHKxMemcach();
                oC_RESQtaByTdrcs = new mlRESQtaByTdr();
                oC_RESQtaByTdrcs = oCancel.C_SEToCancel(pmlREQQtaByTdrCancel);
                oC_Log.Debug("-------------------------------End------------------------------------");
                return Json(oC_RESQtaByTdrcs);
            }
            catch (Exception oEx)
            {
                return BadRequest(oEx.Message);
            }
        }
        //private void C_CHKxMemcach()
        //{
        //    try
        //    {
        //        //Memcach init
        //        var tCon = cConDbByTender.C_GETtDBByTdr();
        //        var oMC_Usr = new BaseCacheManager<string>(cCNVB.oCMConfig);
        //        oMC_Usr.AddOrUpdate("key", tCon, _ => tCon);
        //        cCNVB.tConStr = oMC_Usr.Get("key");
        //    }
        //    catch (WebException oEx)
        //    {
        //        throw oEx;
        //    }
        //}
    }
}