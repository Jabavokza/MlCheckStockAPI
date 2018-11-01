using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CacheManager.Core;
using log4net;

namespace MlCheckStockAPI.ST_Class
{
    public class cCNVB
    {
        public static string tLang = "";

        public static string tErr = "";

        public static string tConStr = "";

        public static string tConDBName = "";

        public static string tDBNameM = "";

        public static string tTdate = "FDDateUpd,FTTimeUpd,FTWhoUpd,FDDateIns,FTTimeIns,FTWhoIns";

        public static string tFdate = "CONVERT(VARCHAR(10),GETDATE(),121),CONVERT(VARCHAR(8),GETDATE(),108),'admin',CONVERT([VARCHAR](10),GETDATE(),(121)),CONVERT([VARCHAR](8),GETDATE(),(108)),'admin'";

        public static string tTdateIns = "FDDateIns, FTTimeIns, FTWhoIns";

        public static string tFdateIns = "CONVERT([VARCHAR](10),GETDATE(),(121)),CONVERT([VARCHAR](8),GETDATE(),(108)),'admin'";

        public static string tPath2 = "";

        public static ICacheManagerConfiguration oCMConfig = null;


    }
}