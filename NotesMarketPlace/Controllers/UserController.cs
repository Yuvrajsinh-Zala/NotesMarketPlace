using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    [Authorize(Roles = "Member")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult BuyerRequests()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult MyDownloadNotes()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult MyRejectedNotes()
        {
            return View();
        }

        public ActionResult MySoldNotes()
        {
            return View();
        }

        public ActionResult SearchNotes()
        {
            return View();
        }

    }
}