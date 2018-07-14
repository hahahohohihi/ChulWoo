using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ChulWoo.Helper;

namespace ChulWoo.Controllers
{
    public class BaseController : Controller
    {
/*        
        protected override void ExecuteCore()
        {
            int culture = 0;
            if (Session == null || Session["CurrentCulture"] == null)
            {

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)Session["CurrentCulture"];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = culture;

            base.ExecuteCore();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
*/
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            int culture = 0;
            if (Session == null || Session["CurrentCulture"] == null)
            {

                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Culture"], out culture);
                Session["CurrentCulture"] = culture;
            }
            else
            {
                culture = (int)Session["CurrentCulture"];
            }
            // calling CultureHelper class properties for setting  
            CultureHelper.CurrentCulture = culture;

            return base.BeginExecuteCore(callback, state);
        }
/*
        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
*/
    }
}