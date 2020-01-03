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
    public class GrupaController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Grupa
        public ActionResult List()
        {
            var idUzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            IEnumerable<Grupa> grupa = dbo.Grupa.Where(g => g.id_uzytkownik == idUzytkownik);
            return View(grupa.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Grupa grupa = dbo.Grupa.Find(id);
            if (grupa == null)
            {
                return HttpNotFound();
            }
            return View(grupa);
        }

        public ActionResult GetUzytkownicyWGrupie(int? id_grupa)
        {
            
            IEnumerable<UzytkownicyWGrupie> uzytkownicyWGrupie = dbo.UzytkownicyWGrupie.Include(u => u.Uzytkownik).Where(u => u.id_grupa == id_grupa);
            return PartialView("_PartialUzytkownicyWGrupie", uzytkownicyWGrupie.ToList());
        }

        public ActionResult DeleteUser(int? id_uzytkownik, int? id_grupa)
        {
            if (id_uzytkownik == null || id_grupa == null)
            {
                return HttpNotFound();
            }
            UzytkownicyWGrupie user = dbo.UzytkownicyWGrupie.Where(u=>u.id_uzytkownik ==id_uzytkownik).FirstOrDefault();
            dbo.UzytkownicyWGrupie.Remove(user);
            dbo.SaveChanges();

            return RedirectToAction("Details", new { id = id_grupa });
        }

        public ActionResult AddUserGrup(int? id_uzytkownik, int? id_grupa)
        {
            if (id_uzytkownik == null || id_grupa == null)
            {
                return HttpNotFound();
            }
            dbo.Dodaj_Uzytkownikow_Grupa(id_grupa, id_uzytkownik);
            dbo.SaveChanges();
            
            return RedirectToAction("AddUserGrupa", "Uzytkownik", new { id = id_grupa });
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Grupa grupa = dbo.Grupa.Find(id);
            if (grupa == null)
            {
                return HttpNotFound();
            }
            return View(grupa);

        }

        [HttpPost]
        public ActionResult Edit(Grupa grupa)
        {
            if (grupa.id == 0)
            {

                if (ModelState.IsValid)
                {
                   grupa.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
                    dbo.Grupa.Add(grupa);
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    return View(grupa);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dbo.Entry(grupa).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {

                    return View(grupa);
                }
            }
        }

        public ActionResult Create()
        {

            return View("Edit", new Grupa());  ///zmienić na create osobny formularz
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grupa grupa = dbo.Grupa.Find(id);
            if (grupa == null)
            {
                return HttpNotFound();
            }

            return View(grupa);


        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
           
            dbo.Grupa_Usun(id);
            dbo.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddGroupEvent(int? id)  
        {

            var idU = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_wydarzenie = id;
            IEnumerable<Grupa> grupa = dbo.Grupa;
            return View(grupa.ToList());
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