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
    [Authorize]
    public class UczestnicyController : Controller
    {
        private KWR dbo = new KWR();


        public ActionResult addUser(int? id, int? idW)
        {
            if(id == null || idW == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            if(dbo.Uzytkownik.Find(id) == null || dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Błąd dodawania użytkownika");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(idW).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Błąd dostępu");
                    return RedirectToAction("GetOpis", "Wydarzenie", new { id = idW });
                }
                var count = dbo.Uczestnicy.Where(u => u.id_uzytkownik == id).Where(u => u.id_wydarzenie == idW).Count();
                if (count == 0)
                {
                    dbo.Wydarzenie_Uczestnik(idW, id);
                }
                return RedirectToAction("addUser", "Uzytkownik", new { id = idW });
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu do relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
        }

        public ActionResult DeleteUser(int? id, int? idW)
        {
            if (id == null || idW == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Uzytkownik.Find(id) == null || dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Błąd usuwania użytkownika");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(idW).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Błąd dostępu");
                    return RedirectToAction("GetOpis","Wydarzenie", new { id = idW });
                }
                var count = dbo.Uczestnicy.Where(u => u.id_uzytkownik == id).Where(u => u.id_wydarzenie == idW).Count();
                if (count != 0)
                {
                    dbo.Wydarzenie_Uczestnik_Usun(idW, id);
                }
                return RedirectToAction("GetOpis", "Wydarzenie", new { id = idW });
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu do relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
        }

        public ActionResult GetUczestnicy(int? id_wydarzenie)
        {
            if(id_wydarzenie == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            if(dbo.Wydarzenie.Find(id_wydarzenie) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id_wydarzenie).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id_wydarzenie && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                IEnumerable<Uczestnicy> uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Include(u => u.Wydarzenie).Where(u => u.id_wydarzenie == id_wydarzenie);
                if (dbo.Wydarzenie.Find(id_wydarzenie).DataArchiwizacji != null)
                {
                    ViewBag.id_wydarzenie = id_wydarzenie;
                    ViewBag.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
                    ViewBag.id_organizator_wydarzenie = dbo.Wydarzenie.Find(id_wydarzenie).id_organizator;
                    
                    return PartialView("_PartialUczestnicyArchiwum", uczestnicy.ToList());
                }
                ViewBag.id_wydarzenie = id_wydarzenie;
                ViewBag.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
                ViewBag.id_organizator_wydarzenie = dbo.Wydarzenie.Find(id_wydarzenie).id_organizator;
               
                return PartialView("_PartialUczestnicy", uczestnicy.ToList());
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wydarzenia");
            }
        }

        public ActionResult AddGroupEvent(int? id, int? idW)
        {
            if (id == null || idW == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Grupa.Find(id) == null || dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(idW).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Błąd dostępu");
                    return RedirectToAction("GetOpis", "Wydarzenie", new { id = idW });
                }
                IList<UzytkownicyWGrupie> uzytkownicyWGrupie = dbo.UzytkownicyWGrupie.Where(uwg => uwg.id_grupa == id).ToList();
                foreach (var item in uzytkownicyWGrupie)
                {
                    var count = dbo.Uczestnicy.Where(u => u.id_uzytkownik == item.id_uzytkownik).Where(u => u.id_wydarzenie == idW).Count();
                    if (count == 0)
                    {
                        dbo.Wydarzenie_Uczestnik(idW, item.id_uzytkownik);
                    }

                }

                return RedirectToAction("AddGroupEvent", "Grupa", new { id = idW });
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wydarzenia");
            }
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