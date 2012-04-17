using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Text;
using CommandLibrary;

namespace Metrix.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            SetMenuSelection("Home");

            return View();
        }


        public ActionResult Support(FormCollection form)
        {
            SetMenuSelection("Support");

            if (form.Count > 0)
            {
                string company = form["txtCompany"];
                string name = form["txtName"];
                string reason = form["txtReason"];
                string email = form["txtEmail"];

                if (name == string.Empty || reason == string.Empty || email == string.Empty)
                {
                    ViewData["Status"] = "Ooops...Name, Email, and the Issue you are having is required";
                    return View();
                }

                Regex r = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (!r.IsMatch(email))
                {
                    ViewData["Status"] = "Somethin' wrong with your email...";
                    return View();
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<strong>SUPPORT REQUEST!!</strong>");
                sb.Append("<p>");
                sb.Append("<b>Company Name</b>: ");
                sb.Append(form["txtCompany"]);
                sb.Append("</p><p>");
                sb.Append("<b>Person's Name</b>: ");
                sb.Append(form["txtName"]);
                sb.Append("</p><p>");
                sb.Append("<b>Email:</b><a href='mailto:");
                sb.Append(email);
                sb.Append("'>");
                sb.Append(email);
                sb.Append("</a></p><p>");
                sb.Append("<b>Issue they are having:</b><br/> ");
                sb.Append(reason);
                sb.Append("</p>");

                UtilCommand.SendEmail("metrix@webosroundup.com", "Metrix Support Request", sb.ToString());
                
                return RedirectToAction("Thanks");
            }

            return View();
        }

        public ActionResult Contact(FormCollection form)
        {
            SetMenuSelection("Contact");

            if (form.Count > 0)
            {
                string name = form["txtName"];
                string message = form["txtMessage"];
                string email = form["txtEmail"];

                if (name == string.Empty || message == string.Empty || email == string.Empty)
                {
                    ViewData["Status"] = "Ooops...Name, Email, and the Message you want to send is required";
                    return View();
                }

                Regex r = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (!r.IsMatch(email))
                {
                    ViewData["Status"] = "Somethin' wrong with your email...";
                    return View();
                }

                StringBuilder sb = new StringBuilder();
                sb.Append("<strong>CONTACT REQUEST!!</strong>");
                sb.Append("<p>");
                sb.Append("<b>Person's Name</b>: ");
                sb.Append(form["txtName"]);
                sb.Append("</p><p>");
                sb.Append("<b>Email:</b><a href='mailto:");
                sb.Append(email);
                sb.Append("'>");
                sb.Append(email);
                sb.Append("</a></p><p>");
                sb.Append("<b>Reason for getting in touch:</b><br/> ");
                sb.Append(message);
                sb.Append("</p>");

                //create the mail message
                UtilCommand.SendEmail("metrix@webosroundup.com", "Metrix Contact", sb.ToString());
                
                return RedirectToAction("Thanks");
            }

            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        public ActionResult Features()
        {
            SetMenuSelection("Features");

            return View();
        }

        public ActionResult Signup(FormCollection form)
        {
            SetMenuSelection("Signup");

            DBContext db = new DBContext();

            #region Add User

            if (form.Count > 0)
            {
                string name = form["txtName"];
                string first = form["txtFirst"];
                string last = form["txtLast"];
                string company = form["txtCompany"];
                string email = form["txtEmail"];
                string password = form["txtPassword"];
                string confirm = form["txtConfirm"];

                ViewData["Name"] = name;
                ViewData["First"] = first;
                ViewData["Last"] = last;
                ViewData["Company"] = company;
                ViewData["Email"] = email;
                
                #region Validation

                if (name == string.Empty || first == string.Empty || last == string.Empty || email == string.Empty || 
                    password == string.Empty || confirm == string.Empty || company == string.Empty)
                {
                    ViewData["Status"] = "All fields are required...";
                    return View();
                }

                if (!Regex.IsMatch(email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    ViewData["Status"] = "Come on...email needs to be real otherwise we can't send you your authorization key.";
                    return View();
                }

                if (password != confirm)
                {
                    ViewData["Status"] = "Passwords don't match...";
                    return View();
                }

                //dup name check
                var check = db.Users.SingleOrDefault(i => i.Login == name);
                if (check != null)
                {
                    ViewData["Status"] = "Sorry, that user name is taken.";
                    return View();
                }

                #endregion

                User u = new User();
                u.ID = Guid.NewGuid();
                u.Login = name;
                u.FirstName = first;
                u.LastName = last;
                u.CompanyName = company;
                u.Email = email;
                u.Password = UtilCommand.GetMD5(password);
                u.AuthKey = Guid.NewGuid();
                db.Users.InsertOnSubmit(u);

                UserUserType uut = new UserUserType();
                uut.ID = Guid.NewGuid();
                uut.UserID = u.ID;
                uut.UserTypeID = new Guid("4cbef1c1-c2fa-4ce6-9bc1-6cada43ad0d4"); //developer
                db.UserUserTypes.InsertOnSubmit(uut);                

                db.SubmitChanges();

                #region Send Email

                StringBuilder sb = new StringBuilder();
                sb.Append("<h2>Welcome to Metrix!</h2>");
                sb.Append("<p>Thank you for signing up to Metrix. The last step to completing your profile is copying the code below and pasting it into your profile page at http://metrix.webosroundup.com.</p>");
                sb.Append("<p>If you have logged out of metrix, simply log back in and paste in the code.</p>");
                sb.Append("<p><strong>Authorization Key:</strong> ");
                sb.Append(u.AuthKey.ToString());
                sb.Append("</p><p>Thank you again for your interest in Metrix. If you have any questions, please feel free to email us at metrix@webosroundup.com and we will get back to you as soon as possible.");
                sb.Append("<br/><p>-- The webOSroundup and Syntactix teams</p>");

                UtilCommand.SendEmail(email, "Metrix Authorization Key", sb.ToString());

                #endregion

                Session["UserID"] = u.ID;
                Session["UserTypes"] = u.UserUserTypes.ToList();

                return RedirectToAction("Authentication", "Profile");
            }

            #endregion

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Thanks()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            return View();
        }

        public ActionResult Login(FormCollection form)
        {
            if (form.Count > 0)
            {

                DBContext db = new DBContext();
                User u = (from t in db.Users
                          where t.Login == form[0] && t.Password == UtilCommand.GetMD5(form[1])
                          select t).FirstOrDefault();

                if (u == null)
                    return View();

                Session["UserID"] = u.ID;
                Session["UserTypes"] = u.UserUserTypes.ToList();
                Session["License"] = u.SignedLicense;

                if (u.AuthKey == null)
                {
                    if (u.UserUserTypes.Where(i => i.UserType.Name == Constants.UserTypes.Admin).Count() > 0)
                        return RedirectToAction("Apps", "Admin");
                    else
                        return RedirectToAction("List", "Apps");
                }
                else
                    return RedirectToAction("Authentication", "Profile");
            }

            return View();
        }

        public ActionResult TeaseEmail(FormCollection form)
        {
            string company = form["txtCompany"];
            string name = form["txtName"];
            string apps = form["txtApps"];
            string email = form["txtEmail"];

            if (company == string.Empty || name == string.Empty || apps == string.Empty || email == string.Empty)
            {
                return RedirectToAction("Index", new { status = "required" });
            }

            Regex r = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            if (!r.IsMatch(email))
            {
                return RedirectToAction("Index", new { status = "bademail" });
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Dev Signup!!");
            sb.Append("<br/><br/>");
            sb.Append("<b>Company Name</b>: ");
            sb.Append(form["txtCompany"]);
            sb.Append("<br/>");
            sb.Append("<b>Person's Name</b>: ");
            sb.Append(form["txtName"]);
            sb.Append("<br/>");
            sb.Append("<b>Apps:</b> ");
            sb.Append(form["txtApps"]);
            sb.Append("<br/>");
            sb.Append("<b>Email:</b>");
            sb.Append(form["txtEmail"]);

            //create the mail message
            UtilCommand.SendEmail("devsignup@webosroundup.com", "Dev Signup!", sb.ToString());

            return RedirectToAction("Thanks");
        }
    }
}
