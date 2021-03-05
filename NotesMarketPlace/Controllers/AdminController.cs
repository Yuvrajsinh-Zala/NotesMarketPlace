using NotesMarketPlace.Database;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    
    public class AdminController : Controller
    {
        private NotesMarketPlaceEntities _Context;
        // GET: Admin

        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult DownloadedNotes()
        {
            return View();
        }

        public AdminController()
        {
            _Context = new NotesMarketPlaceEntities();
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
            List<NoteCategory> NoteCategoriesList = _Context.NoteCategories.ToList(); //new List<NoteCategory>();
            List<User> User = _Context.Users.ToList(); //new List<NoteCategory>();

            var multiple = from c in NoteCategoriesList
                           join t1 in User on c.CreatedBy equals t1.ID
                           select new DataRetrival { NoteCategory = c, User = t1 };

            return View(multiple);
        }
                
        public ActionResult AddCategory()
        {
            return View();
        }
                
        [HttpPost]
        public ActionResult AddCategory(NoteCategory model)
        {

            bool isvalid = _Context.NoteCategories.Any(m => m.Name == model.Name);

            if (!isvalid)
            {
                NoteCategory obj = new NoteCategory();
                obj.Name = model.Name;
                obj.Description = model.Description;
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = 2;
                obj.IsActive = true;

                if (ModelState.IsValid)
                {
                    _Context.NoteCategories.Add(obj);
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        _Context.SaveChanges();

                        ModelState.Clear();

                        return View();

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

            }

            else
                ViewBag.Message = "Note Category already exists";

            return View();
        }


        public ActionResult ManageCountry()
        {
            List<Country> CountriesList = _Context.Countries.ToList(); //new List<Country>();
            List<User> User = _Context.Users.ToList(); //new List<Country>();

            var multiple2 = from c in CountriesList
                            join t1 in User on c.CreatedBy equals t1.ID
                            select new DataRetrival { Country = c, User = t1 };

            return View(multiple2);
        }

        public ActionResult AddCountry()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCountry(Country model)
        {

            bool isvalid = _Context.Countries.Any(m => m.Name == model.Name);

            if (!isvalid)
            {
                Country obj2 = new Country();
                obj2.Name = model.Name;
                obj2.CountryCode = model.CountryCode;
                obj2.CreatedDate = DateTime.Now;
                obj2.CreatedBy = 2;
                obj2.IsActive = true;

                if (ModelState.IsValid)
                {
                    _Context.Countries.Add(obj2);
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        _Context.SaveChanges();

                        ModelState.Clear();

                        return View();

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

            }

            else
                ViewBag.Message = "Country already exists";

            return View();
        }
        public ActionResult ManageSystemConfiguration()
        {
            return View();
        }

        public ActionResult ManageType()
        {
            List<NoteType> NoteTypesList = _Context.NoteTypes.ToList(); //new List<NoteType>();
            List<User> User = _Context.Users.ToList(); //new List<NoteType>();

            var multiple2 = from c in NoteTypesList
                           join t1 in User on c.CreatedBy equals t1.ID
                           select new DataRetrival { NoteType = c, User = t1 };

            return View(multiple2);
        }

        public ActionResult AddType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddType(NoteType model)
        {

            bool isvalid = _Context.NoteTypes.Any(m => m.Name == model.Name);

            if (!isvalid)
            {
                NoteType obj3 = new NoteType();
                obj3.Name = model.Name;
                obj3.Description = model.Description;
                obj3.CreatedDate = DateTime.Now;
                obj3.CreatedBy = 2;
                obj3.IsActive = true;

                if (ModelState.IsValid)
                {
                    _Context.NoteTypes.Add(obj3);
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        _Context.SaveChanges();

                        ModelState.Clear();

                        return View();

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

            }

            else
                ViewBag.Message = "Note Type already exists";

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