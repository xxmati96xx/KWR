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
    public class UczestnicyController : Controller
    {
        private KWR dbo = new KWR();


        public ActionResult addUser(int id, int idW)
        {
            dbo.Wydarzenie_Uczestnik(idW, id);
            return RedirectToAction("addUser", "Uzytkownik", new { id = idW });
        }
    }
}