namespace EasyBankingZinsüberschuss.Datenhaltung;

/// <summary>
/// Entitäts-/Transferklasse für die Zinssätze einer Periode bezüglich der sechs Bankprodukte.
/// Autor: TODO Name, Matrikelnummer: TODO
/// </summary>
public class Zinssatz
{
    /// <summary>
    /// Leerer Konstruktor.
    /// </summary>
    public Zinssatz()
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
    /// Zinssatz für Konsumkredite.
    /// </summary>
    public double Konsumkredite { get; set; }

    /// <summary>
    /// Zinssatz für Autokredite.
    /// </summary>
    public double Autokredite { get; set; }

    /// <summary>
    /// Zinssatz für Hypothekenkredite.
    /// </summary>
    public double Hypothekenkredite { get; set; }

    /// <summary>
    /// Zinssatz für Girokonten.
    /// </summary>
    public double Girokonten { get; set; }

    /// <summary>
    /// Zinssatz für Spareinlagen.
    /// </summary>
    public double Spareinlagen { get; set; }

    /// <summary>
    /// Zinssatz für Termingelder.
    /// </summary>
    public double Termingelder { get; set; }
}
