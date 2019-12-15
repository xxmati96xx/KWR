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


        public ActionResult addUser(int id, int idW)
        {
            var count = dbo.Uczestnicy.Where(u => u.id_uzytkownik == id).Where(u=>u.id_wydarzenie==idW).Count();
            if (count == 0)
            {
                dbo.Wydarzenie_Uczestnik(idW, id);
            }
            return RedirectToAction("addUser", "Uzytkownik", new { id = idW });
        }

        public ActionResult GetUczestnicy(int? id_wydarzenie)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;
            ViewBag.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_organizator_wydarzenie = dbo.Wydarzenie.Find(id_wydarzenie).id_organizator;
            IEnumerable<Uczestnicy> uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Include(u=>u.Wydarzenie).Where(u => u.id_wydarzenie == id_wydarzenie);
            return PartialView("_PartialUczestnicy", uczestnicy.ToList());
        }

        public ActionResult AddGroupEvent(int id, int idW)
        {
            IList<UzytkownicyWGrupie> uzytkownicyWGrupie = dbo.UzytkownicyWGrupie.Where(uwg => uwg.id_grupa == id).ToList();
            foreach(var item in uzytkownicyWGrupie)
            {
                var count = dbo.Uczestnicy.Where(u => u.id_uzytkownik == item.id_uzytkownik).Where(u => u.id_wydarzenie == idW).Count();
                if (count == 0)
                {
                    dbo.Wydarzenie_Uczestnik(idW, item.id_uzytkownik);
                }
               
            }
            
            return RedirectToAction("AddGroupEvent", "Grupa", new { id = idW });
        }

    }
}