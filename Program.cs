using System;
using EasyBankingZinsüberschuss.Datenhaltung;
using EasyBankingZinsüberschuss.Datenverarbeitung;

namespace EasyBankingZinsüberschuss
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "--demo")
            {
                DemoAusführen();
                return;
            }

            Console.WriteLine("EasyBanking Zinsüberschuss");
            Console.WriteLine();
            Console.WriteLine("Dieser Konsolenmodus ist für macOS/Linux, damit das Projekt dort gebaut und ausgeführt werden kann.");
            Console.WriteLine("Die grafische WPF-Anwendung und das Lesen der Access-Datenbank benötigen Windows.");
            Console.WriteLine();
            Console.WriteLine("Demo ausführen:");
            Console.WriteLine("dotnet run -f net10.0 -- --demo");
        }

        private static void DemoAusführen()
        {
            VolumenNeugeschäft volumenAP = new VolumenNeugeschäft
            {
                Konsumkredite = 1_000_000_000m,
                Autokredite = 1_200_000_000m,
                Hypothekenkredite = 1_000_000_000m,
                Girokonten = 2_000_000_000m,
                Spareinlagen = 1_500_000_000m,
                Termingelder = 2_000_000_000m
            };

            VolumenNeugeschäft volumenVP = new VolumenNeugeschäft
            {
                Autokredite = 1_000_000_000m,
                Hypothekenkredite = 1_200_000_000m,
                Termingelder = 1_200_000_000m
            };

            VolumenNeugeschäft volumenVVP = new VolumenNeugeschäft
            {
                Hypothekenkredite = 1_100_000_000m
            };

            VolumenNeugeschäft volumenVVVP = new VolumenNeugeschäft
            {
                Hypothekenkredite = 900_000_000m
            };

            VolumenNeugeschäft volumenVVVVP = new VolumenNeugeschäft
            {
                Hypothekenkredite = 1_000_000_000m
            };

            Zinssatz zinssatzAP = new Zinssatz
            {
                Konsumkredite = 0.05,
                Autokredite = 0.045,
                Hypothekenkredite = 0.025,
                Girokonten = 0.001,
                Spareinlagen = 0.005,
                Termingelder = 0.008
            };

            Zinssatz zinssatzVP = new Zinssatz
            {
                Autokredite = 0.04,
                Hypothekenkredite = 0.025,
                Termingelder = 0.01
            };

            Zinssatz zinssatzVVP = new Zinssatz
            {
                Hypothekenkredite = 0.025
            };

            Zinssatz zinssatzVVVP = new Zinssatz
            {
                Hypothekenkredite = 0.03
            };

            Zinssatz zinssatzVVVVP = new Zinssatz
            {
                Hypothekenkredite = 0.03
            };

            Kredit kreditAP = new Kredit
            {
                Überziehungskredit = 0m,
                Verbindlichkeiten = 4_250_000m,
                Forderungen = 2_125_000m
            };

            Zinsüberschuss ergebnis = new Zinsüberschuss(
                volumenAP,
                volumenVP,
                volumenVVP,
                volumenVVVP,
                volumenVVVVP,
                zinssatzAP,
                zinssatzVP,
                zinssatzVVP,
                zinssatzVVVP,
                zinssatzVVVVP,
                kreditAP);

            Console.WriteLine("Zinsertrag: " + Betrag(ergebnis.Zinsertrag));
            Console.WriteLine("Zinsaufwand: " + Betrag(ergebnis.Zinsaufwand));
            Console.WriteLine("Zinsüberschuss brutto: " + Betrag(ergebnis.ZinsüberschussBrutto));
        }

        private static string Betrag(decimal wert)
        {
            return wert.ToString("N2") + " EUR";
        }
    }
}
