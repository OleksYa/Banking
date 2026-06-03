using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyBankingZinsüberschuss.Datenhaltung
{
    public static class Datenbank
    {
        private const decimal MillionenFaktor = 1_000_000m;
        private static bool _istGeladen;
        private static List<Periode>? _perioden;
        private static List<VolumenNeugeschäft>? _volumenNeugeschäfte;
        private static List<Zinssatz>? _zinssätze;
        private static List<Kredit>? _kredite;
        
        public static bool IstGeladen { get { return _istGeladen; } }

        
        public static int[] PeriodenIDs
        {
            get
            {
                SicherstellenDassGeladen();
                return _perioden!.Select(periode => periode.ID).OrderBy(id => id).ToArray();
            }
        }
        
        public static void DatenbankAuslesen(string pfadZurDatenbank)
        {
            if (String.IsNullOrWhiteSpace(pfadZurDatenbank) || !File.Exists(pfadZurDatenbank))
            {
                _istGeladen = false;
                throw new Exception("Der Pfad zur Datenbank existiert nicht.");
            }

            try
            {
                using var context = new EasyBankingContext(pfadZurDatenbank);
                _perioden = context.Perioden.AsNoTracking().ToList();
                _volumenNeugeschäfte = context.VoluminaNeugeschäft.AsNoTracking().ToList();
                _zinssätze = context.Zinssätze.AsNoTracking().ToList();
                _kredite = context.Kredite.AsNoTracking().ToList();
                _istGeladen = true;
            }
            catch (Exception ex)
            {
                _istGeladen = false;
                _perioden = null;
                _volumenNeugeschäfte = null;
                _zinssätze = null;
                _kredite = null;
                throw new Exception("Das Laden der Datentabellen ist fehlgeschlagen.", ex);
            }
        }
        
        public static Periode Periode(int periodenID)
        {
            SicherstellenDassGeladen();
            Periode? periode = _perioden!.SingleOrDefault(eintrag => eintrag.ID == periodenID);
            if (periode is null)
            {
                throw new Exception("Die Periodennummer ist unbekannt.");
            }

            return new Periode
            {
                ID = periode.ID,
                Beginn = periode.Beginn,
                Ende = periode.Ende
            };
        }

        
        public static VolumenNeugeschäft VolumenNeugeschäft(int periodenID)
        {
            SicherstellenDassGeladen();
            VolumenNeugeschäft? volumen = _volumenNeugeschäfte!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
            if (volumen is null)
            {
                throw new Exception("Die Periodennummer ist unbekannt.");
            }

            return new VolumenNeugeschäft
            {
                ID = volumen.ID,
                PeriodenID = volumen.PeriodenID,
                Konsumkredite = volumen.Konsumkredite * MillionenFaktor,
                Autokredite = volumen.Autokredite * MillionenFaktor,
                Hypothekenkredite = volumen.Hypothekenkredite * MillionenFaktor,
                Girokonten = volumen.Girokonten * MillionenFaktor,
                Spareinlagen = volumen.Spareinlagen * MillionenFaktor,
                Termingelder = volumen.Termingelder * MillionenFaktor
            };
        }
        
        public static Zinssatz Zinssatz(int periodenID)
        {
            SicherstellenDassGeladen();
            Zinssatz? zinssatz = _zinssätze!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
            if (zinssatz is null)
            {
                throw new Exception("Die Periodennummer ist unbekannt.");
            }

            return new Zinssatz
            {
                ID = zinssatz.ID,
                PeriodenID = zinssatz.PeriodenID,
                Konsumkredite = zinssatz.Konsumkredite,
                Autokredite = zinssatz.Autokredite,
                Hypothekenkredite = zinssatz.Hypothekenkredite,
                Girokonten = zinssatz.Girokonten,
                Spareinlagen = zinssatz.Spareinlagen,
                Termingelder = zinssatz.Termingelder
            };
        }
        
        public static Kredit Kredit(int periodenID)
        {
            SicherstellenDassGeladen();
            Kredit? kredit = _kredite!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
            if (kredit is null)
            {
                throw new Exception("Die Periodennummer ist unbekannt.");
            }

            return new Kredit
            {
                ID = kredit.ID,
                PeriodenID = kredit.PeriodenID,
                Überziehungskredit = kredit.Überziehungskredit * MillionenFaktor,
                Verbindlichkeiten = kredit.Verbindlichkeiten * MillionenFaktor,
                Forderungen = kredit.Forderungen * MillionenFaktor
            };
        }

        private static void SicherstellenDassGeladen()
        {
            if (!IstGeladen)
            {
                throw new Exception("Es ist keine Datenbank geladen.");
            }
        }

        private sealed class EasyBankingContext : DbContext
        {
            private readonly string _pfadZurDatenbank;

            public DbSet<Periode> Perioden { get { return Set<Periode>(); } }

            public DbSet<VolumenNeugeschäft> VoluminaNeugeschäft { get { return Set<VolumenNeugeschäft>(); } }

            public DbSet<Zinssatz> Zinssätze { get { return Set<Zinssatz>(); } }

            public DbSet<Kredit> Kredite { get { return Set<Kredit>(); } }

            public EasyBankingContext(string pfadZurDatenbank)
            {
                _pfadZurDatenbank = pfadZurDatenbank;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_pfadZurDatenbank};";
                optionsBuilder.UseJet(connectionString);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Periode>().ToTable("Perioden").HasKey(periode => periode.ID);
                modelBuilder.Entity<VolumenNeugeschäft>().ToTable("VoluminaNeugeschäft").HasKey(volumen => volumen.ID);
                modelBuilder.Entity<Zinssatz>().ToTable("Zinssätze").HasKey(zinssatz => zinssatz.ID);
                modelBuilder.Entity<Kredit>().ToTable("Kredite").HasKey(kredit => kredit.ID);

                modelBuilder.Entity<Periode>().Property(periode => periode.ID).HasColumnName("ID");
                modelBuilder.Entity<VolumenNeugeschäft>().Property(volumen => volumen.PeriodenID).HasColumnName("PeriodenID");
                modelBuilder.Entity<Zinssatz>().Property(zinssatz => zinssatz.PeriodenID).HasColumnName("PeriodenID");
                modelBuilder.Entity<Kredit>().Property(kredit => kredit.PeriodenID).HasColumnName("PeriodenID");
            }
        }
    }
}
