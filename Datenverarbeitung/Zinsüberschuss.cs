using System;
using EasyBankingZinsüberschuss.Datenhaltung;

namespace EasyBankingZinsüberschuss.Datenverarbeitung
{
    /// <summary>
    /// Klasse für Berechnung Zinsüberschuss brutto mit Zwischenschritten.
    /// Autor: TODO Name, Matrikelnummer: TODO
    /// </summary>
    public class Zinsüberschuss
    {
        private decimal _zinsKonsumkredit;
        private decimal _zinsAutokredit;
        private decimal _zinsHypothekenkredit;
        private decimal _forderungenAnKunden;
        private decimal _sonstigeForderungenAnKreditinstitute;
        private decimal _zinsertrag;
        private decimal _zinsGirokonto;
        private decimal _zinsSpareinlage;
        private decimal _zinsTermingeld;
        private decimal _verbindlichkeitenGegenüberKunden;
        private decimal _überziehungskredit;
        private decimal _verbindlichkeitenGegenüberKreditinstituten;
        private decimal _sonstiges;
        private decimal _zinsaufwand;
        private decimal _zinsüberschussBrutto;

        /// <summary>
        /// Zins aus Konsumkrediten.
        /// </summary>
        public decimal ZinsKonsumkredit { get { return _zinsKonsumkredit; } }

        /// <summary>
        /// Zins aus Autokrediten.
        /// </summary>
        public decimal ZinsAutokredit { get { return _zinsAutokredit; } }

        /// <summary>
        /// Zins aus Hypothekenkrediten.
        /// </summary>
        public decimal ZinsHypothekenkredit { get { return _zinsHypothekenkredit; } }

        /// <summary>
        /// Forderungen an Kunden.
        /// </summary>
        public decimal ForderungenAnKunden { get { return _forderungenAnKunden; } }

        /// <summary>
        /// Sonstige Forderungen an Kreditinstitute.
        /// </summary>
        public decimal SonstigeForderungenAnKreditinstitute { get { return _sonstigeForderungenAnKreditinstitute; } }

        /// <summary>
        /// Zinsertrag.
        /// </summary>
        public decimal Zinsertrag { get { return _zinsertrag; } }

        /// <summary>
        /// Zins Girokonten.
        /// </summary>
        public decimal ZinsGirokonto { get { return _zinsGirokonto; } }

        /// <summary>
        /// Zins Spareinlage.
        /// </summary>
        public decimal ZinsSpareinlage { get { return _zinsSpareinlage; } }

        /// <summary>
        /// Zins Termingeld.
        /// </summary>
        public decimal ZinsTermingeld { get { return _zinsTermingeld; } }

        /// <summary>
        /// Verbindlichkeiten gegenüber Kunden.
        /// </summary>
        public decimal VerbindlichkeitenGegenüberKunden { get { return _verbindlichkeitenGegenüberKunden; } }

        /// <summary>
        /// Überziehungskredit.
        /// </summary>
        public decimal Überziehungskredit { get { return _überziehungskredit; } }

        /// <summary>
        /// Verbindlichkeiten gegenüber Kreditinstituten.
        /// </summary>
        public decimal VerbindlichkeitenGegenüberKreditinstituten { get { return _verbindlichkeitenGegenüberKreditinstituten; } }

        /// <summary>
        /// Sonstiges.
        /// </summary>
        public decimal Sonstiges { get { return _sonstiges; } }

        /// <summary>
        /// Zinsaufwand.
        /// </summary>
        public decimal Zinsaufwand { get { return _zinsaufwand; } }

        /// <summary>
        /// Zinsüberschuss brutto.
        /// </summary>
        public decimal ZinsüberschussBrutto { get { return _zinsüberschussBrutto; } }

