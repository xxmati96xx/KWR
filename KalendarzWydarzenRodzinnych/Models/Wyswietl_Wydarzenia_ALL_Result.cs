//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KalendarzWydarzenRodzinnych.Models
{
    using System;
    
    public partial class Wyswietl_Wydarzenia_ALL_Result
    {
        public int id { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public System.DateTime DataRozpoczencia { get; set; }
        public System.DateTime DataZakonczenia { get; set; }
        public int id_organizator { get; set; }
        public Nullable<System.DateTime> DataArchiwizacji { get; set; }
        public string Status_Archwizacji { get; set; }
    }
}
