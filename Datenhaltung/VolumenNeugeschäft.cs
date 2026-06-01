namespace EasyBankingZinsüberschuss.Datenhaltung;

/// <summary>
/// Entitäts-/Transferklasse für die Volumina des Neugeschäfts einer Periode bezüglich der sechs Bankprodukte.
/// Autor: TODO Name, Matrikelnummer: TODO
/// </summary>
public class VolumenNeugeschäft
{
    /// <summary>
    /// Leerer Konstruktor.
    /// </summary>
    public VolumenNeugeschäft()
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
    /// Volumen Neugeschäft für Konsumkredite.
    /// </summary>
    public decimal Konsumkredite { get; set; }

    /// <summary>
    /// Volumen Neugeschäft für Autokredite.
    /// </summary>
    public decimal Autokredite { get; set; }

    /// <summary>
    /// Volumen Neugeschäft für Hypothekenkredite.
    /// </summary>
    public decimal Hypothekenkredite { get; set; }

    /// <summary>
    /// Volumen Neugeschäft für Girokonten.
    /// </summary>
    public decimal Girokonten { get; set; }

    /// <summary>
    /// Volumen Neugeschäft für Spareinlagen.
    /// </summary>
    public decimal Spareinlagen { get; set; }

    /// <summary>
    /// Volumen Neugeschäft für Termingelder.
    /// </summary>
    public decimal Termingelder { get; set; }
}
