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
    public class PowiadomienieController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Powiadomienie
        public ActionResult List()
        {
            var id_user = Convert.ToInt32(User.Identity.GetUzytkownikId());
            IEnumerable<Powiadomienie> notification = dbo.Powiadomienie.Where(p => p.id_uzytkownik == id_user || p.id_organizator == id_user).GroupBy(p => p.DataPowiadomienia).ToList();
            
            //SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id_user);


            //IEnumerable<Powiadomienie> query = dbo.Powiadomienie.SqlQuery("Powiadomienie_Wyswietl @Par_IdUzytkownik", idUzytkownik);
            return View(notification.ToList());
        }
    }
}