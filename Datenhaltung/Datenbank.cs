using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EasyBankingZinsüberschuss.Datenhaltung;

/// <summary>
/// Dies ist die statische Klasse zum gekapselten Zugriff auf die Datenbank.
/// Autor: TODO Name, Matrikelnummer: TODO
/// </summary>
public static class Datenbank
{
    private const decimal MillionenFaktor = 1_000_000m;
    private static List<Periode>? perioden;
    private static List<VolumenNeugeschäft>? volumenNeugeschäfte;
    private static List<Zinssatz>? zinssätze;
    private static List<Kredit>? kredite;

    /// <summary>
    /// Diese Eigenschaft gibt an, ob Zugriff auf eine Datenbank besteht.
    /// </summary>
    public static bool IstGeladen { get; private set; }

    /// <summary>
    /// Auflistung aller Periodennummern der geladenen Datenbank.
    /// </summary>
    /// <exception cref="Exception">Wird ausgelöst, falls keine Datenbank geladen ist.</exception>
    public static int[] PeriodenIDs
    {
        get
        {
            SicherstellenDassGeladen();
            return perioden!.Select(periode => periode.ID).OrderBy(id => id).ToArray();
        }
    }

    /// <summary>
    /// Lädt eine Datenbank.
    /// </summary>
    /// <param name="pfadZurDatenbank">Pfad zur Datenbank.</param>
    /// <exception cref="Exception">Wird ausgelöst, falls der Pfad nicht existiert oder das Laden fehlschlägt.</exception>
    public static void DatenbankAuslesen(string pfadZurDatenbank)
    {
        if (string.IsNullOrWhiteSpace(pfadZurDatenbank) || !File.Exists(pfadZurDatenbank))
        {
            IstGeladen = false;
            throw new Exception("Der Pfad zur Datenbank existiert nicht.");
        }

        try
        {
            using var context = new EasyBankingContext(pfadZurDatenbank);
            perioden = context.Perioden.AsNoTracking().ToList();
            volumenNeugeschäfte = context.VoluminaNeugeschäft.AsNoTracking().ToList();
            zinssätze = context.Zinssätze.AsNoTracking().ToList();
            kredite = context.Kredite.AsNoTracking().ToList();
            IstGeladen = true;
        }
        catch (Exception ex)
        {
            IstGeladen = false;
            perioden = null;
            volumenNeugeschäfte = null;
            zinssätze = null;
            kredite = null;
            throw new Exception("Das Laden der Datentabellen ist fehlgeschlagen.", ex);
        }
    }

    /// <summary>
    /// Liefert zur angegebenen Periodennummer die zugehörige Zeile der Tabelle 'Perioden'.
    /// </summary>
    /// <param name="periodenID">Periodennummer.</param>
    /// <returns>Zugehörige Zeile der Tabelle 'Perioden'.</returns>
    /// <exception cref="Exception">Wird bei nicht geladener Datenbank oder unbekannter Periodennummer ausgelöst.</exception>
    public static Periode Periode(int periodenID)
    {
        SicherstellenDassGeladen();
        var periode = perioden!.SingleOrDefault(eintrag => eintrag.ID == periodenID);
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

    /// <summary>
    /// Liefert zur angegebenen Periodennummer die zugehörige Zeile der Tabelle 'VoluminaNeugeschäft'.
    /// </summary>
    /// <param name="periodenID">Periodennummer.</param>
    /// <returns>Zugehörige Zeile der Tabelle 'VoluminaNeugeschäft'.</returns>
    /// <exception cref="Exception">Wird bei nicht geladener Datenbank oder unbekannter Periodennummer ausgelöst.</exception>
    public static VolumenNeugeschäft VolumenNeugeschäft(int periodenID)
    {
        SicherstellenDassGeladen();
        var volumen = volumenNeugeschäfte!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
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

    /// <summary>
    /// Liefert zur angegebenen Periodennummer die zugehörige Zeile der Tabelle 'Zinssätze'.
    /// </summary>
    /// <param name="periodenID">Periodennummer.</param>
    /// <returns>Zugehörige Zeile der Tabelle 'Zinssätze'.</returns>
    /// <exception cref="Exception">Wird bei nicht geladener Datenbank oder unbekannter Periodennummer ausgelöst.</exception>
    public static Zinssatz Zinssatz(int periodenID)
    {
        SicherstellenDassGeladen();
        var zinssatz = zinssätze!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
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

    /// <summary>
    /// Liefert zur angegebenen Periodennummer die zugehörige Zeile der Tabelle 'Kredite'.
    /// </summary>
    /// <param name="periodenID">Periodennummer.</param>
    /// <returns>Zugehörige Zeile der Tabelle 'Kredite'.</returns>
    /// <exception cref="Exception">Wird bei nicht geladener Datenbank oder unbekannter Periodennummer ausgelöst.</exception>
    public static Kredit Kredit(int periodenID)
    {
        SicherstellenDassGeladen();
        var kredit = kredite!.SingleOrDefault(eintrag => eintrag.PeriodenID == periodenID);
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
        private readonly string pfadZurDatenbank;

        public EasyBankingContext(string pfadZurDatenbank)
        {
            this.pfadZurDatenbank = pfadZurDatenbank;
        }

        public DbSet<Periode> Perioden => Set<Periode>();

        public DbSet<VolumenNeugeschäft> VoluminaNeugeschäft => Set<VolumenNeugeschäft>();

        public DbSet<Zinssatz> Zinssätze => Set<Zinssatz>();

        public DbSet<Kredit> Kredite => Set<Kredit>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={pfadZurDatenbank};";
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
