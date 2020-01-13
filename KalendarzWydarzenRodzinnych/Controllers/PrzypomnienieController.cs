using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using KalendarzWydarzenRodzinnych.Models;
using KalendarzWydarzenRodzinnych.Extensions;
namespace KalendarzWydarzenRodzinnych.Controllers
{
    public class PrzypomnienieController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Przypomnienie
        public ActionResult List()
        {
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            
            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id_user);


            IEnumerable<Przypomnienie_Wyswietl_Result> notify = dbo.Database.SqlQuery<Przypomnienie_Wyswietl_Result>("Przypomnienie_Wyswietl @Par_IdUzytkownik", idUzytkownik);

            
           
            return View(notify.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Brak wybranego przypomnienia w bazie danych");
                return RedirectToAction("List");
            }
            Przypomnienie notification = dbo.Przypomnienie.Find(id);
            if (notification == null)
            {
                TempData["message"] = string.Format("Brak wybranego przypomnienia w bazie danych");
                return RedirectToAction("List");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (notification.id_organizator == null)
            {
                if(notification.id_uzytkownik == id_user)
                {
                    return View(notification);
                }
                else
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List");
                }
            }
            else
            {
                if(notification.id_organizator == id_user)
                {
                    return View(notification);
                }
                else
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List");
                }
            }

           

        }

        [HttpPost]
        public ActionResult Edit(Przypomnienie notification)
        {
            
                if (ModelState.IsValid)
                {
                    if(notification.identyfier == null)
                    {
                    if (notification.Data < DateTime.Now)
                    {
                        TempData["message"] = string.Format("Wybrana data nie może być wcześniejsza niż obecna data");
                        return View(notification);
                    }
                    
                    dbo.Entry(notification).State = EntityState.Modified;
                        dbo.SaveChanges();
                        return RedirectToAction("List");
                    }
                    else
                    {
                    if (notification.Data < DateTime.Now)
                    {
                        TempData["message"] = string.Format("Wybrana data nie może być wcześniejsza niż obecna data");
                        return View(notification);
                    }
                    dbo.Przypomnienie_Zmiana(notification.Data, notification.Tresc, notification.identyfier);
                        dbo.SaveChanges();
                        return RedirectToAction("List");
                }
                    
                }
                else
                {

                    return View(notification);
                }
            
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Wprowadzono niepoprawne dane");
                return RedirectToAction("List");
            }
            Przypomnienie notification = dbo.Przypomnienie.Find(id);
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (notification == null)
            {
                TempData["message"] = string.Format("Brak wybranego przypomnienia w bazie danych");
                return RedirectToAction("List");
            }
           
            if(notification.identyfier == null)
            {
                if(notification.id_uzytkownik == id_user)
                {
                    dbo.Przypomnienie.Remove(notification);
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                }
                else 
                { 
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List");
                }
            }
            else
            {
                if(notification.id_organizator == id_user)
                {
                    dbo.Przypomnienie_Usun(notification.identyfier);
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List");
                }
            }
        }

        [HttpGet]
        public ActionResult AddNotification(int? idW, string r)
        {
            if(idW == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List");
            }
            if (dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Brak wydarzenia do kórego próbowano dodać Przypomnienie");
                return RedirectToAction("List");
            }
            ViewBag.id_wydarzenie = idW;
            if (r == "u")
            {
                return View("AddNotification");
            }
            else
            {
                if (dbo.Wydarzenie.Find(idW).id_organizator == Convert.ToInt32(User.Identity.GetUzytkownikId()))
                {
                    return View("AddNotificationUsers");

                }
                else
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List");
                }
            }
            
        }

        [HttpPost]
        public ActionResult AddNotification(Przypomnienie notification)
        {
            
            if(notification.Data < DateTime.Now)
            {
                
                    TempData["message"] = string.Format("Wybrana data nie może być wcześniejsza niż obecna data");
                    return View("AddNotification", notification);
                
            }
            notification.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            notification.rodzaj = "Użytkownika";
            
           
                dbo.Przypomnienie.Add(notification);
                dbo.SaveChanges();
                return RedirectToAction("List");
           
            
        }

        [HttpPost]
        public ActionResult AddNotificationUsers(Przypomnienie notification)
        {
            if (notification.Data < DateTime.Now)
            {

                TempData["message"] = string.Format("Wybrana data nie może być wcześniejsza niż obecna data");
                return View("AddNotificationUsers", notification);

            }
            notification.rodzaj = "Wydarzenia";
            notification.id_organizator = dbo.Wydarzenie.Find(notification.id_wydarzenie).id_organizator;
            string identyfier = Guid.NewGuid().ToString();
            notification.identyfier = identyfier;
            Przypomnienie newNotification = new Przypomnienie();
            newNotification = notification;

            newNotification.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());            
            dbo.Przypomnienie.Add(newNotification);
            dbo.SaveChanges();
            List<Uczestnicy> users = dbo.Uczestnicy.Where(u => u.id_wydarzenie == notification.id_wydarzenie && u.decyzja == true).ToList();
            foreach(var item in users)
            {
                newNotification = new Przypomnienie();
                newNotification = notification;
                newNotification.id_uzytkownik = item.id_uzytkownik;                
                dbo.Przypomnienie.Add(notification);
                dbo.SaveChanges();
            }
            


            
         
            return RedirectToAction("List");


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