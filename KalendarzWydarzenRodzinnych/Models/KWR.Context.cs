﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class KWR : DbContext
    {
        public KWR()
            : base("name=KWR")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Grupa> Grupa { get; set; }
        public virtual DbSet<Logowanie> Logowanie { get; set; }
        public virtual DbSet<Powiadomienia> Powiadomienia { get; set; }
        public virtual DbSet<Powiadomienie> Powiadomienie { get; set; }
        public virtual DbSet<Przebieg> Przebieg { get; set; }
        public virtual DbSet<Uczestnicy> Uczestnicy { get; set; }
        public virtual DbSet<UzytkownicyWGrupie> UzytkownicyWGrupie { get; set; }
        public virtual DbSet<Uzytkownik> Uzytkownik { get; set; }
        public virtual DbSet<Wiadomosc> Wiadomosc { get; set; }
        public virtual DbSet<Wpis> Wpis { get; set; }
        public virtual DbSet<WpisZdjecia> WpisZdjecia { get; set; }
        public virtual DbSet<Wydarzenie> Wydarzenie { get; set; }
        public virtual DbSet<Zadanie> Zadanie { get; set; }
        public virtual DbSet<ZadanieUczestnik> ZadanieUczestnik { get; set; }
        public virtual DbSet<Zaproszenie> Zaproszenie { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
    
        public virtual int Wydarzenie_Uczestnik(Nullable<int> par_id_wydarzenie, Nullable<int> par_id_uzytkownik)
        {
            var par_id_wydarzenieParameter = par_id_wydarzenie.HasValue ?
                new ObjectParameter("Par_id_wydarzenie", par_id_wydarzenie) :
                new ObjectParameter("Par_id_wydarzenie", typeof(int));
    
            var par_id_uzytkownikParameter = par_id_uzytkownik.HasValue ?
                new ObjectParameter("Par_id_uzytkownik", par_id_uzytkownik) :
                new ObjectParameter("Par_id_uzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Wydarzenie_Uczestnik", par_id_wydarzenieParameter, par_id_uzytkownikParameter);
        }
    
        public virtual int Wydarzenie_Uczestnik_Usun(Nullable<int> par_id_wydarzenie, Nullable<int> par_id_uzytkownik)
        {
            var par_id_wydarzenieParameter = par_id_wydarzenie.HasValue ?
                new ObjectParameter("Par_id_wydarzenie", par_id_wydarzenie) :
                new ObjectParameter("Par_id_wydarzenie", typeof(int));
    
            var par_id_uzytkownikParameter = par_id_uzytkownik.HasValue ?
                new ObjectParameter("Par_id_uzytkownik", par_id_uzytkownik) :
                new ObjectParameter("Par_id_uzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Wydarzenie_Uczestnik_Usun", par_id_wydarzenieParameter, par_id_uzytkownikParameter);
        }
    
        public virtual ObjectResult<Wyswietl_Powiadomienia_Result> Wyswietl_Powiadomienia(Nullable<int> par_IdUzytkownik)
        {
            var par_IdUzytkownikParameter = par_IdUzytkownik.HasValue ?
                new ObjectParameter("Par_IdUzytkownik", par_IdUzytkownik) :
                new ObjectParameter("Par_IdUzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Wyswietl_Powiadomienia_Result>("Wyswietl_Powiadomienia", par_IdUzytkownikParameter);
        }
    
        public virtual ObjectResult<Wyswietl_Uzytkownikow_Result> Wyswietl_Uzytkownikow(Nullable<int> par_IdWydarzenie)
        {
            var par_IdWydarzenieParameter = par_IdWydarzenie.HasValue ?
                new ObjectParameter("Par_IdWydarzenie", par_IdWydarzenie) :
                new ObjectParameter("Par_IdWydarzenie", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Wyswietl_Uzytkownikow_Result>("Wyswietl_Uzytkownikow", par_IdWydarzenieParameter);
        }
    
        public virtual ObjectResult<Wyswietl_Wydarzenia_Result> Wyswietl_Wydarzenia(Nullable<int> par_IdUzytkownik)
        {
            var par_IdUzytkownikParameter = par_IdUzytkownik.HasValue ?
                new ObjectParameter("Par_IdUzytkownik", par_IdUzytkownik) :
                new ObjectParameter("Par_IdUzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Wyswietl_Wydarzenia_Result>("Wyswietl_Wydarzenia", par_IdUzytkownikParameter);
        }
    
        public virtual int ZadanieUczestnik_Dodaj(Nullable<int> par_IdUzytkownik, Nullable<int> par_IdZadanie)
        {
            var par_IdUzytkownikParameter = par_IdUzytkownik.HasValue ?
                new ObjectParameter("Par_IdUzytkownik", par_IdUzytkownik) :
                new ObjectParameter("Par_IdUzytkownik", typeof(int));
    
            var par_IdZadanieParameter = par_IdZadanie.HasValue ?
                new ObjectParameter("Par_IdZadanie", par_IdZadanie) :
                new ObjectParameter("Par_IdZadanie", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ZadanieUczestnik_Dodaj", par_IdUzytkownikParameter, par_IdZadanieParameter);
        }
    
        public virtual int ZadanieUczestnik_Usun(Nullable<int> par_IdUzytkownik, Nullable<int> par_IdZadanie)
        {
            var par_IdUzytkownikParameter = par_IdUzytkownik.HasValue ?
                new ObjectParameter("Par_IdUzytkownik", par_IdUzytkownik) :
                new ObjectParameter("Par_IdUzytkownik", typeof(int));
    
            var par_IdZadanieParameter = par_IdZadanie.HasValue ?
                new ObjectParameter("Par_IdZadanie", par_IdZadanie) :
                new ObjectParameter("Par_IdZadanie", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ZadanieUczestnik_Usun", par_IdUzytkownikParameter, par_IdZadanieParameter);
        }
    
        public virtual int Zaproszenie_Anuluj(Nullable<int> par_id_wydarzenie, Nullable<int> par_id_uzytkownik)
        {
            var par_id_wydarzenieParameter = par_id_wydarzenie.HasValue ?
                new ObjectParameter("Par_id_wydarzenie", par_id_wydarzenie) :
                new ObjectParameter("Par_id_wydarzenie", typeof(int));
    
            var par_id_uzytkownikParameter = par_id_uzytkownik.HasValue ?
                new ObjectParameter("Par_id_uzytkownik", par_id_uzytkownik) :
                new ObjectParameter("Par_id_uzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Zaproszenie_Anuluj", par_id_wydarzenieParameter, par_id_uzytkownikParameter);
        }
    
        public virtual int Zaproszenie_Potwierdz(Nullable<int> par_id_wydarzenie, Nullable<int> par_id_uzytkownik)
        {
            var par_id_wydarzenieParameter = par_id_wydarzenie.HasValue ?
                new ObjectParameter("Par_id_wydarzenie", par_id_wydarzenie) :
                new ObjectParameter("Par_id_wydarzenie", typeof(int));
    
            var par_id_uzytkownikParameter = par_id_uzytkownik.HasValue ?
                new ObjectParameter("Par_id_uzytkownik", par_id_uzytkownik) :
                new ObjectParameter("Par_id_uzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Zaproszenie_Potwierdz", par_id_wydarzenieParameter, par_id_uzytkownikParameter);
        }
    
        public virtual int Zadanie_Usun(Nullable<int> par_IdZadanie)
        {
            var par_IdZadanieParameter = par_IdZadanie.HasValue ?
                new ObjectParameter("Par_IdZadanie", par_IdZadanie) :
                new ObjectParameter("Par_IdZadanie", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Zadanie_Usun", par_IdZadanieParameter);
        }
    
        public virtual ObjectResult<Wyswietl_Uzytkownikow_Grupa_Result> Wyswietl_Uzytkownikow_Grupa(Nullable<int> par_IdGrupa, Nullable<int> par_IdUzytkownik)
        {
            var par_IdGrupaParameter = par_IdGrupa.HasValue ?
                new ObjectParameter("Par_IdGrupa", par_IdGrupa) :
                new ObjectParameter("Par_IdGrupa", typeof(int));
    
            var par_IdUzytkownikParameter = par_IdUzytkownik.HasValue ?
                new ObjectParameter("Par_IdUzytkownik", par_IdUzytkownik) :
                new ObjectParameter("Par_IdUzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Wyswietl_Uzytkownikow_Grupa_Result>("Wyswietl_Uzytkownikow_Grupa", par_IdGrupaParameter, par_IdUzytkownikParameter);
        }
    
        public virtual int Dodaj_Uzytkownikow_Grupa(Nullable<int> par_IdGrupa, Nullable<int> par_IdUzytkownik)
        {
            var par_IdGrupaParameter = par_IdGrupa.HasValue ?
                new ObjectParameter("Par_IdGrupa", par_IdGrupa) :
                new ObjectParameter("Par_IdGrupa", typeof(int));
    
            var par_IdUzytkownikParameter = par_IdUzytkownik.HasValue ?
                new ObjectParameter("Par_IdUzytkownik", par_IdUzytkownik) :
                new ObjectParameter("Par_IdUzytkownik", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Dodaj_Uzytkownikow_Grupa", par_IdGrupaParameter, par_IdUzytkownikParameter);
        }
    
        public virtual int Grupa_Usun(Nullable<int> par_IdGrupa)
        {
            var par_IdGrupaParameter = par_IdGrupa.HasValue ?
                new ObjectParameter("Par_IdGrupa", par_IdGrupa) :
                new ObjectParameter("Par_IdGrupa", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("Grupa_Usun", par_IdGrupaParameter);
        }
    }
}
