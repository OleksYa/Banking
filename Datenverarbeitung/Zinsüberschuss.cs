using System;
using EasyBankingZinsüberschuss.Datenhaltung;

namespace EasyBankingZinsüberschuss.Datenverarbeitung;

/// <summary>
/// Klasse für Berechnung Zinsüberschuss brutto mit Zwischenschritten.
/// Autor: TODO Name, Matrikelnummer: TODO
/// </summary>
public class Zinsüberschuss
{
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

        ZinsKonsumkredit = volumenNeugeschäftAP.Konsumkredite * ZinssatzAlsFaktor(zinssatzAP.Konsumkredite);
        ZinsAutokredit =
            volumenNeugeschäftVP.Autokredite * 0.5m * ZinssatzAlsFaktor(zinssatzVP.Autokredite)
            + volumenNeugeschäftAP.Autokredite * ZinssatzAlsFaktor(zinssatzAP.Autokredite);
        ZinsHypothekenkredit =
            volumenNeugeschäftVVVVP.Hypothekenkredite * 0.2m * ZinssatzAlsFaktor(zinssatzVVVVP.Hypothekenkredite)
            + volumenNeugeschäftVVVP.Hypothekenkredite * 0.4m * ZinssatzAlsFaktor(zinssatzVVVP.Hypothekenkredite)
            + volumenNeugeschäftVVP.Hypothekenkredite * 0.6m * ZinssatzAlsFaktor(zinssatzVVP.Hypothekenkredite)
            + volumenNeugeschäftVP.Hypothekenkredite * 0.8m * ZinssatzAlsFaktor(zinssatzVP.Hypothekenkredite)
            + volumenNeugeschäftAP.Hypothekenkredite * ZinssatzAlsFaktor(zinssatzAP.Hypothekenkredite);
        ForderungenAnKunden = ZinsKonsumkredit + ZinsAutokredit + ZinsHypothekenkredit;
        SonstigeForderungenAnKreditinstitute = kreditAP.Forderungen;
        Zinsertrag = ForderungenAnKunden + SonstigeForderungenAnKreditinstitute;

        ZinsGirokonto = volumenNeugeschäftAP.Girokonten * ZinssatzAlsFaktor(zinssatzAP.Girokonten);
        ZinsSpareinlage = volumenNeugeschäftAP.Spareinlagen * ZinssatzAlsFaktor(zinssatzAP.Spareinlagen);
        var zinssatzTermingeldVP = ZinssatzAlsFaktor(zinssatzVP.Termingelder);
        ZinsTermingeld =
            volumenNeugeschäftVP.Termingelder * (1m + zinssatzTermingeldVP) * zinssatzTermingeldVP
            + volumenNeugeschäftAP.Termingelder * ZinssatzAlsFaktor(zinssatzAP.Termingelder);
        VerbindlichkeitenGegenüberKunden = ZinsGirokonto + ZinsSpareinlage + ZinsTermingeld;
        Überziehungskredit = kreditAP.Überziehungskredit;
        VerbindlichkeitenGegenüberKreditinstituten = kreditAP.Verbindlichkeiten;
        Sonstiges = Überziehungskredit + VerbindlichkeitenGegenüberKreditinstituten;
        Zinsaufwand = VerbindlichkeitenGegenüberKunden + Sonstiges;
        ZinsüberschussBrutto = Zinsertrag - Zinsaufwand;
    }

    /// <summary>
    /// Zins aus Konsumkrediten.
    /// </summary>
    public decimal ZinsKonsumkredit { get; }

    /// <summary>
    /// Zins aus Autokrediten.
    /// </summary>
    public decimal ZinsAutokredit { get; }

    /// <summary>
    /// Zins aus Hypothekenkrediten.
    /// </summary>
    public decimal ZinsHypothekenkredit { get; }

    /// <summary>
    /// Forderungen an Kunden.
    /// </summary>
    public decimal ForderungenAnKunden { get; }

    /// <summary>
    /// Sonstige Forderungen an Kreditinstitute.
    /// </summary>
    public decimal SonstigeForderungenAnKreditinstitute { get; }

    /// <summary>
    /// Zinsertrag.
    /// </summary>
    public decimal Zinsertrag { get; }

    /// <summary>
    /// Zins Girokonten.
    /// </summary>
    public decimal ZinsGirokonto { get; }

    /// <summary>
    /// Zins Spareinlage.
    /// </summary>
    public decimal ZinsSpareinlage { get; }

    /// <summary>
    /// Zins Termingeld.
    /// </summary>
    public decimal ZinsTermingeld { get; }

    /// <summary>
    /// Verbindlichkeiten gegenüber Kunden.
    /// </summary>
    public decimal VerbindlichkeitenGegenüberKunden { get; }

    /// <summary>
    /// Überziehungskredit.
    /// </summary>
    public decimal Überziehungskredit { get; }

    /// <summary>
    /// Verbindlichkeiten gegenüber Kreditinstituten.
    /// </summary>
    public decimal VerbindlichkeitenGegenüberKreditinstituten { get; }

    /// <summary>
    /// Sonstiges.
    /// </summary>
    public decimal Sonstiges { get; }

    /// <summary>
    /// Zinsaufwand.
    /// </summary>
    public decimal Zinsaufwand { get; }

    /// <summary>
    /// Zinsüberschuss brutto.
    /// </summary>
    public decimal ZinsüberschussBrutto { get; }

    private static decimal ZinssatzAlsFaktor(double zinssatz)
    {
        var wert = Convert.ToDecimal(zinssatz);
        return Math.Abs(wert) > 1m ? wert / 100m : wert;
    }
}
