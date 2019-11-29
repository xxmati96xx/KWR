using System;
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
using System.IO.Compression;
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
            ViewBag.zdjecia = dbo.WpisZdjecia.Include(wz => wz.Wpis).Where(wz => wz.Wpis.id_wydarzenie == id);
            ViewBag.wpisWpisZdjecia = new WpisWpisZdjecia();
            return View(wpis.ToList());

        }

        public ActionResult form(int? id_wydarzenie)
        {
            ViewBag.id_wydarzenie = id_wydarzenie;
            WpisWpisZdjecia wpisWpisZdjecia = new WpisWpisZdjecia();
            return PartialView("_PartialCreate", wpisWpisZdjecia);
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            Wpis wpis = dbo.Wpis.Find(id);
            WpisWpisZdjecia wpisWpisZdjecia = new WpisWpisZdjecia();
            wpisWpisZdjecia.Wpis = wpis;
            ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == wpis.id);
            if (wpis == null)
            {
                return HttpNotFound();
            }

            return View(wpisWpisZdjecia);

        }

        [HttpPost]
        public ActionResult Edit(WpisWpisZdjecia wpisWpisZdjecia)
        {

            //   if (ModelState.IsValid)
            //    {
            ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == wpisWpisZdjecia.Wpis.id);
            Wpis wpis = wpisWpisZdjecia.Wpis;
            dbo.Entry(wpis).State = EntityState.Modified;
            dbo.SaveChanges();
            foreach (HttpPostedFileBase image in wpisWpisZdjecia.files)
            {
                try
                {
                    if (image != null)
                    {
                        var InputFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/Image/orginal/") + InputFileName);

                        image.SaveAs(ServerSavePath);
                        WpisZdjecia wpisZdjecia = new WpisZdjecia();
                        wpisZdjecia.id_wpis = wpis.id;
                        wpisZdjecia.zdjecie = InputFileName;
                        dbo.WpisZdjecia.Add(wpisZdjecia);
                        dbo.SaveChanges();
                        WebImage img = new WebImage(image.InputStream);
                        if (img.Width > 300)
                            img.Resize(250, 250);
                        img.Save("~/Image/thumb/" + InputFileName);


                    }
                }
                catch (Exception e)
                {
                    TempData["message"] = string.Format("Dodanie zdjęć nie powiodło się. Spróbuj ponownie lub skontaktuj się z administratorem aplikacji Błąd:" + e.ToString());
                }

            }



            return RedirectToAction("List", new { id = wpis.id_wydarzenie });
        //}
            //   else
             //   {

              //      return View(wpisWpisZdjecia);
              //  }
            
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
                try
                {
                    if (image != null)
                    {
                        var InputFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
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
                catch (Exception e)
                {
                    TempData["message"] = string.Format("Dodanie zdjęć nie powiodło się. Spróbuj ponownie lub skontaktuj się z administratorem aplikacji Błąd:"+e.ToString());
                }
                
                        
                    
                
                }

            return RedirectToAction("List", new { id = wpis.id_wydarzenie });


        }
        [HttpGet]
        public ActionResult DeleteFotoForm(int? id)
        {
            //List<int> checkBox = new List<int>();
            ViewBag.id_wpis = id;
            ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == id);
            return PartialView();
        }
       // [HttpPost]
       // public ActionResult DeleteFotoForm(List<int> checkbox)
       // {
            //ViewBag.zdjecia = dbo.WpisZdjecia;
         //   List<WpisZdjecia> wpisZdjecia = new List<WpisZdjecia>();
        //    foreach(int idZ in checkbox)
        //    {
        //        WpisZdjecia wpis = new WpisZdjecia();
        //        wpis = dbo.WpisZdjecia.Find(idZ);
        //        wpisZdjecia.Add(wpis);
        //    }
//      return DeleteFoto(wpisZdjecia);
      //  }

        [HttpPost]
        public ActionResult DeleteFotoConfirm(List<int> checkbox, int? id_wpis)
        {
            if(checkbox == null)
            {
                TempData["message"] = string.Format("Musisz wybrać co najmniej jedno zdjęcie do usunięcia");
                return RedirectToAction("Edit", new { id = id_wpis });
            }
            List<WpisZdjecia> wpisZdjecia = new List<WpisZdjecia>();
            foreach (int idZ in checkbox)
            {
                WpisZdjecia wpis = new WpisZdjecia();
                wpis = dbo.WpisZdjecia.Find(idZ);
                if(wpis == null)
                {
                    return RedirectToAction("Edit", new { id = id_wpis });
                }
                wpisZdjecia.Add(wpis);
            }
            return View(wpisZdjecia);
        }

        [HttpPost]
        public ActionResult DeleteFoto(List<int> checkbox)
        {
            WpisZdjecia wpis = new WpisZdjecia();
            foreach (int idZ in checkbox)
            {
                
                wpis = dbo.WpisZdjecia.Find(idZ);
                dbo.WpisZdjecia.Remove(wpis);
                dbo.SaveChanges();
               
                string orginalPath = Path.Combine(Server.MapPath("~/Image/orginal/" + wpis.zdjecie));
                string thumbPath = Path.Combine(Server.MapPath("~/Image/thumb/" + wpis.zdjecie));
                if (System.IO.File.Exists(orginalPath))
                {
                    System.IO.File.Delete(orginalPath);
                    System.IO.File.Delete(thumbPath);
                }
            }
            return RedirectToAction("Edit",new { id = wpis.id_wpis});
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var id_wydarzenie = dbo.Wpis.Find(id).id_wydarzenie;
            ViewBag.id_uzytkownik = Convert.ToInt32(Session["id"]);
            Wpis wpis = dbo.Wpis.Include(w=>w.Uzytkownik).Include(w=>w.Wydarzenie).SingleOrDefault(x => x.id == id); ;            
            ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == id);
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id_wydarzenie);
            if (wpis == null)
            {
                return HttpNotFound();
            }
            return View(wpis);
        }

        public ActionResult Download(int? id)
        {
            IEnumerable<WpisZdjecia> lista = dbo.WpisZdjecia.Include(wz=>wz.Wpis).Where(wz => wz.id_wpis == 15);
            string nazwa = "Image.zip";
            using (var memoryStream = new MemoryStream())
            {
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var item in lista)
                    {
                        string orginalPath = Path.Combine(Server.MapPath("~/Image/orginal/"+item.zdjecie));
                        zip.CreateEntryFromFile(orginalPath, item.zdjecie);
                        nazwa = item.Wpis.Wydarzenie.Tytul+"("+item.Wpis.Uzytkownik.Imie+"_" + item.Wpis.Uzytkownik.Nazwisko+"_"+item.Wpis.data_dodania+").zip";
                    }
                }
                return File(memoryStream.ToArray(), "application/zip", nazwa);
            }
        }

    }
}