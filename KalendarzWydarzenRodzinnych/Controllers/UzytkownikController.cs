using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using KalendarzWydarzenRodzinnych.Models;
using KalendarzWydarzenRodzinnych.Extensions;
using static KalendarzWydarzenRodzinnych.Controllers.ManageController;
using System.IO;
using System.Web.Helpers;

namespace KalendarzWydarzenRodzinnych.Controllers
{
    [Authorize]
    public class UzytkownikController : Controller
    {
        private KWR dbo = new KWR();
        
        [HttpGet]
        public ActionResult addUser(int? id)  
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu ");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Wydarzenie.Find(id) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranego wydarzenia");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(id).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Błąd dostępu");
                    return RedirectToAction("ListArchiwum","Wydarzenie");
                }
                ViewBag.id_wydarzenie = id;
                ViewBag.id_organizator = id_user;
                ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id);
                SqlParameter idWydarzenie = new SqlParameter("@Par_IdWydarzenie", id);
                IEnumerable<Uzytkownik> query = dbo.Uzytkownik.SqlQuery("Wyswietl_Uzytkownikow @Par_IdWydarzenie", idWydarzenie);
                return View(query.ToList());
            }
            else{
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
        }

        [HttpGet]
        public ActionResult AddUserGrupa(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu ");
                return RedirectToAction("List", "Grupa");
            }
            if (dbo.Grupa.Find(id) == null)
            {
                TempData["message"] = string.Format("Brak wybranej grupy");
                return RedirectToAction("List", "Grupa");
            }

            ViewBag.id_grupa = id;
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Grupa.Find(id).id_uzytkownik == id_user)
            {
                SqlParameter idGrupa = new SqlParameter("@Par_IdGrupa", id);
                SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id_user);

                IEnumerable<Uzytkownik> query = dbo.Uzytkownik.SqlQuery("Wyswietl_Uzytkownikow_Grupa @Par_IdGrupa, @Par_IdUzytkownik", idGrupa, idUzytkownik);
                return View(query.ToList());
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
        }

        [HttpGet]
        public ActionResult UserProfile(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Zmieniono hasło."
                : message == ManageMessageId.SetPasswordSuccess ? "Ustawiono hasło."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Ustawiono dostawcę uwierzytelniania dwuetapowego."
                : message == ManageMessageId.Error ? "Wystąpił błąd."
                : message == ManageMessageId.AddPhoneSuccess ? "Dodano numer telefonu."
                : message == ManageMessageId.RemovePhoneSuccess ? "Usunięto numer telefonu."
                : "";
            var userId = Convert.ToInt32(User.Identity.GetUzytkownikId());
            Uzytkownik user = dbo.Uzytkownik.Find(userId);

            return View(user);

        }

        [HttpPost]
        public ActionResult UserProfile(Uzytkownik user)
        {

            var userId = Convert.ToInt32(User.Identity.GetUzytkownikId()); //poprawić warunki bo bez zdjęcia nie wchodzi
            //Uzytkownik user = dbo.Uzytkownik.Find(userId);
            foreach (HttpPostedFileBase image in user.files)
            {
                try
                {
                    if (image != null)
                    {
                        var InputFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                        user.Zdjcie = InputFileName;
                    
                        dbo.Entry(user).State = EntityState.Modified;
                        dbo.SaveChanges();
                        WebImage img = new WebImage(image.InputStream);
                        if (img.Width > 300)
                            img.Resize(250, 250);
                        img.Save("~/Image/profile/" + InputFileName);


                    }
                }
                catch (Exception e)
                {
                    TempData["message"] = string.Format("Dodanie zdjęć nie powiodło się. Spróbuj ponownie lub skontaktuj się z administratorem aplikacji Błąd:" + e.ToString());
                }




            }
            return RedirectToAction("UserProfile");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}