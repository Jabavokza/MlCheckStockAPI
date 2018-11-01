using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace MlCheckStockAPI.X_Class
{
    public static class cExtensionMessageError
    {
        public static string C_GETtErrorModaleSta(this ModelStateDictionary poModelSta)
        {
            try
            {
                var oModelerror = poModelSta.Values.Select(value => value.Errors).FirstOrDefault();
                if (oModelerror != null)
                {
                    return oModelerror[0].ErrorMessage;
                }
                return null;
            }
            catch (Exception oEx)
            {
                throw oEx;
            }
            
        }
    }
}