using System;
using System.IO;
using System.Linq;

namespace EasyBankingZinsüberschuss.Datenhaltung
{
    public static class Datenbank
    {
        private const decimal MillionenFaktor = 1_000_000.0M;

        private static bool _istGeladen;
        private static Periode[]? _perioden;
        private static Kredit[]? _kredite;
        private static VolumenNeugeschäft[]? _voluminaNeugeschäft;
        private static Zinssatz[]? _zinssätze;

        public static bool IstGeladen { get { return _istGeladen; } }

        public static int[] PeriodenIDs
        {
            get
            {
                SicherstellenDassGeladen();
                return _perioden!.Select(periode => periode.ID).ToArray();
            }
        }

        public static void DatenbankAuslesen(string pfadZurDatenbank)
        {
            if (String.IsNullOrWhiteSpace(pfadZurDatenbank) || !File.Exists(pfadZurDatenbank))
            {
                _istGeladen = false;
                throw new Exception("Der Pfad zur Datenbank existiert nicht.");
            }

            _perioden =
            [
                ErzeugtePeriode(1, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31)),
                ErzeugtePeriode(2, new DateTime(2020, 1, 1), new DateTime(2020, 12, 31)),
                ErzeugtePeriode(3, new DateTime(2021, 1, 1), new DateTime(2021, 12, 31)),
                ErzeugtePeriode(4, new DateTime(2022, 1, 1), new DateTime(2022, 12, 31)),
                ErzeugtePeriode(5, new DateTime(2023, 1, 1), new DateTime(2023, 12, 31)),
                ErzeugtePeriode(6, new DateTime(2024, 1, 1), new DateTime(2024, 12, 31)),
                ErzeugtePeriode(7, new DateTime(2025, 1, 1), new DateTime(2025, 12, 31))
            ];

            _kredite =
            [
                ErzeugterKredit(1, 1, 0.0M, 4.25M, 2.125M),
                ErzeugterKredit(2, 2, 0.0M, 4.00M, 2.000M),
                ErzeugterKredit(3, 3, 2.0M, 3.75M, 1.950M),
                ErzeugterKredit(4, 4, 0.0M, 2.90M, 1.500M),
                ErzeugterKredit(5, 5, 1.0M, 3.00M, 1.700M),
                ErzeugterKredit(6, 6, 0.0M, 3.70M, 2.000M),
                ErzeugterKredit(7, 7, 0.0M, 4.25M, 2.125M)
            ];

            _voluminaNeugeschäft =
            [
                ErzeugtesVolumenNeugeschäft(1, 1, 1350.0M, 1000.0M, 0900.0M, 1300.0M, 2200.0M, 1100.0M),
                ErzeugtesVolumenNeugeschäft(2, 2, 1500.0M, 1080.0M, 1000.0M, 1350.0M, 2400.0M, 1200.0M),
                ErzeugtesVolumenNeugeschäft(3, 3, 1400.0M, 1100.0M, 1000.0M, 1400.0M, 2220.0M, 1150.0M),
                ErzeugtesVolumenNeugeschäft(4, 4, 1350.0M, 1000.0M, 0900.0M, 1300.0M, 2200.0M, 1100.0M),
                ErzeugtesVolumenNeugeschäft(5, 5, 1500.0M, 1000.0M, 1100.0M, 1350.0M, 2400.0M, 1500.0M),
                ErzeugtesVolumenNeugeschäft(6, 6, 1000.0M, 1000.0M, 1200.0M, 1500.0M, 2000.0M, 1200.0M),
                ErzeugtesVolumenNeugeschäft(7, 7, 1000.0M, 1200.0M, 1000.0M, 2000.0M, 1500.0M, 2000.0M)
            ];

