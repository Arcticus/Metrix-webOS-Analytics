using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Text;
using CommandLibrary;

namespace Metrix.Controllers
{
    public class ProfileController : BaseController
    {
        //
        // GET: /Profile/

        public ActionResult Update(FormCollection form)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            if (((List<UserUserType>)Session["UserTypes"]).Where(i => i.UserType.Name == Constants.UserTypes.Admin).Count() > 0)
                SetMenuSelection("AdminProfile");
            else
                SetMenuSelection("Profile");

            DBContext db = new DBContext();
            User u = (from t in db.Users
                      where t.ID.ToString() == Session["UserID"].ToString()
                      select t).FirstOrDefault();

            ViewData["User"] = u;

            if (form.Count > 0)
            {
                string first = form["txtName"];
                string last = form["txtLast"];
                string company = form["txtCompany"];
                string email = form["txtEmail"];
                string password = form["txtPassword"];
                string confirm = form["txtConfirm"];

                if (company == string.Empty || first == string.Empty || last == string.Empty || email == string.Empty)
                {
                    ViewData["Status"] = "All of the fields are required";
                    return View();
                }

                if (password != string.Empty && password != confirm)
                {
                    ViewData["Status"] = "Passwords don't match";
                    return View();
                }

                Regex r = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (!r.IsMatch(email))
                {
                    ViewData["Status"] = "We need a valid email address so we can send fun messages!";
                    return View();
                }

                u.FirstName = first;
                u.LastName = last;
                u.CompanyName = company;
                u.Email = email;

                if (password != string.Empty)
                    u.Password = UtilCommand.GetMD5(password);

                db.SubmitChanges();

                ViewData["Status"] = "Good to go chief";
            }

            return View();
        }

        public ActionResult Authentication(FormCollection form)
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Home");

            DBContext db = new DBContext();
            User u = db.Users.SingleOrDefault(i => i.ID.ToString() == Session["UserID"].ToString());
            ViewData["Email"] = u.Email;

            if (form["txtKey"] != null)
            {
                if (form["txtKey"].Trim() != u.AuthKey.ToString())
                {
                    ViewData["Status"] = "Sorry chief, that key didn't work...please try again, and if this continues to be a problem email us at metrix@webosroundup.com";
                    return View();
                }

                u.AuthKey = null;
                db.SubmitChanges();

                #region Send email to admins

                StringBuilder sb = new StringBuilder();
                sb.Append("<strong>NEW DEV!!</strong><br/>A new dev has just completed the signup process to use Metrix.");
                sb.Append("<p>");
                sb.Append("<b>Company Name</b>: ");
                sb.Append(u.CompanyName);
                sb.Append("</p><p>");
                sb.Append("<b>Person's Name</b>: ");
                sb.Append(u.FirstName);
                sb.Append(" ");
                sb.Append(u.LastName);
                sb.Append("</p><p>");
                sb.Append("<b>Email:</b><a href='mailto:");
                sb.Append(u.Email);
                sb.Append("'>");
                sb.Append(u.Email);
                sb.Append("</a></p>");
               
                //create the mail message
                UtilCommand.SendEmail("metrix@webosroundup.com", "Metrix New Dev Signup", sb.ToString());
                
                #endregion

                return RedirectToAction("List", "Apps");
            }
            else if (form["txtEmail"] != null)
            {
                Regex r = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                if (!r.IsMatch(form["txtEmail"]))
                {
                    ViewData["Status"] = "We need a valid email address so we can send fun messages!";
                    return View();
                }

                u.Email = form["txtEmail"];
                db.SubmitChanges();

                SendEmail(u);
                ViewData["Status"] = "Email changed and we resent your authentication key!";
            }
            else if (form["hdnResend"] != null)
            {
                SendEmail(u);
                ViewData["Status"] = "Authentication key resent!";
            }

            return View();
        }

        private void SendEmail(User u)
        {
            #region Send Email

            StringBuilder sb = new StringBuilder();
            sb.Append("<h2>Welcome to Metrix!/h2>");
            sb.Append("<p>Thank you for signing up to Metrix. The last step to completing your profile is copying the code below and pasting it into your profile page at http://metrix.webosroundup.com.</p>");
            sb.Append("<p>If you have logged out of metrix, simply log back in and paste in the code.</p>");
            sb.Append("<p><strong>Authorization Key:</strong> ");
            sb.Append(u.AuthKey.ToString());
            sb.Append("</p><p>Thank you again for your interest in Metrix. If you have any questions, please feel free to email us at metrix@webosroundup.com and we will get back to you as soon as possible.");
            sb.Append("<br/><p>-- The webOSroundup and Syntactix teams</p>");

            //create the mail message
            UtilCommand.SendEmail(u.Email, "Metrix Authorization Key", sb.ToString());
            
            #endregion
        }
    }
}
