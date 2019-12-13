using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace KalendarzWydarzenRodzinnych.Models
{
    public class Message
    {
        public Wiadomosc wiadomosc { get; set; }

        public int Do { get; set; }

        public int DoGroup { get; set; }

        [NotMapped]
        public IEnumerable<Uzytkownik> UserColection { get; set; }

        [NotMapped]
        public int[] SelectedID { get; set; }
    }
}