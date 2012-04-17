using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommandLibrary;
using MongoCommand;
using System.Configuration;

using Norm;
using Norm.Collections;

namespace Metrix.Controllers
{
    public class AppsController : BaseController
    {
        //
        // GET: /Apps/

        public ActionResult List(FormCollection form)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            SetMenuSelection("Apps");

            DBContext db = new DBContext();

            

            if (form.Count > 0)
            {
                string pid = form[0];
                //make sure it hasn't been claimed before
                var test = from t in db.AppUsers
                            where t.PackageID == pid
                            select t;

                if (test.Count() > 0)
                    ViewData["Status"] = "Hey! That app is already taken!!";
                else
                {
                    AppUser au = new AppUser();
                    au.ID = Guid.NewGuid();
                    au.PackageID = pid;
                    au.UserID = new Guid(Session["UserID"].ToString());
                    db.AppUsers.InsertOnSubmit(au);
                    db.SubmitChanges();
                }
            }

            var list = from t in db.AppUsers
                        where t.UserID.ToString() == Session["UserID"].ToString()
                        select t;

            List<string> apps = new List<string>();

            foreach (var item in list)
            {
                apps.Add(item.PackageID);
            }

            ViewData["Apps"] = apps;

            return View();
        }

        public ActionResult Analytics(string packageid)
        {
            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            DBContext db = new DBContext();
            var au = db.AppUsers.Single(i => i.PackageID == packageid);

            if (Session["UserID"].ToString() != au.UserID.ToString())
                return RedirectToAction("Index", "Apps");

            ViewData["App"] = packageid;
            return View();
        }

        public ActionResult AdStorm(string packageid, FormCollection form)
        {
            return null; 
            //SetMenuSelection("Apps");

            //if (!CheckAppSecurity(packageid))
            //    return RedirectToAction("Login", "Home");

            //using (IMongo mongo = MDB.Instance().GetMongo())
            //{
            //if (form.Count > 0)
            //{
            //    //They just agreed to the TOS
            //    if (form["hdnAgree"] != null)
            //    {
            //        //create the record in the DB, but make sure it is not approved
            //        MApp a = col.SingleOrDefault(i => i.PackageID == packageid);
            //        if (a == null)
            //            return RedirectToAction("Login", "Home");

            //        var list = mongo.GetCollection<AppApproval>();
            //        AppApproval search = list.FindOne(new { packageid = packageid });
            //        if (search == null)
            //        {
            //            AppApproval newAA = new AppApproval();
            //            newAA.PackageID = packageid;
            //            newAA.IsApproved = false;

            //            list.Save(newAA);

            //            //SEND EMAIL!
            //        }
            //    }
            //    }

            //    AppApproval aa = mongo.GetCollection<AppApproval>().AsQueryable().SingleOrDefault(i => i.PackageID == packageid);

            //    ViewData["Approval"] = aa;

            //    if (aa != null && aa.IsApproved)
            //    {
            //        List<AdContainer> containers = mongo.GetCollection<AdContainer>().AsQueryable().Where(i => i.AppPackageID == packageid).OrderBy(i => i.ContainerName).ToList();
            //        //we only want one record per name
            //        List<AdContainer> returnList = new List<AdContainer>();
            //        string prev = null;
            //        foreach (var item in containers)
            //        {
            //            if (item.ContainerName != prev)
            //            {
            //                returnList.Add(item);
            //                prev = item.ContainerName;
            //            }
            //        }

            //        ViewData["Containers"] = returnList;
            //        ViewData["AdNetworks"] = MDB.Instance().GetAdNetworks();
            //    }


            //    return View();
            //}
        }

