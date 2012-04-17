using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MongoCommand;
using Norm;
using CommandLibrary;

namespace Metrix.Controllers
{
    public class BaseController : Controller
    {
        protected void SetMenuSelection(string page)
        {
            ViewData["MenuHome"] = "";
            ViewData["MenuFeatures"] = "";
            ViewData["MenuSignup"] = "";
            ViewData["MenuSupport"] = "";
            ViewData["MenuContact"] = "";
            ViewData["MenuApps"] = "";
            ViewData["MenuProfile"] = "";
            ViewData["MenuDownload"] = "";
            ViewData["MenuAdminApps"] = "";
            ViewData["MenuAdminProfile"] = "";
            ViewData["MenuAdminAdStorm"] = "";

            switch (page)
            {
                case "Home":
                    ViewData["MenuHome"] = "opacity: 1;";
                    break;
                case "Features":
                    ViewData["MenuFeatures"] = "opacity: 1;";
                    break;
                case "Signup":
                    ViewData["MenuSignup"] = "opacity: 1;";
                    break;
                case "Support":
                    ViewData["MenuSupport"] = "opacity: 1;";
                    break;
                case "Contact":
                    ViewData["MenuContact"] = "opacity: 1;";
                    break;
                case "Apps":
                    ViewData["MenuApps"] = "opacity: 1;";
                    break;
                case "Profile":
                    ViewData["MenuProfile"] = "opacity: 1;";
                    break;
                case "Download":
                    ViewData["MenuDownload"] = "opacity: 1;";
                    break;
                case "AdminApps":
                    ViewData["MenuAdminApps"] = "opacity: 1;";
                    break;
                case "AdminProfile":
                    ViewData["MenuAdminProfile"] = "opacity: 1;";
                    break;
                case "AdminAdStorm":
                    ViewData["MenuAdminAdStorm"] = "opacity: 1;";
                    break;
            }

        }

        protected bool CheckAppSecurity(string packageid)
        {
            if (Session["UserID"] == null || packageid == null)
                return false;

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var collection = mongo.GetCollection<MetrixApp>();
                var app = collection.AsQueryable().SingleOrDefault(i => i.PackageID == packageid);

                if (Session["UserID"] == null || ((List<UserUserType>)Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin).Count() > 0)
                    return true;

                if (app == null || app.UserID.ToString() != Session["UserID"].ToString())
                    return false;

                return true;
            }
        }

    }
}
