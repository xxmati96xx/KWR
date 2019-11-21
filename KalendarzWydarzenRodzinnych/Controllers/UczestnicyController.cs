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

namespace KalendarzWydarzenRodzinnych.Controllers
{
    public class UczestnicyController : Controller
    {
        private KWR dbo = new KWR();


        public ActionResult addUser(int id, int idW)
        {
            dbo.Wydarzenie_Uczestnik(idW, id);
            return RedirectToAction("addUser", "Uzytkownik", new { id = idW });
        }

        public ActionResult GetUczestnicy(int? id_wydarzenie)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;
            ViewBag.id_uzytkownik = Convert.ToInt32(Session["id"]);
            ViewBag.id_organizator_wydarzenie = dbo.Wydarzenie.Find(id_wydarzenie).id_organizator;
            IEnumerable<Uczestnicy> uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Include(u=>u.Wydarzenie).Where(u => u.id_wydarzenie == id_wydarzenie);
            return PartialView("_PartialUczestnicy", uczestnicy.ToList());
        }
    }
}