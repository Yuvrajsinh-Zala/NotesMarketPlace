using NoteMarketPlace.Models;
using NotesMarketPlace.Database;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NotesMarketPlace.Controllers
{
    
    [Authorize(Roles = "Member")]
    public class UserController : Controller
    {
        private NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();


        // GET: User
        public ActionResult BuyerRequests()
        {
            return View();
        }

        public ActionResult ContactUS()
        {
            ViewBag.Message = "Contact Us page.";
            ModelState.Clear();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactUs model)
        {


            if (ModelState.IsValid)
            {

                using (MailMessage mm = new MailMessage("email@example.com", model.Email))
                {
                    mm.Subject = model.Subject;
                    string body = "Hello " + model.Name + ",";
                    // body += "<br /><br />Please click the following link to activate your account";
                    //body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                    //body += "<br /><br />Thanks";

                    mm.Body = model.Message;
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("email@example.com", "**********");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }

            }
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
            ViewBag.Message = "Search Notes page.";

            List<SellerNote> SellerNotes = dbobj.SellerNotes.ToList();
            List<Country> Countries = dbobj.Countries.ToList();

            var multiple = from c in SellerNotes
                           join t1 in Countries on c.Country equals t1.ID
                           where c.Status == 9
                           select new DataRetrival { SellerNote = c, Country = t1 };

            ViewBag.Count = (from c in SellerNotes
                             join t1 in Countries on c.Country equals t1.ID
                             where c.Status == 9
                             select c).Count();

            return View(multiple);
        }

        public ActionResult AddNotes()
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var NoteCategory = entities.NoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteType = entities.NoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = entities.Countries.ToList();
            SelectList CountryList = new SelectList(CountryName, "ID", "Name");
            ViewBag.Country = CountryList;



            return View();



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNotes(SellerNote model)
        {


            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var NoteCategory = entities.NoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteType = entities.NoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = entities.Countries.ToList();
            SelectList CountryList = new SelectList(CountryName, "ID", "Name");
            ViewBag.Country = CountryList;


            string name = User.Identity.Name;
            int u = (from user in dbobj.Users where user.EmailID == name select user.ID).Single();
            string book_title = model.Title;
            string notename_fullpath = null;
            string picname_fullpath = null;
            string previewname_fullpath = null;

            SellerNote obj = new SellerNote();


            if (model.uploadNote != null)
            {
                string notepath = Server.MapPath("~/App_Data/Archive");
                string notename = Path.GetFileName(model.uploadNote.FileName);
                notename_fullpath = Path.Combine(notepath, notename);
                model.uploadNote.SaveAs(notename_fullpath);

            }
            else
            {
                ViewBag.Message = "Please Upload You File";
                return View();
            }

            if (model.displayPic != null)
            {
                string picpath = Server.MapPath("~/App_Data/Archive");
                string picname = Path.GetFileName(model.displayPic.FileName);
                picname_fullpath = Path.Combine(picpath, picname);
                model.displayPic.SaveAs(picname_fullpath);
                obj.DisplayPicture = picname_fullpath;


            }
            if (model.notePreview != null)
            {
                string previewpath = Server.MapPath("~/App_Data/Archive");
                string previewname = Path.GetFileName(model.notePreview.FileName);
                previewname_fullpath = Path.Combine(previewpath, previewname);
                model.notePreview.SaveAs(previewname_fullpath);
                obj.NotesPreview = previewname_fullpath;
            }

            try
            {
                obj.SellerID = u;
                obj.Title = model.Title;
                obj.Category = model.Category;
                obj.NoteType = model.NoteType;
                obj.NumberofPages = model.NumberofPages;
                obj.Description = model.Description;
                obj.UniversityName = model.UniversityName;
                obj.Country = model.Country;
                obj.Course = model.Course;
                obj.CourseCode = model.CourseCode;
                obj.Professor = model.Professor;
                obj.Status = 7;

                obj.IsActive = true;
                obj.SellingPrice = model.SellingPrice;
                obj.IsPaid = model.IsPaid;

                dbobj.SellerNotes.Add(obj);
                dbobj.SaveChanges();


            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                throw dbEx;
            }


            int book_id = (from record in dbobj.SellerNotes where record.SellerID == u && record.Title == book_title select record.ID).First();

            try
            {
                SellerNotesAttachement sellerattachment = new SellerNotesAttachement();
                sellerattachment.NoteID = book_id;
                sellerattachment.FilePath = notename_fullpath;
                sellerattachment.FileName = book_title;
                sellerattachment.CreatedBy = u;
                sellerattachment.CreatedDate = DateTime.Now;
                sellerattachment.IsActive = true;
                dbobj.SellerNotesAttachements.Add(sellerattachment);
                dbobj.SaveChanges();

            }
            catch
            {

            }
            return View();
        }

    }
}