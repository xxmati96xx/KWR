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
using KalendarzWydarzenRodzinnych.Extensions;

namespace KalendarzWydarzenRodzinnych.Controllers
{
    [Authorize]
    public class WpisController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Wpis
        public ActionResult List(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Wydarzenie.Find(id) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranego wydarzenia");
                return RedirectToAction("List", "Wydarzenie");
            }
            
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                IEnumerable<Wpis> wpis = dbo.Wpis.Include(w => w.Uzytkownik).Where(w => w.id_wydarzenie == id);
                if (dbo.Wydarzenie.Find(id).DataArchiwizacji != null)
                {
                    
                    ViewBag.id_wydarzenie = id;
                    if (dbo.Wydarzenie.Find(id).DataArchiwizacji > DateTime.Now)
                    {
                        ViewBag.czyArchiwum = true;
                    }
                    else
                    {
                        ViewBag.czyArchiwum = false;
                    }
                    ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
                    ViewBag.zdjecia = dbo.WpisZdjecia.Include(wz => wz.Wpis).Where(wz => wz.Wpis.id_wydarzenie == id);

                    return View("ListArchiwum", wpis.ToList());
                }
                
                ViewBag.id_wydarzenie = id;
                ViewBag.id_organizator = dbo.Wydarzenie.Find(id).id_organizator;
                ViewBag.zdjecia = dbo.WpisZdjecia.Include(wz => wz.Wpis).Where(wz => wz.Wpis.id_wydarzenie == id);
                // ViewBag.wpisWpisZdjecia = new WpisWpisZdjecia();
                return View(wpis.ToList());
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu do relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
        }

        public ActionResult form(int? id_wydarzenie)
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
            if (dbo.Wydarzenie.Find(id_wydarzenie).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id_wydarzenie && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                if (dbo.Wydarzenie.Find(id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Błąd dostępu do formularza");
                    return RedirectToAction("List", new { id = id_wydarzenie });
                }
                ViewBag.id_wydarzenie = id_wydarzenie;
                WpisWpisZdjecia wpisWpisZdjecia = new WpisWpisZdjecia();
                return PartialView("_PartialCreate", wpisWpisZdjecia);
            }
            else
            {
                TempData["message"] = string.Format("Błąd dostępu do formularza");
                return RedirectToAction("List", "Wydarzenie");
            }
        }


        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do edycji relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            Wpis wpis = dbo.Wpis.Find(id);
            if (wpis == null)
            {
                TempData["message"] = string.Format("Błąd dostępu. Brak wybranej relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (wpis.id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (dbo.Wydarzenie.Find(wpis.id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = wpis.id_wydarzenie });
                }
                WpisWpisZdjecia wpisWpisZdjecia = new WpisWpisZdjecia();
                wpisWpisZdjecia.Wpis = wpis;
                ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == wpis.id);

                return View(wpisWpisZdjecia);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wpis", new { id = wpis.id_wydarzenie });
            }

        }

        [HttpPost]
        public ActionResult Edit(WpisWpisZdjecia wpisWpisZdjecia)
        {

            if (wpisWpisZdjecia.Wpis.id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
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
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wpis", new { id = wpisWpisZdjecia.Wpis.id_wydarzenie });
            }
            
        }
        [HttpGet]
        public ActionResult Create(int? id_wydarzenie, int? id_przebieg)
        {
            if (id_wydarzenie == null || id_przebieg == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do formularza");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Wydarzenie.Find(id_wydarzenie) == null || dbo.Przebieg.Find(id_przebieg) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do formularza");
                return RedirectToAction("List", "Wydarzenie");
            }

            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(id_wydarzenie).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id_wydarzenie && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null) {
                if (dbo.Wydarzenie.Find(id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = id_wydarzenie });
                }
                ViewBag.id_wydarzenie = id_wydarzenie;
                ViewBag.id_przebieg = id_przebieg;

                return View();
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Przebieg", new { id = id_wydarzenie });
            }
        }
        [HttpPost]
        public ActionResult Create(WpisWpisZdjecia wpisWpisZdjecia)
        {

            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(wpisWpisZdjecia.Wpis.id_wydarzenie).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == wpisWpisZdjecia.Wpis.id_wydarzenie && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                Wpis wpis = wpisWpisZdjecia.Wpis;

                wpis.id_uzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());


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
                        TempData["message"] = string.Format("Dodanie zdjęć nie powiodło się. Spróbuj ponownie lub skontaktuj się z administratorem aplikacji Błąd:" + e.ToString());
                    }




                }

                return RedirectToAction("List", new { id = wpis.id_wydarzenie });
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wpis", new { id = wpisWpisZdjecia.Wpis.id_wydarzenie });
            }

        }
        [HttpGet]
        public ActionResult DeleteFotoForm(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do edycji relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            if(dbo.Wpis.Find(id) == null)
            {
                TempData["message"] = string.Format("Błąd dostępu do edycji relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            //List<int> checkBox = new List<int>();
            if (dbo.Wpis.Find(id).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (dbo.Wydarzenie.Find(dbo.Wpis.Find(id).id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = dbo.Wpis.Find(id).id_wydarzenie });
                }
                ViewBag.id_wpis = id;
                ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == id);
                return PartialView();
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wpis", new { id = id });
            }
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
            if (id_wpis == null)
            {
                TempData["message"] = string.Format("Błąd edycji relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Wpis.Find(id_wpis) == null)
            {
                TempData["message"] = string.Format("Błąd edycji relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            if (dbo.Wpis.Find(id_wpis).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {
                if (dbo.Wydarzenie.Find(dbo.Wpis.Find(id_wpis).id_wydarzenie).DataArchiwizacji != null)
                {
                    TempData["message"] = string.Format("Brak dostępu");
                    return RedirectToAction("List", new { id = dbo.Wpis.Find(id_wpis).id_wydarzenie });
                }
                if (checkbox == null)
                {
                    TempData["message"] = string.Format("Musisz wybrać co najmniej jedno zdjęcie do usunięcia");
                    return RedirectToAction("Edit", new { id = id_wpis });
                }

                List<WpisZdjecia> wpisZdjecia = new List<WpisZdjecia>();
                foreach (int idZ in checkbox)
                {
                    WpisZdjecia wpis = new WpisZdjecia();
                    wpis = dbo.WpisZdjecia.Find(idZ);
                    if (wpis == null)
                    {
                        TempData["message"] = string.Format("Nie udało się usunąć wybranych zdjęć");
                        return RedirectToAction("Edit", new { id = id_wpis });
                    }
                    if (wpis.id_wpis == id_wpis)
                    {
                        wpisZdjecia.Add(wpis);
                    }
                    else
                    {
                        TempData["message"] = string.Format("Wybrane zdjęcie nie należy do wybranej realacji");
                        return RedirectToAction("Edit", new { id = id_wpis });
                    }
                }
                ViewBag.id_wpis = id_wpis;
                return View(wpisZdjecia);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wpis", new { id = id_wpis });
            }
        }

        [HttpPost]
        public ActionResult DeleteFoto(List<int> checkbox, int? id_wpis)
        {
            if (dbo.Wpis.Find(id_wpis).id_uzytkownik == Convert.ToInt32(User.Identity.GetUzytkownikId()))
            {

                WpisZdjecia wpis = new WpisZdjecia();
                foreach (int idZ in checkbox)
                {

                    wpis = dbo.WpisZdjecia.Find(idZ);
                    if (wpis == null)
                    {
                        TempData["message"] = string.Format("Nie udało się usunąć wybranych zdjęć");
                        return RedirectToAction("Edit", new { id = id_wpis });
                    }
                    if (wpis.id_wpis == id_wpis)
                    {
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
                    else
                    {
                        TempData["message"] = string.Format("Wybrane zdjęcie nie należy do wybranej realacji");
                        return RedirectToAction("Edit", new { id = id_wpis });
                    }
                }
                return RedirectToAction("Edit", new { id = wpis.id_wpis });
            }
            else
            {
                TempData["message"] = string.Format("Wybrane zdjęcie nie należy do relacji wybranej realacji");
                return RedirectToAction("Edit", new { id = id_wpis });
            }
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
           

            //var id_wydarzenie = dbo.Wpis.Find(id).id_wydarzenie;
           
            Wpis wpis = dbo.Wpis.Include(w=>w.Uzytkownik).Include(w=>w.Wydarzenie).SingleOrDefault(x => x.id == id); ;            
            ViewBag.zdjecia = dbo.WpisZdjecia.Where(wz => wz.id_wpis == id);      
            if (wpis == null)
            {
                TempData["message"] = string.Format("Brak dostępu do relacji");
                return RedirectToAction("List", "Wydarzenie");
            }
            
                var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(wpis.id_wydarzenie).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == wpis.id_wydarzenie && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                if (dbo.Wydarzenie.Find(wpis.id_wydarzenie).DataArchiwizacji != null)
                {
                    if (dbo.Wydarzenie.Find(wpis.id_wydarzenie).DataArchiwizacji > DateTime.Now)
                    {
                        ViewBag.czyArchiwumDetails = true;
                    }
                    else
                    {
                        ViewBag.czyArchiwumDetails = false;
                    }
                    return View("DetailsArchiwum", wpis);
                }

                return View(wpis);
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", new { id = id });
            }
        }

        public ActionResult Download(int? id)
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            Wpis wpis = dbo.Wpis.Find(id);
            if (wpis == null)
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (dbo.Wydarzenie.Find(wpis.id_wydarzenie).id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == wpis.id_wydarzenie && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                if (dbo.Wydarzenie.Find(wpis.id_wydarzenie).DataArchiwizacji < DateTime.Now)
                {
                    TempData["message"] = string.Format("Brak zdjęć do pobrania");
                    return RedirectToAction("List", new { id = wpis.id_wydarzenie });
                }
                IEnumerable<WpisZdjecia> lista = dbo.WpisZdjecia.Include(wz=>wz.Wpis).Where(wz => wz.id_wpis == id);
            if(!lista.Any())
            {
                TempData["message"] = string.Format("Brak zdjęć do pobrania");
                return RedirectToAction("Details", "Wpis",new { id = id });
            }

            string nazwa = "Image.zip";
                using (var memoryStream = new MemoryStream())
                {
                    using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var item in lista)
                        {

                            string orginalPath = Path.Combine(Server.MapPath("~/Image/orginal/" + item.zdjecie));
                            zip.CreateEntryFromFile(orginalPath, item.zdjecie);
                            nazwa = item.Wpis.Wydarzenie.Tytul + "(" + item.Wpis.Uzytkownik.Imie + "_" + item.Wpis.Uzytkownik.Nazwisko + "_" + item.Wpis.data_dodania + ").zip";
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", nazwa);
                }
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", new { id = id });
            }
        }

        public ActionResult DownloadAll(int? id) //Poprawić
        {
            if (id == null)
            {
                TempData["message"] = string.Format("Błąd dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            Wydarzenie wydarzenie = dbo.Wydarzenie.Find(id);
            if (wydarzenie == null)
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", "Wydarzenie");
            }
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            if (wydarzenie.id_organizator == id_user || dbo.Uczestnicy.Where(u => u.id_wydarzenie == id && u.id_uzytkownik == id_user && u.decyzja == true).FirstOrDefault() != null)
            {
                IEnumerable<Wpis> listaWpis = dbo.Wpis.Where(w => w.id_wydarzenie == id);
                if (!listaWpis.Any())
                {
                    TempData["message"] = string.Format("Brak zdjęć do pobrania");
                    return RedirectToAction("List", "Wpis", new { id = id });
                }
                string nazwaW = wydarzenie.Tytul+".zip";
                using (var memoryStreamW = new MemoryStream())
                {
                    using (var zipW = new ZipArchive(memoryStreamW, ZipArchiveMode.Create, true))
                    {
                         
                        foreach (var ite in listaWpis)
                        {
                        
                            IEnumerable<WpisZdjecia> lista = dbo.WpisZdjecia.Include(wz => wz.Wpis).Where(wz => wz.id_wpis == ite.id);
                            if (!lista.Any())
                            {

                            }
                            else
                            {
                                string nazwa = "Image.zip";
                               
                                    using (var zip = new ZipArchive(memoryStreamW, ZipArchiveMode.Create, true))
                                    {
                                        foreach (var item in lista)
                                        {
                                            string orginalPath = Path.Combine(Server.MapPath("~/Image/orginal/" + item.zdjecie));
                                            zip.CreateEntryFromFile(orginalPath, item.zdjecie);
                                            nazwa = item.Wpis.Wydarzenie.Tytul + "(" + item.Wpis.Uzytkownik.Imie + "_" + item.Wpis.Uzytkownik.Nazwisko + "_" + item.Wpis.data_dodania + ").zip";
                                            
                                        }
                                    
                                    }

                                    return File(memoryStreamW.ToArray(), "application/zip", nazwaW);


                            }
                        }
                    }
                    return File(memoryStreamW.ToArray(), "application/zip", nazwaW);
                }
            }
            else
            {
                TempData["message"] = string.Format("Brak dostępu");
                return RedirectToAction("List", new { id = id });
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