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
    public class ZadanieController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Zadanie
        public ActionResult List(int? id)
        {
            IEnumerable<Zadanie> zadania = dbo.Zadanie.Where(z => z.id_wydarzenie == id);
            ViewBag.id_wydarzenie = id;
            ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id);
            ViewBag.zadanieUczestnik = dbo.ZadanieUczestnik.Include(zu=>zu.Uzytkownik).ToList();
            ViewBag.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            return View(zadania.ToList());
        }
        
        public ActionResult addZadanieUczestnik(int idZ, int idW)
        {
            var idU = Convert.ToInt32(User.Identity.GetUzytkownikId());
            var liczba_uczstnikow = dbo.Zadanie.Find(idZ).liczba_uczestnikow;
            if (liczba_uczstnikow > 0)
            {
                dbo.ZadanieUczestnik_Dodaj(idU, idZ);
            }
            else
            {
                
                TempData["message"] = string.Format("Do zadania przydzielona jest maksymalna liczba uczestników");
            }
            return RedirectToAction("List", "Zadanie", new { id = idW });
        }
        public ActionResult deleteZadanieUczestnik(int idZ, int idW)
        {
            var idU = Convert.ToInt32(User.Identity.GetUzytkownikId());
            try
            {
                dbo.ZadanieUczestnik_Usun(idU, idZ);
            }
            catch (Exception ex)
            {
                TempData["message"] = string.Format("Operacja niedozwolona. Skontaktuj się z administratorem aplikacji lub organizatorem wydarzenia");
            }


            return RedirectToAction("List", "Zadanie", new { id = idW });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Zadanie zadanie = dbo.Zadanie.Find(id);
            if (zadanie == null)
            {
                return HttpNotFound();
            }

            return View(zadanie);

        }

        [HttpPost]
        public ActionResult Edit(Zadanie zadanie)
        {
            if (zadanie.id == 0)
            {
                if (ModelState.IsValid)
                {
                    
                    dbo.Zadanie.Add(zadanie);
                    dbo.SaveChanges();
                    return RedirectToAction("List",new { id = zadanie.id_wydarzenie });
                }
                else
                {
                    return View(zadanie);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dbo.Entry(zadanie).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("List", new { id = zadanie.id_wydarzenie });
                }
                else
                {

                    return View(zadanie);
                }
            }
        }

        public ActionResult Create(int? id_wydarzenie)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;
            
            return View("Edit", new Zadanie());
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Zadanie zadanie = dbo.Zadanie.Find(id);
            if (zadanie == null)
            {
                return HttpNotFound();
            }

            return View(zadanie);


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Zadanie zadanie = dbo.Zadanie.Find(id);
            dbo.Zadanie_Usun(id);
            dbo.SaveChanges();
            return RedirectToAction("List", new { id = zadanie.id_wydarzenie });
        }
    }
}