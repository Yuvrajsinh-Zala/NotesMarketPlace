using NotesMarketPlace.Database;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NotesMarketPlace.Controllers
{
    public class LoginController : Controller
    {

        private NotesMarketPlaceEntities _Context;
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public LoginController()
            {
                _Context = new NotesMarketPlaceEntities();
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(login model)
        {
            bool isvalid = _Context.Users.Any(m => m.EmailID == model.EmailID && m.Password == model.Password);

            if (isvalid)
            {
                var result = _Context.Users.Where(m => m.EmailID == model.EmailID).FirstOrDefault();
                if (result.RoleID == 101 || result.RoleID == 102)
                {
                    FormsAuthentication.SetAuthCookie(model.EmailID, false);
                    return RedirectToAction("Dashboard", "Admin");
                }

                else if (result.RoleID == 103)
                {
                    FormsAuthentication.SetAuthCookie(model.EmailID, false);
                    return RedirectToAction("Dashboard", "User");
                }
                else
                    ViewBag.NotValidUser = "Something went wrong";
            }
            else
            {
                ViewBag.NotValidUser = "Incorrect Email or Password";

                /*if (model.Password == result.Password)
                {
                  /*  if (User.Identity.IsAuthenticated)
                    {
                        string name = User.Identity.Name;
                    }
                    if()
                    FormsAuthentication.SetAuthCookie(result.EmailID,true);
                    return RedirectToAction("", "user");
                }
                else
                {
                    ViewBag.NotValidPassword = "Passowrd is Incorrect";
                }*/
            }
            return View("Login");

        }
    
        public ActionResult ForgotPassword()
        {
            return View();
        }

        NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User model)
        {
            var context = new NotesMarketPlaceEntities();
            string email = model.EmailID;

            if (IsValidEmail(email))
            {
                if (IsValidPassword(model.Password, model.RePassword))
                {
                    var result = context.Users.Where(m => m.EmailID == email).FirstOrDefault();
                    if (result == null)
                    {

                        User obj = new User();
               
                        obj.FirstName = model.FirstName;
                        obj.LastName = model.LastName;
                        obj.EmailID = model.EmailID;
                        obj.Password = model.Password;
                        obj.RePassword = "abbc";
                        obj.IsEmailVerified = false;
                        obj.IsActive = true;
                        obj.ModifiedBy = null;
                        obj.ModifiedDate = null;

                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedBy = null;
                        obj.RoleID = 103;

                        if (ModelState.IsValid)
                        {
                            dbobj.Users.Add(obj);
                            try
                            {
                                // Your code...
                                // Could also be before try if you know the exception occurs in SaveChanges

                                dbobj.SaveChanges();
                                ModelState.Clear();

                                FormsAuthentication.SetAuthCookie(model.EmailID, true);
                                return RedirectToAction("MyProfile", "User");

                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName, ve.ErrorMessage);
                                    }
                                }
                            }

                        }

                        return RedirectToAction("Home", "Home");

                    }

                    else
                    {
                        ViewBag.UserExist = "This Email is already exists";

                    }



                }

                else
                {
                    ViewBag.NotValidPassword = "Password and Re-enter password must be same";
                }
            }
            else
            {
                ViewBag.NotValidEmail = "Email is not valid";
            }
            return View("Signup");

        }
        public static bool IsValidPassword(string pswd, string repswd)
        {
            if (pswd == repswd && pswd != "")
            {
                return true;
            }
            return false;
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}