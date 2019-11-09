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
   
    public class WydarzenieController : Controller
    {
       
        private KWR dbo = new KWR();
        
        // GET: Wydarzenie
        public ActionResult List()
        {
            int cos = 1;
            Session["id"] = cos;
            var id = Convert.ToInt32(Session["id"]);
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
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                return HttpNotFound();
            }
            return View(wydarzenie);

        }

        [HttpPost]
        public ActionResult Edit(Wydarzenie wydarzenie)
        {
            if (wydarzenie.id == 0)
            {
                if (ModelState.IsValid)
                {
                    wydarzenie.id_organizator = Convert.ToInt32(Session["id"]);
                    dbo.Wydarzenie.Add(wydarzenie);
                    dbo.SaveChanges();
                    return RedirectToAction("List");
                }
                else
                {
                    return View(wydarzenie);
                }
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
            
            return View("Edit",new Wydarzenie());
        }

        public ActionResult dajOpis(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if(wydarzenie == null)
            {
                return HttpNotFound();
            }
            
            var idU = Convert.ToInt32(Session["id"]);
            ViewBag.id_organizator = idU;
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u=>u.Uzytkownik).Where(u=>u.id_wydarzenie == id);
            return View(wydarzenie);
        }

        [HttpGet]
        public ActionResult EditOpis(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                return HttpNotFound();
            }
            return View(wydarzenie);

        }
        [HttpPost]
        public ActionResult EditOpis(Wydarzenie wydarzenie)
        {
            
                if (ModelState.IsValid)
                {
                    dbo.Entry(wydarzenie).State = EntityState.Modified;
                    dbo.SaveChanges();
                    return RedirectToAction("dajOpis",new { id=wydarzenie.id });
                }
                else
                {

                    return View(wydarzenie);
                }
         
        }

    }

 }


