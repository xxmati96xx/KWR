using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace KalendarzWydarzenRodzinnych.Models
{
    public class WpisWpisZdjecia
    {
        public Wpis Wpis { set; get; }
        public WpisZdjecia WpisZdjecia { set; get; }
        [Display(Name = "Dodaj zdjęcia:")]
        public HttpPostedFileBase[] files { get; set; }
    }
}