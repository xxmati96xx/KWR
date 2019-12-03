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
using KalendarzWydarzenRodzinnych.Extensions;

namespace KalendarzWydarzenRodzinnych.Controllers
{
    [Authorize]
    public class UzytkownikController : Controller
    {
        private KWR dbo = new KWR();
        
        [HttpGet]
        public ActionResult addUser(int? id)
        {
            
            var idU = Convert.ToInt32(User.Identity.GetUzytkownikId());
            ViewBag.id_wydarzenie = id;
            ViewBag.id_organizator = idU;
            ViewBag.uczestnicy = dbo.Uczestnicy.Include(u => u.Uzytkownik).Where(u => u.id_wydarzenie == id);
            SqlParameter idWydarzenie = new SqlParameter("@Par_IdWydarzenie", id);
            IEnumerable<Uzytkownik> query = dbo.Uzytkownik.SqlQuery("Wyswietl_Uzytkownikow @Par_IdWydarzenie", idWydarzenie);
            return View(query.ToList());
        }
        
        
    }
}