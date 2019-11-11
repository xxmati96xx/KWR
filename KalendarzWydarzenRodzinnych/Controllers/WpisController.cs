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
    public class WpisController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Wpis
        public ActionResult List(int? id)
        {
            IEnumerable<Wpis> wpis = dbo.Wpis.Include(w=>w.Uzytkownik).Where(w => w.id_wydarzenie == id);
            ViewBag.id_wydarzenie = id;
            ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id);
            ViewBag.id_uzytkownik = Convert.ToInt32(Session["id"]);
            return View(dbo.Wpis);
        }
    }
}