using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KalendarzWydarzenRodzinnych.Models
{
    public class WpisWpisZdjecia
    {
        public Wpis Wpis { set; get; }
        public WpisZdjecia WpisZdjecia { set; get; }
        public HttpPostedFileBase[] files { get; set; }
    }
}