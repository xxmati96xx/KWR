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
            IEnumerable<Wyslane_Wiadomosc> send = dbo.Wyslane_Wiadomosc.Include(w=>w.Uzytkownik).Include(w => w.Wiadomosc).Where(w => w.Od == idUzytkownik).OrderByDescending(w=>w.id);
            return View(send.ToList()); ;
        }

        public ActionResult ListReceived()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SendMessage()
        {
            Message message = new Message();
            message.UserColection = dbo.Uzytkownik.ToList();
            var users = dbo.Uzytkownik
                .Select(u => new
                {
                    id = u.id,
                    user = u.Imie + " " + u.Nazwisko
                }).ToList();
            ViewBag.Do = users;
            return View(message);
        }

        [HttpPost]
        public ActionResult SendMessage(Message message)
        {
            if(message == null)
            {
                return HttpNotFound();
            }
            try
            {
                Wiadomosc wiadomosc = message.wiadomosc;
                dbo.Wiadomosc.Add(wiadomosc);
                dbo.SaveChanges();
                if (message.SelectedID.Length != 0)
                    foreach (var item in message.SelectedID)
                    {
                        Wyslane_Wiadomosc send_message = new Wyslane_Wiadomosc();
                        send_message.id_wiadomosc = wiadomosc.id;
                        send_message.Od = Convert.ToInt32(User.Identity.GetUzytkownikId());
                        send_message.Do = item;
                        dbo.Wyslane_Wiadomosc.Add(send_message);

                        Odebrane_Wiadomosc received_message = new Odebrane_Wiadomosc(); //zmiana na nie dynamiczne id i branie go z send_message zabezpieczenie przed ewentualnymi błedami przy jednoczesnym zapisie do bazy z wielu różnych miejsc
                        received_message.id_wiadomosc = wiadomosc.id;
                        received_message.Od = Convert.ToInt32(User.Identity.GetUzytkownikId());
                        received_message.Do = item;
                        dbo.Odebrane_Wiadomosc.Add(received_message);
                        dbo.SaveChanges();
                    }
            }catch(Exception ex)
            {

            }
            return RedirectToAction("ListSend");
        }

        public ActionResult Details(int? id,string r)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            if(r == "s")
            {
                Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Include(s=>s.Wiadomosc).Include(s => s.Uzytkownik1).Include(s => s.Uzytkownik).Where(s => s.id == id).FirstOrDefault(); 
               
                if (send_message == null)
                {
                    return HttpNotFound();
                }
                return View("DetailsSend",send_message);
            }
            else
            {
                Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Include(s => s.Wiadomosc).Include(s => s.Uzytkownik1).Include(s => s.Uzytkownik).Where(s => s.id == id).FirstOrDefault();
                if (received_message == null)
                {
                    return HttpNotFound();
                }
                return View("DetailsReceived",received_message);
            }
        }

        [HttpGet]
        public ActionResult DeleteMessage(int? id, string r)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(r == "s") {
                Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Find(id);
                if (send_message == null)
                {
                    return HttpNotFound();
                }
            
            return View("DeleteMessageSend",send_message);
            }
            else
            {
                Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Find(id);
                if (received_message == null)
                {
                    return HttpNotFound();
                }

            return View("DeleteMessageRecived", received_message);
            }


        }

        [HttpPost]
        public ActionResult DeleteMessageSend(int id)
        {
            Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Find(id);
            if (received_message == null)
            {
                Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Find(id);
                Wiadomosc message = dbo.Wiadomosc.Find(send_message.id_wiadomosc);
                dbo.Wyslane_Wiadomosc.Remove(send_message);
                dbo.Wiadomosc.Remove(message);
                dbo.SaveChanges();
            }
            
            return RedirectToAction("ListSend");
        }
    
    }
}