using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyBankingZinsüberschuss.Datenhaltung
{
    public static class Datenbank
    {
        private const decimal MillionenFaktor = 1_000_000.0M;

        private static bool _istGeladen;
        private static List<Periode>? _perioden;
        private static List<Kredit>? _kredite;
        private static List<VolumenNeugeschäft>? _voluminaNeugeschäft;
        private static List<Zinssatz>? _zinssätze;

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
                using EasyBankingContext context = new EasyBankingContext(pfadZurDatenbank);
                _perioden = context.Perioden.AsNoTracking().ToList();
                _kredite = context.Kredite.AsNoTracking().ToList();
                _voluminaNeugeschäft = context.VoluminaNeugeschäft.AsNoTracking().ToList();
                _zinssätze = context.Zinssätze.AsNoTracking().ToList();
                _istGeladen = true;
            }
            catch (Exception ex)
            {
                _istGeladen = false;
                _perioden = null;
                _kredite = null;
                _voluminaNeugeschäft = null;
                _zinssätze = null;
                throw new Exception("Das Laden der Datentabellen ist fehlgeschlagen.", ex);
            }
        }

        public static Periode Periode(int periodenID)
        {
            SicherstellenDassGeladen();
            Periode? periode = _perioden!.SingleOrDefault(eintrag => eintrag.ID == periodenID);
            if (periode == null)
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

        public static Kredit Kredit(int periodenID)
        {
            SicherstellenDassGeladen();
            Kredit? kredit = _kredite!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
            if (kredit == null)
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

        public static VolumenNeugeschäft VolumenNeugeschäft(int periodenID)
        {
            SicherstellenDassGeladen();
            VolumenNeugeschäft? volumen = _voluminaNeugeschäft!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
            if (volumen == null)
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
            if (zinssatz == null)
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

            public DbSet<Kredit> Kredite { get { return Set<Kredit>(); } }

            public DbSet<VolumenNeugeschäft> VoluminaNeugeschäft { get { return Set<VolumenNeugeschäft>(); } }

            public DbSet<Zinssatz> Zinssätze { get { return Set<Zinssatz>(); } }

            public EasyBankingContext(string pfadZurDatenbank)
            {
                _pfadZurDatenbank = pfadZurDatenbank;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=" + _pfadZurDatenbank);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Periode>().ToTable("Perioden").HasKey(periode => periode.ID);
                modelBuilder.Entity<Kredit>().ToTable("Kredite").HasKey(kredit => kredit.ID);
                modelBuilder.Entity<VolumenNeugeschäft>().ToTable("VoluminaNeugeschäft").HasKey(volumen => volumen.ID);
                modelBuilder.Entity<Zinssatz>().ToTable("Zinssätze").HasKey(zinssatz => zinssatz.ID);
            }
        }
    }
}
