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
            if(id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do listy zadań");
                return RedirectToAction("List","Wydarzenie");
            }
            if(dbo.Wydarzenie.Find(id) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranego wydarzenia");
                return RedirectToAction("List", "Wydarzenie");
            }
            
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                IEnumerable<Zadanie> zadania = dbo.Zadanie.Where(z => z.id_wydarzenie == id);
                if (dbo.Wydarzenie.Find(id).DataArchiwizacji != null)
                {
                    
                    ViewBag.id_wydarzenie = id;
                    ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
                    ViewBag.zadanieUczestnik = dbo.ZadanieUczestnik.Include(zu => zu.Uzytkownik).ToList();
                    return View("ListArchiwum", zadania.ToList());

                }
                
                ViewBag.id_wydarzenie = id;
                ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
                ViewBag.zadanieUczestnik = dbo.ZadanieUczestnik.Include(zu => zu.Uzytkownik).ToList();
                return View(zadania.ToList());
            }
            else
            {

                TempData["message"] = string.Format("Błąd dostępu do listy zadań");
                return RedirectToAction("List","Wydarzenie");
            }
        }
        
        public ActionResult addZadanieUczestnik(int? idZ, int? idW)
        {
            if (idZ == null || idW == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do zadania");
                return RedirectToAction("List", "Wydarzenie");
            }
            if(dbo.Zadanie.Find(idZ) == null || dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Podane wartości są nieprawidłowe");
                return RedirectToAction("List", "Wydarzenie");
            }
            
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.ZadanieUczestnik.Where(zu=>zu.id_uzytkownik == id_user && zu.id_zadanie == idZ).FirstOrDefault() != null)
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = idW });
                }
                TempData["message"] = string.Format("Należysz już do zadania");
                return RedirectToAction("List", "Zadanie", new { id = idW });
            }
            
            var liczba_uczstnikow = dbo.Zadanie.Find(idZ).liczba_uczestnikow;
            if (liczba_uczstnikow > 0)
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = idW });
                }
                dbo.ZadanieUczestnik_Dodaj(id_user, idZ);
            }
            else
            {
                
                TempData["message"] = string.Format("Do zadania przydzielona jest maksymalna liczba uczestników");
            }
            return RedirectToAction("List", "Zadanie", new { id = idW });
        }
        public ActionResult deleteZadanieUczestnik(int? idZ, int? idW)
        {
            if (idZ == null || idW == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do zadania");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Zadanie.Find(idZ) == null || dbo.Wydarzenie.Find(idW) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Podane wartości są nieprawidłowe");
                return RedirectToAction("List", "Wydarzenie");
            }
            
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.ZadanieUczestnik.Where(zu => zu.id_uzytkownik == id_user && zu.id_zadanie == idZ).FirstOrDefault() == null)
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = idW });
                }
                TempData["message"] = string.Format("Nie należysz do wybranego zadania");
                return RedirectToAction("List", "Zadanie", new { id = idW });
            }
            try
            {
                if (dbo.Wydarzenie.Find(idW).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = idW });
                }
                dbo.ZadanieUczestnik_Usun(id_user, idZ);
            }
            catch (Exception ex)
            {
                TempData["message"] = string.Format("Nie należysz do wybranego zadania");
            }


            return RedirectToAction("List", "Zadanie", new { id = idW });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do edycji zadania");
                return RedirectToAction("List", "Wydarzenie");
            }
            Zadanie zadanie = dbo.Zadanie.Find(id);
            if (zadanie == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranego zadania");
                return RedirectToAction("List", "Wydarzenie");
            }           
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(zadanie.id_wydarzenie).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(zadanie.id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = id });
                }
                return View(zadanie);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Zadanie",new { id = zadanie.id_wydarzenie});
            }                         
        }

        [HttpPost]
        public ActionResult Edit(Zadanie zadanie)
        {
            if (zadanie.id == 0)
            {
                if (ModelState.IsValid)
                {
                    if (zadanie.liczba_uczestnikow < 0)
                    {
                        TempData["message"] = string.Format("Wybrana liczba uczestników nie może być mniejsza od 0");
                        return View(zadanie);
                    }
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
                    if (zadanie.liczba_uczestnikow < 0)
                    {
                        TempData["message"] = string.Format("Wybrana liczba uczestników nie może być mniejsza od 0");
                        return View(zadanie);
                    }
                    dbo.Entry(zadanie).State = EntityState.Modified;
                    dbo.SaveChanges();
                    dbo.Powiadomienie_Edit(zadanie.id_wydarzenie);
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
            if (id_wydarzenie == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do formularza");
                return RedirectToAction("List", "Wydarzenie");
            }
            
            if (dbo.Wydarzenie.Find(id_wydarzenie) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do formularza");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id_wydarzenie).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = id_wydarzenie });
                }
                ViewBag.id_wydarzenie = id_wydarzenie;

                return View("Edit", new Zadanie());
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Zadanie", new { id = id_wydarzenie });
            }
            
        }

        [HttpGet]
        public ActionResult Delete(int? id) //sprawdzić działanie
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            Zadanie zadanie = dbo.Zadanie.Find(id);
            if (zadanie == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranego zadania");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(zadanie.id_wydarzenie).id_organizator == id_user)
            {
                if (dbo.Wydarzenie.Find(zadanie.id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List",new { id = id });
                }
                return View(zadanie);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Zadanie", new { id = zadanie.id_wydarzenie });
            }

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Zadanie zadanie = dbo.Zadanie.Find(id);
            dbo.Zadanie_Usun(id);
            dbo.SaveChanges();
            return RedirectToAction("List", new { id = zadanie.id_wydarzenie });
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