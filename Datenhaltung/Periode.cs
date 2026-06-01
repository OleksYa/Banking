using System;

namespace EasyBankingZinsüberschuss.Datenhaltung;

/// <summary>
/// Entitäts-/Transferklasse zur Aufnahme der Werte einer Zeile der Tabelle 'Perioden'.
/// Autor: TODO Name, Matrikelnummer: TODO
/// </summary>
public class Periode
{
    /// <summary>
    /// Leerer Konstruktor.
    /// </summary>
    public Periode()
    {
    }

    /// <summary>
    /// ID der Entität.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Spalte 'Beginn'.
    /// </summary>
    public DateTime Beginn { get; set; }

    /// <summary>
    /// Spalte 'Ende'.
    /// </summary>
    public DateTime Ende { get; set; }

    /// <summary>
    /// Erstellt eine druckbare Darstellung der Periode.
    /// </summary>
    /// <returns>Druckbare Darstellung.</returns>
    public override string ToString()
    {
        return $"{Beginn:d} - {Ende:d}";
    }
}
