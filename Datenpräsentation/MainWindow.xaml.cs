using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using EasyBankingZinsüberschuss.Datenhaltung;
using EasyBankingZinsüberschuss.Datenverarbeitung;
using Microsoft.Win32;

namespace EasyBankingZinsüberschuss.Datenpräsentation
{
    /// <summary>
    /// Hauptfenster zur Auswahl der Datenbank und Darstellung des Brutto-Zinsüberschusses.
    /// Autor: TODO Name, Matrikelnummer: TODO
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Erstellt das Hauptfenster.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string beispielDatenbank = System.IO.Path.Combine(AppContext.BaseDirectory, "Beispielbank.accdb");
            if (System.IO.File.Exists(beispielDatenbank))
            {
                BerechnungAusDatenbankAnzeigen(beispielDatenbank);
            }
        }

        private void DatenbankAuswählen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Title = "Datenbank auswählen",
                Filter = "Access-Datenbank (*.accdb;*.mdb)|*.accdb;*.mdb|Alle Dateien (*.*)|*.*"
            };

            if (dialog.ShowDialog(this) == true)
            {
                BerechnungAusDatenbankAnzeigen(dialog.FileName);
            }
        }

        private void BerechnungAusDatenbankAnzeigen(string pfadZurDatenbank)
        {
            try
            {
                Datenbank.DatenbankAuslesen(pfadZurDatenbank);
                int[] periodenIDs = Datenbank.PeriodenIDs
                    .Select(id => Datenbank.Periode(id))
                    .OrderBy(periode => periode.Ende)
                    .Select(periode => periode.ID)
                    .ToArray();

                if (periodenIDs.Length < 5)
                {
                    throw new Exception("Für die Berechnung werden mindestens fünf Perioden benötigt.");
                }

                int aktuellePeriodeID = periodenIDs[^1];
                Zinsüberschuss ergebnis = new Zinsüberschuss(
                    Datenbank.VolumenNeugeschäft(aktuellePeriodeID),
                    Datenbank.VolumenNeugeschäft(periodenIDs[^2]),
                    Datenbank.VolumenNeugeschäft(periodenIDs[^3]),
                    Datenbank.VolumenNeugeschäft(periodenIDs[^4]),
                    Datenbank.VolumenNeugeschäft(periodenIDs[^5]),
                    Datenbank.Zinssatz(aktuellePeriodeID),
                    Datenbank.Zinssatz(periodenIDs[^2]),
                    Datenbank.Zinssatz(periodenIDs[^3]),
                    Datenbank.Zinssatz(periodenIDs[^4]),
                    Datenbank.Zinssatz(periodenIDs[^5]),
                    Datenbank.Kredit(aktuellePeriodeID));

                DatenbankPfadTextBlock.Text = pfadZurDatenbank;
                PeriodeTextBox.Text = Datenbank.Periode(aktuellePeriodeID).ToString();
                WerteAnzeigen(ergebnis);
                StatusTextBlock.Text = "Datenbank wurde geladen und berechnet.";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Fehler beim Laden der Datenbank.";
                MessageBox.Show(this, ex.Message, "EasyBanking", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WerteAnzeigen(Zinsüberschuss ergebnis)
        {
            ZinsKonsumkreditTextBox.Text = Betrag(ergebnis.ZinsKonsumkredit);
            ZinsAutokreditTextBox.Text = Betrag(ergebnis.ZinsAutokredit);
            ZinsHypothekenkreditTextBox.Text = Betrag(ergebnis.ZinsHypothekenkredit);
            ForderungenAnKundenTextBox.Text = Betrag(ergebnis.ForderungenAnKunden);
            SonstigeForderungenAnKreditinstituteTextBox.Text = Betrag(ergebnis.SonstigeForderungenAnKreditinstitute);
            ZinsertragTextBox.Text = Betrag(ergebnis.Zinsertrag);

            ZinsGirokontoTextBox.Text = Betrag(ergebnis.ZinsGirokonto);
            ZinsSpareinlageTextBox.Text = Betrag(ergebnis.ZinsSpareinlage);
            ZinsTermingeldTextBox.Text = Betrag(ergebnis.ZinsTermingeld);
            VerbindlichkeitenGegenüberKundenTextBox.Text = Betrag(ergebnis.VerbindlichkeitenGegenüberKunden);
            ÜberziehungskreditTextBox.Text = Betrag(ergebnis.Überziehungskredit);
            VerbindlichkeitenGegenüberKreditinstitutenTextBox.Text = Betrag(ergebnis.VerbindlichkeitenGegenüberKreditinstituten);
            SonstigesTextBox.Text = Betrag(ergebnis.Sonstiges);
            ZinsaufwandTextBox.Text = Betrag(ergebnis.Zinsaufwand);
            ZinsüberschussBruttoTextBox.Text = Betrag(ergebnis.ZinsüberschussBrutto);
        }

        private static string Betrag(decimal wert)
        {
            return wert.ToString("N2", CultureInfo.GetCultureInfo("de-DE")) + " EUR";
        }
    }
}
