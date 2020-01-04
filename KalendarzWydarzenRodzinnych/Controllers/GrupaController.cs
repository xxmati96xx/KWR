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
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            Grupa grupa = dbo.Grupa.Find(id);
            if (grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if(dbo.Grupa.Find(id).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                return View(grupa);
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
        }

        public ActionResult GetUzytkownicyWGrupie(int? id_grupa)
        {
            if (id_grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            Grupa grupa = dbo.Grupa.Find(id_grupa);
            if (grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if (dbo.Grupa.Find(id_grupa).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                IEnumerable<UzytkownicyWGrupie> uzytkownicyWGrupie = dbo.UzytkownicyWGrupie.Include(u => u.Uzytkownik).Where(u => u.id_grupa == id_grupa);
                return PartialView("_PartialUzytkownicyWGrupie", uzytkownicyWGrupie.ToList());
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
        }

        public ActionResult DeleteUser(int? id_uzytkownik, int? id_grupa)
        {
            if (id_uzytkownik == null || id_grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if(dbo.Grupa.Find(id_grupa) == null  )
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if (dbo.Grupa.Find(id_grupa).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (dbo.UzytkownicyWGrupie.Where(u => u.id_grupa == id_grupa).Where(u => u.id_uzytkownik == id_uzytkownik).FirstOrDefault() != null)
                {
                    UzytkownicyWGrupie user = dbo.UzytkownicyWGrupie.Where(u => u.id_uzytkownik == id_uzytkownik).FirstOrDefault();
                    dbo.UzytkownicyWGrupie.Remove(user);
                    dbo.SaveChanges();

                    return RedirectToAction("Details", new { id = id_grupa });
                }
                else
                {
                    TempData["message"] = string.Format("Błąd usuwania użytkownika");
                    return RedirectToAction("List", "Grupa");
                }
            }
            else{
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
        }

        public ActionResult AddUserGrup(int? id_uzytkownik, int? id_grupa)
        {
            if (id_uzytkownik == null || id_grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if(dbo.Grupa.Find(id_grupa) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if (dbo.Grupa.Find(id_grupa).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (dbo.UzytkownicyWGrupie.Where(u => u.id_grupa == id_grupa).Where(u => u.id_uzytkownik == id_uzytkownik).FirstOrDefault() == null)
                {
                    dbo.Dodaj_Uzytkownikow_Grupa(id_grupa, id_uzytkownik);
                    dbo.SaveChanges();

                    return RedirectToAction("AddUserGrupa", "Uzytkownik", new { id = id_grupa });
                }
                else
                {
                    TempData["message"] = string.Format("Użytkownik jest już w grupie");
                    return RedirectToAction("List", "Grupa");
                }
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            Grupa grupa = dbo.Grupa.Find(id);
            if (grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if (dbo.Grupa.Find(id).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                return View(grupa);
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }

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
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            Grupa grupa = dbo.Grupa.Find(id);
            if (grupa == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }
            if (dbo.Grupa.Find(id).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {

                return View(grupa);
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Grupa");
            }


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
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if(wydarzenie == null) 
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_wydarzenie = id;
            if(wydarzenie.id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(id).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Błąd dostępu");
                    return RedirectToAction("ListArchiwum", "Wydarzenie");
                }
                IEnumerable<Grupa> grupa = dbo.Grupa.Where(g => g.id_uzytkownik == id_user);
                return View(grupa.ToList());
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
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