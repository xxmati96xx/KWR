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

        public ActionResult List(int? id) //sprawdzić działanie
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do listy przebiegu");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                IEnumerable<Przebieg> przebieg = dbo.Przebieg.Where(z => z.id_wydarzenie == id);
                ViewBag.id_wydarzenie = id;
                ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
                return View(przebieg.ToList());
            }
            else
            {

                TempData["message"] = string.Format("Błąd dostępu do listy przebiegu");
                return RedirectToAction("List", "Wydarzenie");
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do edycji zadania");
                return RedirectToAction("List", "Wydarzenie");
            }
            Przebieg przebieg = dbo.Przebieg.Find(id);
            if (przebieg == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranego zadania");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(przebieg.id_wydarzenie).id_organizator == id_user)
            {
                return View(przebieg);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Zadanie", new { id = przebieg.id_wydarzenie });
            }
            

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