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
    public class PowiadomienieController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Powiadomienie
        public ActionResult List()
        {
            var id = Convert.ToInt32(User.Identity.GetUzytkownikId());
            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id);
            IEnumerable<Powiadomienie_Wyswietl_Result> query = dbo.Database.SqlQuery<Powiadomienie_Wyswietl_Result>("Powiadomienie_Wyswietl @Par_IdUzytkownik", idUzytkownik);
            
            return View(query.ToList());
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List");
            }
            if (dbo.Powiadomienie.Find(id) == null)
            {
                TempData["message"] = string.Format("Brak wybranej grupy");
                return RedirectToAction("List");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Powiadomienie.Find(id).id_uzytkownik == id_user)
            {
                dbo.Powiadomienie_Usun(id);
                dbo.SaveChanges();
                TempData["message"] = string.Format("Brak wybranej grupy");
                return RedirectToAction("List");
            }
            else
            {
                TempData["message"] = string.Format("Brak wybranej grupy");
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