using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommandLibrary;

using Norm;
using MongoCommand;

namespace Metrix.Controllers
{
    public class AdminController : BaseController
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (Session["UserID"] == null || ((List<UserUserType>) Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin ).Count() == 0)
                return RedirectToAction("Login", "Home");

            return View();
        }

        public ActionResult Apps(FormCollection form)
        {
            SetMenuSelection("AdminApps");

            if (Session["UserID"] == null || ((List<UserUserType>) Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin ).Count() == 0)
                return RedirectToAction("Login", "Home");

            using (IMongo mongo = MDB.Instance().GetMongo())
            {

                DBContext db = new DBContext();
                var metrix = mongo.GetCollection<MetrixApp>().AsQueryable();
                var apps = mongo.GetCollection<MApp>().AsQueryable();

                List<MApp> appList = new List<MApp>();
                foreach (var item in metrix)
                {
                    MApp a = apps.SingleOrDefault(i => i.PackageID == item.PackageID);
	                if (a != null)
	                {
	                    appList.Add(a);
	                }
	        	}
           
            ViewData["AppList"] = appList.OrderBy(i => i.Name).ToList();

                if (form.Count > 0)
                {
                    string package = form[0];
                    var a = metrix.SingleOrDefault(i => i.PackageID == package);

                    ViewData["App"] = a;
                }

                return View();
            }
        }

        public void ApproveApp(string id)
        {
            if (id == null)
                return;

            if (Session["UserID"] == null || ((List<UserUserType>)Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin).Count() == 0)
                return;

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var collection = mongo.GetCollection<AppApproval>();

                AppApproval aa = collection.AsQueryable().SingleOrDefault(i => i.PackageID == id);
                if (aa == null)
                    return;

                aa.IsApproved = true;
                collection.Save(aa);
            }
        }

        public ActionResult Reset()
        {
            if (Session["UserID"] == null || ((List<UserUserType>)Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin).Count() == 0)
                return RedirectToAction("Login", "Home");

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var collection = mongo.GetCollection<AppApproval>();
                foreach (var item in collection.AsQueryable())
                {
                    item.IsApproved = false;
                    collection.Save(item);
                }

                return RedirectToAction("Adstorm", "Admin");
            }
        }

        public ActionResult AdStorm()
        {
            SetMenuSelection("AdminAdStorm");

            if (Session["UserID"] == null || ((List<UserUserType>)Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin).Count() == 0)
                return RedirectToAction("Login", "Home");

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var apps = mongo.GetCollection<MApp>().AsQueryable();
                DBContext db = new DBContext();

                var collection = mongo.GetCollection<AppApproval>().AsQueryable();
                var query = from t in collection
                            where t.IsApproved == false
                            select t.PackageID;

                List<MApp> unapproved = new List<MApp>();
                if (query != null && query.Count() > 0)
                {
                    foreach (var packageID in query)
                    {
                        MApp a = apps.Where(i => i.PackageID == packageID).FirstOrDefault();
                        if (a != null)
                            unapproved.Add(a);
                    }
                }

                ViewData["Unapproved"] = unapproved;

                query = from t in collection
                        where t.IsApproved == true
                        select t.PackageID;

                List<MApp> approved = new List<MApp>();

                if (query != null && query.Count() > 0)
                {
                    foreach (var packageID in query)
                    {
                        MApp a = apps.Where(i => i.PackageID == packageID).FirstOrDefault();
                        if (a != null)
                            approved.Add(a);
                    }
                }

                ViewData["Approved"] = approved;

                return View();
            }
        }

    }
}
