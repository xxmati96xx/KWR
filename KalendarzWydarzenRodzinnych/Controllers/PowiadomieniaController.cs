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
    public class PowiadomieniaController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Powiadomienia
        public ActionResult List()
        {
            var id = Convert.ToInt32(User.Identity.GetUzytkownikId());
            SqlParameter idUzytkownik = new SqlParameter("@Par_IdUzytkownik", id);
            var query = dbo.Wyswietl_Powiadomienia(id);
            return View(query);
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