        /// <summary>
        /// Konstruktor / Berechnung Zinsüberschuss brutto mit Zwischenschritten.
        /// </summary>
        public Zinsüberschuss(
            VolumenNeugeschäft volumenNeugeschäftAP,
            VolumenNeugeschäft volumenNeugeschäftVP,
            VolumenNeugeschäft volumenNeugeschäftVVP,
            VolumenNeugeschäft volumenNeugeschäftVVVP,
            VolumenNeugeschäft volumenNeugeschäftVVVVP,
            Zinssatz zinssatzAP,
            Zinssatz zinssatzVP,
            Zinssatz zinssatzVVP,
            Zinssatz zinssatzVVVP,
            Zinssatz zinssatzVVVVP,
            Kredit kreditAP)
        {
            ArgumentNullException.ThrowIfNull(volumenNeugeschäftAP);
            ArgumentNullException.ThrowIfNull(volumenNeugeschäftVP);
            ArgumentNullException.ThrowIfNull(volumenNeugeschäftVVP);
            ArgumentNullException.ThrowIfNull(volumenNeugeschäftVVVP);
            ArgumentNullException.ThrowIfNull(volumenNeugeschäftVVVVP);
            ArgumentNullException.ThrowIfNull(zinssatzAP);
            ArgumentNullException.ThrowIfNull(zinssatzVP);
            ArgumentNullException.ThrowIfNull(zinssatzVVP);
            ArgumentNullException.ThrowIfNull(zinssatzVVVP);
            ArgumentNullException.ThrowIfNull(zinssatzVVVVP);
            ArgumentNullException.ThrowIfNull(kreditAP);

            _zinsKonsumkredit = volumenNeugeschäftAP.Konsumkredite * ZinssatzAlsFaktor(zinssatzAP.Konsumkredite);
            _zinsAutokredit =
                volumenNeugeschäftVP.Autokredite * 0.5m * ZinssatzAlsFaktor(zinssatzVP.Autokredite)
                + volumenNeugeschäftAP.Autokredite * ZinssatzAlsFaktor(zinssatzAP.Autokredite);
            _zinsHypothekenkredit =
                volumenNeugeschäftVVVVP.Hypothekenkredite * 0.2m * ZinssatzAlsFaktor(zinssatzVVVVP.Hypothekenkredite)
                + volumenNeugeschäftVVVP.Hypothekenkredite * 0.4m * ZinssatzAlsFaktor(zinssatzVVVP.Hypothekenkredite)
                + volumenNeugeschäftVVP.Hypothekenkredite * 0.6m * ZinssatzAlsFaktor(zinssatzVVP.Hypothekenkredite)
                + volumenNeugeschäftVP.Hypothekenkredite * 0.8m * ZinssatzAlsFaktor(zinssatzVP.Hypothekenkredite)
                + volumenNeugeschäftAP.Hypothekenkredite * ZinssatzAlsFaktor(zinssatzAP.Hypothekenkredite);
            _forderungenAnKunden = ZinsKonsumkredit + ZinsAutokredit + ZinsHypothekenkredit;
            _sonstigeForderungenAnKreditinstitute = kreditAP.Forderungen;
            _zinsertrag = ForderungenAnKunden + SonstigeForderungenAnKreditinstitute;

            _zinsGirokonto = volumenNeugeschäftAP.Girokonten * ZinssatzAlsFaktor(zinssatzAP.Girokonten);
            _zinsSpareinlage = volumenNeugeschäftAP.Spareinlagen * ZinssatzAlsFaktor(zinssatzAP.Spareinlagen);
            decimal zinssatzTermingeldVP = ZinssatzAlsFaktor(zinssatzVP.Termingelder);
            _zinsTermingeld =
                volumenNeugeschäftVP.Termingelder * (1m + zinssatzTermingeldVP) * zinssatzTermingeldVP
                + volumenNeugeschäftAP.Termingelder * ZinssatzAlsFaktor(zinssatzAP.Termingelder);
            _verbindlichkeitenGegenüberKunden = ZinsGirokonto + ZinsSpareinlage + ZinsTermingeld;
            _überziehungskredit = kreditAP.Überziehungskredit;
            _verbindlichkeitenGegenüberKreditinstituten = kreditAP.Verbindlichkeiten;
            _sonstiges = Überziehungskredit + VerbindlichkeitenGegenüberKreditinstituten;
            _zinsaufwand = VerbindlichkeitenGegenüberKunden + Sonstiges;
            _zinsüberschussBrutto = Zinsertrag - Zinsaufwand;
        }

        private static decimal ZinssatzAlsFaktor(double zinssatz)
        {
            decimal wert = Convert.ToDecimal(zinssatz);
            return Math.Abs(wert) > 1m ? wert / 100m : wert;
        }
    }
}
