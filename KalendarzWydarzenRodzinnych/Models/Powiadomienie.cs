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
    using System.Collections.Generic;
    
    public partial class Powiadomienie
    {
        public int id { get; set; }
        public System.DateTime DataPowiadomienia { get; set; }
        public int Czestotliwosc { get; set; }
        public int id_wydarzenie { get; set; }
    
        public virtual Wydarzenie Wydarzenie { get; set; }
    }
}
