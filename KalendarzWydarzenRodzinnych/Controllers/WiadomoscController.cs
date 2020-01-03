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
            var idUzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            IEnumerable<Odebrane_Wiadomosc> received = dbo.Odebrane_Wiadomosc.Include(w => w.Uzytkownik).Include(w => w.Wiadomosc).Where(w => w.Do == idUzytkownik).OrderByDescending(w => w.id);
            return View(received.ToList()); ;
           
        }

        [HttpGet]
        public ActionResult SendMessage(int? id, int? idW)
        {
            var idUzytkownik = Convert.ToInt32(User.Identity.GetUzytkownikId());
            Message message = new Message();
            if (dbo.Wydarzenie.Find(idW) != null)
            {
                message.DoGroup = (int)idW;
                ViewBag.wydarzenie = "Uczestnicy wydarzenia " + dbo.Wydarzenie.Find(idW).Tytul;
                return View(message);
            }
            if (dbo.Uzytkownik.Find(id) != null)
            {
               
                    
                    message.Do = (int)id;
                    ViewBag.uzytkownik = dbo.Uzytkownik.Find(id).Imie + " " + dbo.Uzytkownik.Find(id).Nazwisko;
                    return View(message);
                
            }
            else
            {


                
                
                message.UserColection = dbo.Uzytkownik.Where(u => u.id != idUzytkownik).ToList();
                var users = dbo.Uzytkownik
                    .Select(u => new
                    {
                        id = u.id,
                        user = u.Imie + " " + u.Nazwisko
                    }).Where(u => u.id != idUzytkownik).ToList();
                ViewBag.Do = users;
                return View(message);
            }
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
                if(message.DoGroup != 0){
                    message.SelectedID = dbo.Uczestnicy.Where(u => u.id_wydarzenie == message.DoGroup).Select(u=>u.id_uzytkownik).ToArray();
                }
                Wiadomosc wiadomosc = message.wiadomosc;
                dbo.Wiadomosc.Add(wiadomosc);
                dbo.SaveChanges();
                
                    foreach (var item in message.SelectedID)
                    {
                        Wyslane_Wiadomosc send_message = new Wyslane_Wiadomosc();
                        send_message.id_wiadomosc = wiadomosc.id;
                        send_message.Od = Convert.ToInt32(User.Identity.GetUzytkownikId());
                        if (message.Do == 0)
                        {
                            send_message.Do = item;
                        }
                        else
                        {
                            send_message.Do = message.Do;
                        }
                        dbo.Wyslane_Wiadomosc.Add(send_message);
                        dbo.SaveChanges();

                        Odebrane_Wiadomosc received_message = new Odebrane_Wiadomosc();
                        received_message.id_wiadomosc = wiadomosc.id;
                        received_message.id = send_message.id;
                        received_message.Od = Convert.ToInt32(User.Identity.GetUzytkownikId());
                        if (message.Do == 0)
                        {
                            received_message.Do = item;
                        }
                        else
                        {
                            received_message.Do = message.Do;
                        }
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
                if (!(bool)received_message.Przeczytana)
                {
                    dbo.Wiadomosc_Przeczytana(received_message.id);
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

            return View("DeleteMessageReceived", received_message);
            }


        }

        [HttpPost]
        public ActionResult DeleteMessageSend(int id)
        {
            Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Find(id);
            Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Find(id);
            if (received_message == null)
            {
                var message_received_count = dbo.Odebrane_Wiadomosc.Where(o => o.id_wiadomosc == send_message.id_wiadomosc).Count();
                var message_send_count = dbo.Wyslane_Wiadomosc.Where(o => o.id_wiadomosc == send_message.id_wiadomosc).Count();
                if (message_received_count == 0 && message_send_count == 1)
                {
                    Wiadomosc message = dbo.Wiadomosc.Find(send_message.id_wiadomosc);
                    dbo.Wyslane_Wiadomosc.Remove(send_message);
                    dbo.Wiadomosc.Remove(message);
                    dbo.SaveChanges();
                }
                else
                {

                    dbo.Wyslane_Wiadomosc.Remove(send_message);

                    dbo.SaveChanges();
                }
            }
            else
            {
                dbo.Wyslane_Wiadomosc.Remove(send_message);

                dbo.SaveChanges();

            }
            
            return RedirectToAction("ListSend");
        }

        [HttpPost]
        public ActionResult DeleteMessageReceived(int id)
        {
            Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Find(id);
            Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Find(id);
            if (send_message == null)
            {
                
                var message_received_count = dbo.Odebrane_Wiadomosc.Where(o => o.id_wiadomosc == received_message.id_wiadomosc).Count();
                var message_send_count = dbo.Wyslane_Wiadomosc.Where(o => o.id_wiadomosc == received_message.id_wiadomosc).Count();
                if (message_received_count == 1 && message_send_count == 0) {
                    Wiadomosc message = dbo.Wiadomosc.Find(received_message.id_wiadomosc);
                    dbo.Odebrane_Wiadomosc.Remove(received_message);
                    dbo.Wiadomosc.Remove(message);
                    dbo.SaveChanges();
                }
                else
                {
                    dbo.Odebrane_Wiadomosc.Remove(received_message);
                    dbo.SaveChanges();
                }
            }
            else
            {
                dbo.Odebrane_Wiadomosc.Remove(received_message);
                dbo.SaveChanges();
            }
            


            return RedirectToAction("ListReceived");
        }

        [HttpPost]
        public ActionResult DeleteMessageSendConfirm(List<int> checkbox)
        {
            if (checkbox == null)
            {
                TempData["message"] = string.Format("Musisz wybrać co najmniej jedną wiadomość do usunięcia");
                return RedirectToAction("ListSend");
            }
            List<Wyslane_Wiadomosc> send_messages = new List<Wyslane_Wiadomosc>();
            foreach (int id_message in checkbox)
            {
                Wyslane_Wiadomosc send_message = new Wyslane_Wiadomosc();
                send_message = dbo.Wyslane_Wiadomosc.Find(id_message);
                if (send_message == null)
                {
                    TempData["message"] = string.Format("Niepowodzenie usuwania. Spróbuj ponownie");
                    return RedirectToAction("ListSend");
                }
                send_messages.Add(send_message);
            }
            return View(send_messages);
        }
        [HttpPost]
        public ActionResult DeleteListMessageSend(List<int> checkbox)
        {
           
            foreach (int id_message in checkbox)
            {

                Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Find(id_message);
                Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Find(id_message);
                if (received_message == null)
                {
                    var message_received_count = dbo.Odebrane_Wiadomosc.Where(o => o.id_wiadomosc == send_message.id_wiadomosc).Count();
                    var message_send_count = dbo.Wyslane_Wiadomosc.Where(o => o.id_wiadomosc == send_message.id_wiadomosc).Count();
                    if (message_received_count == 0 && message_send_count == 1)
                    {
                        Wiadomosc message = dbo.Wiadomosc.Find(send_message.id_wiadomosc);
                        dbo.Wyslane_Wiadomosc.Remove(send_message);
                        dbo.Wiadomosc.Remove(message);
                        dbo.SaveChanges();
                    }
                    else
                    {
                        dbo.Wyslane_Wiadomosc.Remove(send_message);

                        dbo.SaveChanges();
                    }
                }
                else
                {
                    dbo.Wyslane_Wiadomosc.Remove(send_message);

                    dbo.SaveChanges();
                }


                


            }
            return RedirectToAction("ListSend");
        }


        [HttpPost]
        public ActionResult DeleteMessageReceivedConfirm(List<int> checkbox)
        {
            if (checkbox == null)
            {
                TempData["message"] = string.Format("Musisz wybrać co najmniej jedną wiadomość do usunięcia");
                return RedirectToAction("ListSend");
            }
            List<Odebrane_Wiadomosc> received_messages = new List<Odebrane_Wiadomosc>();
            foreach (int id_message in checkbox)
            {
                Odebrane_Wiadomosc received_message = new Odebrane_Wiadomosc();
                received_message = dbo.Odebrane_Wiadomosc.Find(id_message);
                if (received_message == null)
                {
                    TempData["message"] = string.Format("Niepowodzenie usuwania. Spróbuj ponownie");
                    return RedirectToAction("ListSend");
                }
                received_messages.Add(received_message);
            }
            return View(received_messages);
        }
        [HttpPost]
        public ActionResult DeleteListMessageReceived(List<int> checkbox)
        {
         
        
            foreach (int id_message in checkbox)
            {

                Wyslane_Wiadomosc send_message = dbo.Wyslane_Wiadomosc.Find(id_message);
                Odebrane_Wiadomosc received_message = dbo.Odebrane_Wiadomosc.Find(id_message);
                if (send_message == null)
                {

                    var message_received_count = dbo.Odebrane_Wiadomosc.Where(o => o.id_wiadomosc == received_message.id_wiadomosc).Count();
                    var message_send_count = dbo.Wyslane_Wiadomosc.Where(o => o.id_wiadomosc == received_message.id_wiadomosc).Count();
                    if (message_received_count == 1 && message_send_count == 0)
                    {
                        Wiadomosc message = dbo.Wiadomosc.Find(received_message.id_wiadomosc);
                        dbo.Odebrane_Wiadomosc.Remove(received_message);
                        dbo.Wiadomosc.Remove(message);
                        dbo.SaveChanges();
                    }
                    else
                    {
                        dbo.Odebrane_Wiadomosc.Remove(received_message);
                        dbo.SaveChanges();
                    }
                }
                else
                {
                    dbo.Odebrane_Wiadomosc.Remove(received_message);
                    dbo.SaveChanges();

                }


            }
            return RedirectToAction("ListReceived");
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