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
    public class PrzebiegController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Przebieg

        public ActionResult List(int? id)
        {
            IEnumerable<Przebieg> przebieg = dbo.Przebieg.Where(z => z.id_wydarzenie == id);
            ViewBag.id_wydarzenie = id;
            ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id);
            ViewBag.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            return View(przebieg.ToList());
            
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Przebieg przebieg = dbo.Przebieg.Find(id);
            if (przebieg == null)
            {
                return HttpNotFound();
            }

            return View(przebieg);

        }

        [HttpPost]
        public ActionResult Edit(Przebieg przebieg)
        {
            if (przebieg.id == 0)
            {
                if (ModelState.IsValid)
                {

                    dbo.Przebieg.Add(przebieg);
                    dbo.SaveChanges();
                    return RedirectToAction("List", new { id = przebieg.id_wydarzenie });
                }
                else
                {
                    return View(przebieg);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dbo.Entry(przebieg).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("List", new { id = przebieg.id_wydarzenie });
                }
                else
                {

                    return View(przebieg);
                }
            }
        }
        public ActionResult Create(int? id_wydarzenie)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;

            return View("Edit", new Przebieg());
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Przebieg przebieg = dbo.Przebieg.Find(id);
            if (przebieg == null)
            {
                return HttpNotFound();
            }

            return View(przebieg);


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Przebieg przebieg = dbo.Przebieg.Find(id);
            dbo.Przebieg.Remove(przebieg);
            dbo.SaveChanges();
            return RedirectToAction("List", new { id = przebieg.id_wydarzenie });
        }

    }
}