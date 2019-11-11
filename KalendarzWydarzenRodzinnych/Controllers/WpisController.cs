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
            return View(wpis.ToList());

        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Wpis wpis = dbo.Wpis.Find(id);
            if (wpis == null)
            {
                return HttpNotFound();
            }

            return View(wpis);

        }

        [HttpPost]
        public ActionResult Edit(Wpis wpis)
        {
            if (wpis.id == 0)
            {
                if (ModelState.IsValid)
                {
                    wpis.id_uzytkownik = Convert.ToInt32(Session["id"]);
                    dbo.Wpis.Add(wpis);
                    dbo.SaveChanges();
                    return RedirectToAction("List", new { id = wpis.id_wydarzenie });
                }
                else
                {
                    return View(wpis);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dbo.Entry(wpis).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("List", new { id = wpis.id_wydarzenie });
                }
                else
                {

                    return View(wpis);
                }
            }
        }
        public ActionResult Create(int? id_wydarzenie,int? id_przebieg)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;
            ViewBag.id_przebieg = id_przebieg;

            return View("Edit", new Wpis());
        }
    }
}