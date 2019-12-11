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
    public class WiadomoscController : Controller
    {
        private KWR dbo = new KWR();
        // GET: Wiadomosc
        public ActionResult ListSend()
        {
            var idUzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            IEnumerable<Wyslane_Wiadomosc> send = dbo.Wyslane_Wiadomosc.Include(w=>w.Uzytkownik).Include(w => w.Wiadomosc).Where(w => w.Od == idUzytkownik);
            return View(send.ToList()); ;
        }

        public ActionResult ListReceived()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            var users = dbo.Uzytkownik
                .Select(u => new
                {
                    id = u.id,
                    user = u.Imie + " " + u.Nazwisko
                }).ToList();
            ViewBag.Do = new MultiSelectList(users, "id", "user");
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            return View();
        }
    }
}