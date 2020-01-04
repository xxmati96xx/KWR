using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;
using KalendarzWydarzenRodzinnych.Models;
using KalendarzWydarzenRodzinnych.Extensions;

namespace KalendarzWydarzenRodzinnych.Controllers
{
    [Authorize]
    public class WydarzenieController : Controller
    {
       
        private KWR dbo = new KWR();
        
        // GET: Wydarzenie
        public ActionResult List()
        {
            //var idU = User.Identity.GetUzytkownikId();
            //var user = dbo.AspNetUsers.Find(UserId).id_uzytkownik;
            
            //int cos = 1;
            //User.Identity.GetUzytkownikId() = cos;
            var id = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_organizator = id;
            

            // IEnumerable<Wydarzenie> query = (from w in dbo.Wydarzenie.DefaultIfEmpty()
            //                                 from u in dbo.Uczestnicy.Where(u => u.id_wydarzenie == w.id).DefaultIfEmpty()                                           
            //                                 where (w.id_organizator == id || u.id_uzytkownik == id) && w.DataZakonczenia >= @DateTime.Now                                            
            //                                 select w);


            // var query =dbo.Wydarzenie.SqlQuery("exec Wyswietl_Wydarzenia @Par_IdUzytkownik ",1).ToList<Wydarzenie>();
            //ViewBag.wydarzenia = query;

            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id);
            

            IEnumerable<Wydarzenie> query = dbo.Wydarzenie.SqlQuery("Wyswietl_Wydarzenia @Par_IdUzytkownik", idUzytkownik);



            return View(query.ToList<Wydarzenie>());


        }

        public ActionResult ListArchiwum()
        {
            
            var id = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_organizator = id;
            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id);
            IEnumerable<Wydarzenie> query = dbo.Wydarzenie.SqlQuery("Wyswietl_Wydarzenia_Archiwum @Par_IdUzytkownik", idUzytkownik);
            return View(query.ToList<Wydarzenie>());


        }
        public ActionResult ListAll()
        {

            var id = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_organizator = id;
            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id);
            IEnumerable<Wydarzenie> query = dbo.Wydarzenie.SqlQuery("Wyswietl_Wydarzenia_ALL @Par_IdUzytkownik", idUzytkownik);
            return View(query.ToList<Wydarzenie>());


        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do wydarzenia");
                return RedirectToAction("List");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                TempData["message"] = string.Format("Brak wybranego wydarzenia");
                return RedirectToAction("List");
            }
           
            if(wydarzenie.id_organizator == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (wydarzenie.DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("ListArchium");
                }
                return View(wydarzenie);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List");
            }

        }

        [HttpPost]
        public ActionResult Edit(Wydarzenie wydarzenie)
        {
            if (wydarzenie.id == 0)
            {
                
                
                    wydarzenie.id_organizator = Convert.ToInt32(User.Identity.GetUzytkownikId());
                    dbo.Wydarzenie.Add(wydarzenie);
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dbo.Entry(wydarzenie).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {

                    return View(wydarzenie);
                }
            }
        }

        public ActionResult Create()
        {
            
            return View(); 
        }

        public ActionResult GetOpis(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do wydarzenia");
                return RedirectToAction("List");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if(wydarzenie == null)
            {
                TempData["message"] = string.Format("Brak wybranego wydarzenia");
                return RedirectToAction("List");
            }
            
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (wydarzenie.id_organizator == id_user || dbo.Uczestnicy.Where(u=>u.id_wydarzenie == wydarzenie.id && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() !=null)
            {
                if (wydarzenie.DataArchiwizacji != null)
                {
                    return View("GetOpisArchiwum", wydarzenie);
                }
                return View(wydarzenie);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List");
            }
        }

        [HttpGet]
        public ActionResult EditOpis(int? id )
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do edycji opisu");
                return RedirectToAction("List");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                TempData["message"] = string.Format("Brak wybranego wydarzenia");
                return RedirectToAction("GetOpis", new { id = wydarzenie.id });
            }

            if (wydarzenie.id_organizator == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if(wydarzenie.DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("GetOpis", new { id = wydarzenie.id });
                }
                return View(wydarzenie);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("GetOpis", new { id = wydarzenie.id });
            }

        }
        [HttpPost]
        public ActionResult EditOpis(Wydarzenie wydarzenie)
        {
            
                if (ModelState.IsValid)
                {
                    dbo.Entry(wydarzenie).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("GetOpis",new { id=wydarzenie.id });
                }
                else
                {

                    return View(wydarzenie);
                }
         
        }

        public ActionResult Archiwizacja(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do wydarzenia");
                return RedirectToAction("List");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                TempData["message"] = string.Format("Brak wybranego wydarzenia");
                return RedirectToAction("ListAll");
            }
            if (wydarzenie.id_organizator == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (wydarzenie.DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("ListArchiwum");
                }
                dbo.Archiwizacja_Wydarzenia(id);
                return RedirectToAction("ListArchiwum");
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("ListAll");
            }
        }
        public ActionResult CancelArchiwizacja(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do wydarzenia");
                return RedirectToAction("List");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                TempData["message"] = string.Format("Brak wybranego wydarzenia");
                return RedirectToAction("ListAll");
            }
            if (wydarzenie.id_organizator == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (wydarzenie.DataArchiwizacji == null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("ListAll");
                }
                dbo.Archiwizacja_Cancel(id);

                return RedirectToAction("ListAll");
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("ListAll");
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


