namespace EasyBankingZinsüberschuss.Datenhaltung;

/// <summary>
/// Entitäts-/Transferklasse mit Angaben der Verbindlichkeiten und Forderungen der Bank einer Periode.
/// Autor: TODO Name, Matrikelnummer: TODO
/// </summary>
public class Kredit
{
    /// <summary>
    /// Leerer Konstruktor.
    /// </summary>
    public Kredit()
    {
    }

    /// <summary>
    /// ID der Entität.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// ID der zugehörigen Periode.
    /// </summary>
    public int PeriodenID { get; set; }

    /// <summary>
    /// Höhe des Überziehungskredits dieser Periode.
    /// </summary>
    public decimal Überziehungskredit { get; set; }

    /// <summary>
    /// Höhe der Verbindlichkeiten gegenüber anderen Banken in dieser Periode.
    /// </summary>
    public decimal Verbindlichkeiten { get; set; }

    /// <summary>
    /// Höhe der Forderungen an andere Banken in dieser Periode.
    /// </summary>
    public decimal Forderungen { get; set; }
}
