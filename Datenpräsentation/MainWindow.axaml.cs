using System;
using System.Globalization;
using System.IO;
using Avalonia.Controls;
using EasyBankingZinsüberschuss.Datenhaltung;
using EasyBankingZinsüberschuss.Datenverarbeitung;

namespace EasyBankingZinsüberschuss.Datenpräsentation
{
    public partial class MainWindow : Window
    {
        private readonly CultureInfo _kultur;
        private ComboBox _periodenAuswahl = null!;
        private TextBlock _zinsertragText = null!;
        private TextBlock _zinsaufwandText = null!;
        private TextBlock _zinsueberschussText = null!;
        private TextBlock _zinsKonsumkreditText = null!;
        private TextBlock _zinsAutokreditText = null!;
        private TextBlock _zinsHypothekenkreditText = null!;
        private TextBlock _forderungenText = null!;
        private TextBlock _zinsertragDetailText = null!;
        private TextBlock _zinsGirokontoText = null!;
        private TextBlock _zinsSpareinlageText = null!;
        private TextBlock _zinsTermingeldText = null!;
        private TextBlock _sonstigesText = null!;
        private TextBlock _zinsaufwandDetailText = null!;
        private TextBlock _statusText = null!;

        public MainWindow()
        {
            _kultur = CultureInfo.GetCultureInfo("de-DE");

            InitializeComponent();
            SteuerelementeVerbinden();
            DatenLaden();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }

        private void SteuerelementeVerbinden()
        {
            _periodenAuswahl = this.FindControl<ComboBox>("PeriodenAuswahl")!;
            _zinsertragText = this.FindControl<TextBlock>("ZinsertragText")!;
            _zinsaufwandText = this.FindControl<TextBlock>("ZinsaufwandText")!;
            _zinsueberschussText = this.FindControl<TextBlock>("ZinsueberschussText")!;
            _zinsKonsumkreditText = this.FindControl<TextBlock>("ZinsKonsumkreditText")!;
            _zinsAutokreditText = this.FindControl<TextBlock>("ZinsAutokreditText")!;
            _zinsHypothekenkreditText = this.FindControl<TextBlock>("ZinsHypothekenkreditText")!;
            _forderungenText = this.FindControl<TextBlock>("ForderungenText")!;
            _zinsertragDetailText = this.FindControl<TextBlock>("ZinsertragDetailText")!;
            _zinsGirokontoText = this.FindControl<TextBlock>("ZinsGirokontoText")!;
            _zinsSpareinlageText = this.FindControl<TextBlock>("ZinsSpareinlageText")!;
            _zinsTermingeldText = this.FindControl<TextBlock>("ZinsTermingeldText")!;
            _sonstigesText = this.FindControl<TextBlock>("SonstigesText")!;
            _zinsaufwandDetailText = this.FindControl<TextBlock>("ZinsaufwandDetailText")!;
            _statusText = this.FindControl<TextBlock>("StatusText")!;

            _periodenAuswahl.SelectionChanged += PeriodenAuswahlGeaendert;
        }

        private void DatenLaden()
        {
            try
            {
                Datenbank.DatenbankAuslesen(PfadZurDatenbank());
                int[] periodenIDs = Datenbank.PeriodenIDs;
                _periodenAuswahl.ItemsSource = periodenIDs;

                if (periodenIDs.Length > 0)
                {
                    _periodenAuswahl.SelectedItem = periodenIDs[periodenIDs.Length - 1];
                }
            }
            catch (Exception ex)
            {
                _statusText.Text = ex.Message;
            }
        }

        private string PfadZurDatenbank()
        {
            string ausgabePfad = Path.Combine(AppContext.BaseDirectory, "Datenbank", "Bank.sqlite");
            if (File.Exists(ausgabePfad))
            {
                return ausgabePfad;
            }

            return Path.Combine(Environment.CurrentDirectory, "Datenbank", "Bank.sqlite");
        }

        private void PeriodenAuswahlGeaendert(object? sender, SelectionChangedEventArgs e)
        {
            if (_periodenAuswahl.SelectedItem is int periodenID)
            {
                ErgebnisAnzeigen(periodenID);
            }
        }

        private void ErgebnisAnzeigen(int periodenID)
        {
            try
            {
                int[] periodenIDs = Datenbank.PeriodenIDs;
                int index = Array.IndexOf(periodenIDs, periodenID);
                if (index < 4)
                {
                    throw new Exception("Für die Berechnung werden die aktuelle und vier vorherige Perioden benötigt.");
                }

                int periodeAP = periodenIDs[index];
                int periodeVP = periodenIDs[index - 1];
                int periodeVVP = periodenIDs[index - 2];
                int periodeVVVP = periodenIDs[index - 3];
                int periodeVVVVP = periodenIDs[index - 4];

                Zinsüberschuss ergebnis = new Zinsüberschuss(
                    Datenbank.VolumenNeugeschäft(periodeAP),
                    Datenbank.VolumenNeugeschäft(periodeVP),
                    Datenbank.VolumenNeugeschäft(periodeVVP),
                    Datenbank.VolumenNeugeschäft(periodeVVVP),
                    Datenbank.VolumenNeugeschäft(periodeVVVVP),
                    Datenbank.Zinssatz(periodeAP),
                    Datenbank.Zinssatz(periodeVP),
                    Datenbank.Zinssatz(periodeVVP),
                    Datenbank.Zinssatz(periodeVVVP),
                    Datenbank.Zinssatz(periodeVVVVP),
                    Datenbank.Kredit(periodeAP));

                WerteEintragen(ergebnis);
                Periode periode = Datenbank.Periode(periodeAP);
                _statusText.Text = "Geladene Datenbank: " + PfadZurDatenbank()
                    + " | Ausgewählte Periode: " + periode.Beginn.ToString("d", _kultur)
                    + " bis " + periode.Ende.ToString("d", _kultur);
            }
            catch (Exception ex)
            {
                _statusText.Text = ex.Message;
            }
        }

        private void WerteEintragen(Zinsüberschuss ergebnis)
        {
            _zinsertragText.Text = Betrag(ergebnis.Zinsertrag);
            _zinsaufwandText.Text = Betrag(ergebnis.Zinsaufwand);
            _zinsueberschussText.Text = Betrag(ergebnis.ZinsüberschussBrutto);
            _zinsKonsumkreditText.Text = Betrag(ergebnis.ZinsKonsumkredit);
            _zinsAutokreditText.Text = Betrag(ergebnis.ZinsAutokredit);
            _zinsHypothekenkreditText.Text = Betrag(ergebnis.ZinsHypothekenkredit);
            _forderungenText.Text = Betrag(ergebnis.SonstigeForderungenAnKreditinstitute);
            _zinsertragDetailText.Text = Betrag(ergebnis.Zinsertrag);
            _zinsGirokontoText.Text = Betrag(ergebnis.ZinsGirokonto);
            _zinsSpareinlageText.Text = Betrag(ergebnis.ZinsSpareinlage);
            _zinsTermingeldText.Text = Betrag(ergebnis.ZinsTermingeld);
            _sonstigesText.Text = Betrag(ergebnis.Sonstiges);
            _zinsaufwandDetailText.Text = Betrag(ergebnis.Zinsaufwand);
        }

        private string Betrag(decimal wert)
        {
            return wert.ToString("C2", _kultur);
        }
    }
}
