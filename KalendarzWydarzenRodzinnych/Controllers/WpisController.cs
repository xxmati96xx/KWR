﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Web.Mvc;
using System.Web.Helpers;
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
        [HttpGet]
        public ActionResult Create(int id_wydarzenie,int? id_przebieg)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;
            ViewBag.id_przebieg = id_przebieg;
            
            return View();
        }
        [HttpPost]
        public ActionResult Create(WpisWpisZdjecia wpisWpisZdjecia)
        {
            

                Wpis wpis = wpisWpisZdjecia.Wpis;
                
                wpis.id_uzytkownik = Convert.ToInt32(Session["id"]);
              

                dbo.Wpis.Add(wpis);
                dbo.SaveChanges();
                int id = wpis.id;
                foreach (HttpPostedFileBase image in wpisWpisZdjecia.files)
                {
                     
                    if (image != null)
                    {
                        var InputFileName = Guid.NewGuid().ToString() +Path.GetExtension(image.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Image/orginal/") + InputFileName);
                        
                        image.SaveAs(ServerSavePath);
                        WpisZdjecia wpisZdjecia = new WpisZdjecia();
                        wpisZdjecia.id_wpis = id;
                        wpisZdjecia.zdjecie = InputFileName;
                        dbo.WpisZdjecia.Add(wpisZdjecia);
                        dbo.SaveChanges();
                        WebImage img = new WebImage(image.InputStream);
                        if (img.Width > 300)
                        img.Resize(250, 250);
                        img.Save("~/Image/thumb/" + InputFileName);

                   
                            
                        
                    }
                
                }

            return RedirectToAction("List", new { id = wpis.id_wydarzenie });


        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var id_wydarzenie = dbo.Wpis.Find(id).id_wydarzenie;
            Wpis wpis = dbo.Wpis.Include(w=>w.Uzytkownik).SingleOrDefault(x => x.id == id); ;            
            ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == id);
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id_wydarzenie);
            if (wpis == null)
            {
                return HttpNotFound();
            }
            return View(wpis);
        }

    }
}