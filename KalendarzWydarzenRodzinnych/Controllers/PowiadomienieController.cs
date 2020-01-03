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
    public class PowiadomienieController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Powiadomienie
        public ActionResult List()
        {
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            
            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id_user);


            IEnumerable<Powiadomienie_Wyswietl_Result> notify = dbo.Database.SqlQuery<Powiadomienie_Wyswietl_Result>("Powiadomienie_Wyswietl @Par_IdUzytkownik", idUzytkownik);

            
           
            return View(notify.ToList());
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Brak wybranego powiadomienia w bazie danych");
                return RedirectToAction("List");
            }
            Powiadomienie notification = dbo.Powiadomienie.Find(id);
            if (notification == null)
            {
                TempData["message"] = string.Format("Brak wybranego powiadomienia w bazie danych");
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
        public ActionResult Edit(Powiadomienie notification)
        {
            
                if (ModelState.IsValid)
                {
                    if(notification.identyfier == null)
                    {
                        dbo.Entry(notification).State = EntityState.Modified;
                        dbo.SaveChanges();
                        return RedirectToAction("List");
                    }
                    else
                    {
                        dbo.Powiadomienie_Zmiana(notification.DataPowiadomienia, notification.Tresc, notification.identyfier);
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
            Powiadomienie notification = dbo.Powiadomienie.Find(id);
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (notification == null)
            {
                TempData["message"] = string.Format("Brak wybranego powiadomienia w bazie danych");
                return RedirectToAction("List");
            }
           
            if(notification.identyfier == null)
            {
                if(notification.id_uzytkownik == id_user)
                {
                    dbo.Powiadomienie.Remove(notification);
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
                    dbo.Powiadomienie_Usun(notification.identyfier);
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
            if (dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Brak wydarzenia do kórego próbowano dodać powiadomienie");
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
        public ActionResult AddNotification(Powiadomienie notification)
        {
            
            notification.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            notification.rodzaj = "Użytkownika";
            
           
                dbo.Powiadomienie.Add(notification);
                dbo.SaveChanges();
                return RedirectToAction("List");
           
            
        }

        [HttpPost]
        public ActionResult AddNotificationUsers(Powiadomienie notification)
        {
            notification.rodzaj = "Wydarzenia";
            notification.id_organizator = dbo.Wydarzenie.Find(notification.id_wydarzenie).id_organizator;
            string identyfier = Guid.NewGuid().ToString();
            notification.identyfier = identyfier;
            Powiadomienie newNotification = new Powiadomienie();
            newNotification = notification;

            newNotification.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());            
            dbo.Powiadomienie.Add(newNotification);
            dbo.SaveChanges();
            List<Uczestnicy> users = dbo.Uczestnicy.Where(u => u.id_wydarzenie == notification.id_wydarzenie && u.decyzja == true).ToList();
            foreach(var item in users)
            {
                newNotification = new Powiadomienie();
                newNotification = notification;
                newNotification.id_uzytkownik = item.id_uzytkownik;                
                dbo.Powiadomienie.Add(notification);
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