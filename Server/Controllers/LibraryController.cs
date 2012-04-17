using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommandLibrary;

namespace Metrix.Controllers
{
    public class LibraryController : BaseController
    {
        //
        // GET: /Library/

        public ActionResult License(FormCollection form)
        {
            SetMenuSelection("Download");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            if (form.Count > 0)
            {
                DBContext db = new DBContext();
                User u = (from t in db.Users
                          where t.ID.ToString() == Session["UserID"].ToString()
                          select t).FirstOrDefault();

                u.SignedLicense = true;
                db.SubmitChanges();
                Session["License"] = true.ToString();
                return RedirectToAction("Download", "Library");
            }

            return View();
        }

        public ActionResult Index()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            if (Session["License"].ToString() != true.ToString())
                return RedirectToAction("License", "Library");

            return RedirectToAction("Download", "Library");
        }

        public ActionResult Download(FormCollection form)
        {
            SetMenuSelection("Download");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            if (form.Count > 0)
                return RedirectToAction("DownloadFile");

            return View();
        }

        public FilePathResult DownloadFile()
        {
            if (Session["UserID"] == null)
                return null;

            string path = AppDomain.CurrentDomain.BaseDirectory + @"Download\MetrixLibrary.zip";
            return File(path, "multipart/form-data", "MetrixLibrary.zip");
        }

    }
}
