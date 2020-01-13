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
    public class ZaproszenieController : Controller
    {
        private KWR dbo = new KWR();
        public ActionResult List()
        {
            
            var id = Convert.ToInt32(User.Identity.GetUzytkownikId());
            var zaproszenia = dbo.Zaproszenie.Include(z => z.Uzytkownik1).Include(z=>z.Wydarzenie).Where(z => z.Do == id);
            return View(zaproszenia.ToList());
        }

        public ActionResult Accept(int? id)
        {
            if(id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            Zaproszenie zaproszenie = dbo.Zaproszenie.Where(z=>z.id_wydarzenie == id).Where(z=>z.Do == id_user).SingleOrDefault();
            if(zaproszenie == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List");
            }
            
            if (zaproszenie.Do == id_user)
            {
                dbo.Zaproszenie_Potwierdz(id, id_user);
                return RedirectToAction("List");
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu do relacji");
                return RedirectToAction("List");
            }
        }

        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            Zaproszenie zaproszenie = dbo.Zaproszenie.Where(z => z.id_wydarzenie == id).Where(z => z.Do == id_user).SingleOrDefault();
            if (zaproszenie == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List");
            }
           
            if (zaproszenie.Do == id_user)
            {
                dbo.Zaproszenie_Anuluj(id, id_user);
                return RedirectToAction("List");
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu do relacji");
                return RedirectToAction("List");
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