        public void DeleteContainer(string container)
        {
            if (Session["UserID"] == null)
                return;

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var collection = mongo.GetCollection<AdContainer>();

                List<AdContainer> containers = collection.AsQueryable().Where(i => i.ContainerName == container && i.UserID == new Guid(Session["UserID"].ToString()).ToString()).ToList();

                foreach (var item in containers)
                {
                    collection.Delete(item);
                }
            }
        }

        public ActionResult EditContainer(string packageid, string container)
        {
            if (!CheckAppSecurity(packageid))
                return RedirectToAction("Login", "Home");

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                List<AdContainer> containers = mongo.GetCollection<AdContainer>().AsQueryable().Where(i => i.AppPackageID == packageid && i.ContainerName == container).ToList();

                AppApproval aa = mongo.GetCollection<AppApproval>().AsQueryable().SingleOrDefault(i => i.PackageID == packageid);

                ViewData["Approval"] = aa;
                ViewData["Containers"] = containers;
                ViewData["AdNetworks"] = MDB.Instance().GetAdNetworks();

                return View();
            }
        }

        public ActionResult AddContainer(string packageid)
        {
            if (!CheckAppSecurity(packageid))
                return RedirectToAction("Login", "Home");

            using (IMongo mongo = MDB.Instance().GetMongo())
            {

                AppApproval aa = mongo.GetCollection<AppApproval>().AsQueryable().SingleOrDefault(i => i.PackageID == packageid);

                ViewData["Approval"] = aa;
                ViewData["AdNetworks"] = MDB.Instance().GetAdNetworks();

                return View();
            }
        }

        public ActionResult SaveContainer(string packageid, FormCollection form)
        {
            if (!CheckAppSecurity(packageid))
                return RedirectToAction("Login", "Home");

            string oldName = form["hdnOldName"];

            List<AdNetwork> networks = MDB.Instance().GetAdNetworks();

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var containers = mongo.GetCollection<AdContainer>();

                List<AdContainer> conList = (from t in containers.AsQueryable()
                                             where t.AppPackageID == packageid && t.ContainerName == oldName
                                             select t).ToList();

                if (conList.Count() == 0) //new container
                {
                    foreach (var item in networks)
                    {
                        AdContainer con = new AdContainer();
                        con.AppPackageID = packageid;
                        con.UserID = new Guid(Session["UserID"].ToString()).ToString();

                        con.AdNetworkID = item.ID;
                        con.AdNetworkName = item.Name;

                        conList.Add(con);
                    }
                }

                foreach (var con in conList)
                {
                    con.ContainerName = form["txtName"];
                    con.ContainerSize = form["ddlSize"];

                    AdNetwork network = networks.Where(i => i.ID == con.AdNetworkID).First();

                    string val = form["txtPercent" + network.ID];

                    if (val == string.Empty)
                        con.ShowPercentage = 0;
                    else
                        con.ShowPercentage = int.Parse(val);

                    containers.Save(con);
                }

                return RedirectToAction("AdStorm/" + packageid, "Apps");
            }
        }

        public JsonResult CheckName(string old, string name)
        {
            if (Session["UserID"] == null)
                return null;

            if (old == name)
                return Json(false, JsonRequestBehavior.AllowGet);

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
                var collection = mongo.GetCollection<AdContainer>().AsQueryable();
                var check = from t in collection
                            where t.ContainerName == name && t.UserID == new Guid(Session["UserID"].ToString()).ToString()
                            select t;

                if (check.Count() > 0)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Status(FormCollection form)
        {
            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            if (form.Count > 0)
            {
                DBContext db = new DBContext();
                var test = from t in db.MetrixGeneralDeviceDatas
                           where t.PackageID == form[0]
                           select t;

                if (test == null || test.Count() == 0)
                    ViewData["Status"] = "Hmm...something doesn't look quite right. Check the help file in the Metrix download to make sure everything is setup correctly.";
                else
                    ViewData["Status"] = "All clear on our end. We are collecting data as we speak!";
            }

            return View();
        }

        public ActionResult Delete(string packageid, FormCollection form)
        {
            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            using (IMongo mongo = MDB.Instance().GetMongo())
            {
               ViewData["PID"] = packageid;

                if (form["hdnDelete"] != null)
                {
                    DBContext db = new DBContext();
                    var list = db.AppUsers.Where(i => i.PackageID == packageid);

                    db.AppUsers.DeleteAllOnSubmit(list);
                    db.SubmitChanges();
                    return RedirectToAction("List", "Apps");
                }

                return View();
            }
        }

        [ValidateInput(false)]
        public ActionResult MaintainBulletin(string packageid, FormCollection form)
        {
            //it's not really the package id...just being lazy for now
            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            DBContext db = new DBContext();
            
            ViewData["Title"] = "Update Bulletin";
            MetrixBulletinBoardAnnouncement mbba = db.MetrixBulletinBoardAnnouncements.SingleOrDefault(i => i.ID.ToString() == packageid);
            if (mbba == null)
                return RedirectToAction("Login", "Home");

            ViewData["Bulletin"] = mbba;
            ViewData["PackageID"] = mbba.PackageID;

            if (form.Count > 0)
            {
                mbba.Title = form["txtTitle"];
                mbba.Message = form["editorUpdate"];
                db.SubmitChanges();

                Response.Redirect("/Apps/Bulletin/" + mbba.PackageID);
            }

            return View();

        }

        [ValidateInput(false)]
        public ActionResult AddBulletin(string packageid, FormCollection form)
        {
            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            DBContext db = new DBContext();
            ViewData["PackageID"] = packageid;

            if (form.Count > 0)
            {
                //create
                if (form["txtCreateTitle"] == string.Empty)
                {
                    ViewData["CreateStatus"] = "Title is required";
                    return View();
                }

                MetrixBulletinBoardAnnouncement mbba = new MetrixBulletinBoardAnnouncement();
                mbba.Message = form["editorCreate"];
                mbba.PackageID = packageid;
                mbba.Title = form["txtCreateTitle"];
                db.MetrixBulletinBoardAnnouncements.InsertOnSubmit(mbba);
                db.SubmitChanges();

                Response.Redirect("/Apps/Bulletin/" + packageid);
            }

            return View();
        }

        public ActionResult DeleteBulletin(string packageid)
        {
            //it's not really the package id...just being lazy for now

            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            DBContext db = new DBContext();
            MetrixBulletinBoardAnnouncement a = db.MetrixBulletinBoardAnnouncements.SingleOrDefault(i => i.ID.ToString() == packageid);
            
            db.MetrixBulletinBoardAnnouncements.DeleteOnSubmit(a);
            db.SubmitChanges();

            return RedirectToAction("Bulletin", "Apps", new { packageid = a.PackageID});
        }

        public ActionResult Bulletin(string packageid, FormCollection form)
        {
            SetMenuSelection("Apps");

            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            DBContext db = new DBContext();
            var list = from t in db.MetrixBulletinBoards
                        where t.PackageID == packageid
                        select t;

            //see if the version already exists
            MetrixBulletinBoard version = db.MetrixBulletinBoards.SingleOrDefault(i => i.PackageID == packageid);
            if (version == null)
            {
                version = new MetrixBulletinBoard();
                version.PackageID = packageid;
                version.BulletinBoardVersion = 1;
                db.MetrixBulletinBoards.InsertOnSubmit(version);
                db.SubmitChanges();
            }

            ViewData["Version"] = version;
            ViewData["Bulletins"] = list.ToList();
            ViewData["PID"] = packageid;

            if (form.Count > 0)
            {
                #region Validate

                int versionNumber;
                try
                {
                    versionNumber = int.Parse(form["txtUpdateVersion"]);
                }
                catch
                {
                    ViewData["VersionStatus"] = "Version needs to be a number";
                    return View();
                }

                #endregion

                version.BulletinBoardVersion = versionNumber;
                db.SubmitChanges();
                ViewData["VersionStatus"] = "Got it!";
            }

            return View();
        }

    }
}
