using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult DownloadedNotes()
        {
            return View();
        }
        public ActionResult ManageAdministrator()
        {
            return View();
        }

        public ActionResult AddAdministrator()
        {
            return View();
        }
        public ActionResult ManageCategory()
        {
            return View();
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        public ActionResult ManageCountry()
        {
            return View();
        }

        public ActionResult AddCountry()
        {
            return View();
        }

        public ActionResult ManageSystemConfiguration()
        {
            return View();
        }

        public ActionResult ManageType()
        {
            return View();
        }

        public ActionResult AddType()
        {
            return View();
        }

        public ActionResult ManageMembers()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            return View();
        }
        public ActionResult NotesUnderPreview()
        {
            return View();
        }

        public ActionResult PublishedNotes()
        {
            return View();
        }

        public ActionResult RejectedNotes()
        {
            return View();
        }

        public ActionResult SpamReports()
        {
            return View();
        }

    }
}