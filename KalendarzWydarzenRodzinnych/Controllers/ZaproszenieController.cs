﻿using System;
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
    public class ZaproszenieController : Controller
    {
        private KWR dbo = new KWR();
        public ActionResult List()
        {
            
            var id = Convert.ToInt32(Session["id"]);
            var zaproszenia = dbo.Zaproszenie.Include(z => z.Uzytkownik1).Include(z=>z.Wydarzenie).Where(z => z.Do == id);
            return View(zaproszenia.ToList());
        }

        public ActionResult Accept(int? id)
        {
            
            var idU = Convert.ToInt32(Session["id"]);
            dbo.Zaproszenie_Potwierdz(id, idU);
            return RedirectToAction("List");
        }

        public ActionResult Cancel(int? id)
        {
            
            var idU = Convert.ToInt32(Session["id"]);
            dbo.Zaproszenie_Anuluj(id, idU);
            return RedirectToAction("List");
        }
    }
}