            _zinssätze =
            [
                ErzeugterZinssatz(1, 1, .060, .050, .035, .005, .010, .015),
                ErzeugterZinssatz(2, 2, .060, .050, .030, .004, .008, .013),
                ErzeugterZinssatz(3, 3, .055, .045, .030, .003, .007, .011),
                ErzeugterZinssatz(4, 4, .055, .045, .030, .001, .006, .010),
                ErzeugterZinssatz(5, 5, .050, .040, .025, .001, .005, .010),
                ErzeugterZinssatz(6, 6, .050, .040, .025, .001, .005, .010),
                ErzeugterZinssatz(7, 7, .050, .045, .025, .001, .005, .008)
            ];

            _istGeladen = true;
        }

        public static Periode Periode(int periodenID)
        {
            SicherstellenDassGeladen();
            Periode? periode = _perioden!.SingleOrDefault(eintrag => eintrag.ID == periodenID);
            if (periode == null)
            {
                throw new Exception("Die Periodennummer ist unbekannt.");
            }

            return ErzeugtePeriode(periode.ID, periode.Beginn, periode.Ende);
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
                Überziehungskredit = kredit.Überziehungskredit,
                Verbindlichkeiten = kredit.Verbindlichkeiten,
                Forderungen = kredit.Forderungen
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
                Konsumkredite = volumen.Konsumkredite,
                Autokredite = volumen.Autokredite,
                Hypothekenkredite = volumen.Hypothekenkredite,
                Girokonten = volumen.Girokonten,
                Spareinlagen = volumen.Spareinlagen,
                Termingelder = volumen.Termingelder
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

            return ErzeugterZinssatz(zinssatz.ID,
                                     zinssatz.PeriodenID,
                                     zinssatz.Konsumkredite,
                                     zinssatz.Autokredite,
                                     zinssatz.Hypothekenkredite,
                                     zinssatz.Girokonten,
                                     zinssatz.Spareinlagen,
                                     zinssatz.Termingelder);
        }

        private static void SicherstellenDassGeladen()
        {
            if (!IstGeladen)
            {
                throw new Exception("Es ist keine Datenbank geladen.");
            }
        }

        private static Periode ErzeugtePeriode(int id, DateTime beginn, DateTime ende)
        {
            return new Periode
            {
                ID = id,
                Beginn = beginn,
                Ende = ende
            };
        }

        private static Kredit ErzeugterKredit(int id, int periodenID, decimal überziehungskredit, decimal verbindlichkeiten, decimal forderungen)
        {
            return new Kredit
            {
                ID = id,
                PeriodenID = periodenID,
                Überziehungskredit = überziehungskredit * MillionenFaktor,
                Verbindlichkeiten = verbindlichkeiten * MillionenFaktor,
                Forderungen = forderungen * MillionenFaktor
            };
        }

        private static VolumenNeugeschäft ErzeugtesVolumenNeugeschäft(int id,
                                                                      int periodenID,
                                                                      decimal konsumkredite,
                                                                      decimal autokredite,
                                                                      decimal hypothekenkredite,
                                                                      decimal girokonten,
                                                                      decimal spareinlagen,
                                                                      decimal termingelder)
        {
            return new VolumenNeugeschäft
            {
                ID = id,
                PeriodenID = periodenID,
                Konsumkredite = konsumkredite * MillionenFaktor,
                Autokredite = autokredite * MillionenFaktor,
                Hypothekenkredite = hypothekenkredite * MillionenFaktor,
                Girokonten = girokonten * MillionenFaktor,
                Spareinlagen = spareinlagen * MillionenFaktor,
                Termingelder = termingelder * MillionenFaktor
            };
        }

        private static Zinssatz ErzeugterZinssatz(int id,
                                                  int periodenID,
                                                  double konsumkredite,
                                                  double autokredite,
                                                  double hypothekenkredite,
                                                  double girokonten,
                                                  double spareinlagen,
                                                  double termingelder)
        {
            return new Zinssatz
            {
                ID = id,
                PeriodenID = periodenID,
                Konsumkredite = konsumkredite,
                Autokredite = autokredite,
                Hypothekenkredite = hypothekenkredite,
                Girokonten = girokonten,
                Spareinlagen = spareinlagen,
                Termingelder = termingelder
            };
        }
    }
}
