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

        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult MySoldNotes()
        {
            ViewBag.Count = 1;

            List<User> tblUsersList = dbobj.Users.ToList();
            List<Download> tblDownloadList = dbobj.Downloads.ToList();
            List<UserProfile> tblUserProfilesList = dbobj.UserProfiles.ToList();

            int user_id = (from user in dbobj.Users where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = (from d in tblDownloadList
                            join t1 in tblUsersList on d.Downloader equals t1.ID
                            join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                            where d.Seller == user_id && d.IsSellerHasAllowedDownload == true


                            select new DataRetrival { Download = d, User = t1, UserProfile = t2 }).ToList();

            return View(multiple);

        }


        public ActionResult MyDownloadNotes(int id)
        {

            var user_id = dbobj.Users.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
            var download = dbobj.Downloads.Where(m => m.NoteID.Equals(id) && m.Seller == user_id.ID).FirstOrDefault();

            var attachments = dbobj.SellerNotesAttachements.Where(m => m.NoteID == id).FirstOrDefault();
            var seller = dbobj.SellerNotes.Where(m => m.ID == id && m.SellerID == user_id.ID && m.Status == 10).FirstOrDefault();
            if (seller != null || download != null)
            {
                string path = attachments.FilePath;
                string filename = attachments.FileName + ".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            else
                return HttpNotFound();
        }

        public ActionResult MyRejectedNotes()
        {

            List<User> tblUsersList = dbobj.Users.ToList();
            List<SellerNote> tblSellerNotes = dbobj.SellerNotes.ToList();
            List<NoteCategory> tblNoteCategories = dbobj.NoteCategories.ToList();

            int user_id = (from user in dbobj.Users where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            ViewBag.Count = 1;
            var multiple = (from d in tblSellerNotes
                            join t1 in tblUsersList on d.SellerID equals t1.ID
                            join t2 in tblNoteCategories on d.Category equals t2.ID
                            where d.SellerID == user_id && d.Status == 10


                            select new DataRetrival { SellerNote = d, User = t1, NoteCategory = t2 }).ToList();

            return View(multiple);
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

        public ActionResult NoteDeatils(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
            var user_id = dbobj.Users.Where(m => m.EmailID == User.Identity.Name && m.RoleID != 103).FirstOrDefault();
            if (user_id != null)
                goto eligible;
            var tblSeller = dbobj.SellerNotes.Where(m => m.ID == id && m.Status == 9).FirstOrDefault();
            if (tblSeller == null)
                return HttpNotFound();
            eligible:
            List<SellerNote> SellerNotes = dbobj.SellerNotes.ToList();
            List<Country> Countries = dbobj.Countries.ToList();
            List<NoteCategory> NoteCategories = dbobj.NoteCategories.ToList();

            var multiple = from c in SellerNotes
                           join t1 in Countries on c.Country equals t1.ID
                           join t2 in NoteCategories on c.Category equals t2.ID
                           where c.ID == id
                           select new DataRetrival { SellerNote = c, Country = t1, NoteCategory = t2 };

            return View(multiple);
        }


        public ActionResult FreeDownLoad(int? id)
        {


            var user_email = dbobj.Users.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblSeller = dbobj.SellerNotes.Where(m => m.ID == id).FirstOrDefault();
            var user_id = user_email.ID;

            /* if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             else*/
            if (!tblSeller.IsPaid)
            {
                if (tblSeller == null || tblSeller.Status != 9)
                    return HttpNotFound();

                else if (tblSeller != null && tblSeller.Status == 9)
                {

                    string path = (from sa in dbobj.SellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                    string category = (from c in dbobj.NoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                    Download obj = new Download();
                    obj.NoteID = tblSeller.ID;
                    obj.Seller = tblSeller.SellerID;
                    obj.Downloader = user_id;
                    obj.IsSellerHasAllowedDownload = true;
                    obj.AttachmentPath = path;
                    obj.IsAttachmentDownloaded = true;
                    obj.IsPaid = false;
                    obj.PurchasedPrice = tblSeller.SellingPrice;
                    obj.NoteTitle = tblSeller.Title;
                    obj.NoteCategory = category;

                    obj.CreatedDate = DateTime.Now;
                    dbobj.Downloads.Add(obj);
                    dbobj.SaveChanges();

                    string filename = (from sa in dbobj.SellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
                    filename += ".pdf";
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

                }
            }
            return HttpNotFound();

        }


        public ActionResult AskforDownload(int id)
        {
            var user_email = dbobj.Users.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblSeller = dbobj.SellerNotes.Where(m => m.ID == id).FirstOrDefault();
            var user_id = user_email.ID;
            var check_download = dbobj.Downloads.Where(m => m.NoteID == id && m.Downloader == user_id).FirstOrDefault();

            if (check_download != null)
                return Json(new { success = true, alertMsg = "you downloaded this book already." }, JsonRequestBehavior.AllowGet);



            //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
            if (tblSeller == null || tblSeller.Status != 9)
                return HttpNotFound();

            else if (tblSeller != null && tblSeller.Status == 9)
            {


                var seller = dbobj.Users.Where(m => m.ID == tblSeller.SellerID).FirstOrDefault();
                string path = (from sa in dbobj.SellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                string category = (from c in dbobj.NoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                string seller_name = seller.FirstName;
                seller_name += " " + seller.LastName;
                string buyer_name = user_email.FirstName;
                buyer_name += " " + user_email.LastName;
                string buyer_email = seller.EmailID;

                Download obj = new Download();
                obj.NoteID = tblSeller.ID;
                obj.Seller = tblSeller.SellerID;
                obj.Downloader = user_id;
                obj.IsSellerHasAllowedDownload = false;
                obj.AttachmentPath = path;
                obj.IsAttachmentDownloaded = false;
                obj.IsPaid = true;
                obj.PurchasedPrice = tblSeller.SellingPrice;
                obj.NoteTitle = tblSeller.Title;
                obj.NoteCategory = category;
                obj.CreatedDate = DateTime.Now;

                dbobj.Downloads.Add(obj);
                dbobj.SaveChanges();
                ViewBag.Msg = "Request Added";

                return Json(new { success = true, responseText = seller_name }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { success = false, responseText = "Not Completed." }, JsonRequestBehavior.AllowGet);
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