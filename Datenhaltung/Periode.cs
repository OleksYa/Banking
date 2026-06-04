using System;

namespace EasyBankingZinsüberschuss.Datenhaltung
{
    /// <summary>
    /// Entitäts-/Transferklasse zur Aufnahme der Werte einer Zeile der Tabelle 'Perioden'.
    /// Autor: Oleksandr Yampolskyi 106851
    /// </summary>
    public class Periode
    {
        private int _id;
        private DateTime _beginn;
        private DateTime _ende;

        /// <summary>
        /// ID der Entität.
        /// </summary>
        public int ID { get { return _id; } internal set { _id = value; } }

        /// <summary>
        /// Spalte 'Beginn'.
        /// </summary>
        public DateTime Beginn { get { return _beginn; } internal set { _beginn = value; } }

        /// <summary>
        /// Spalte 'Ende'.
        /// </summary>
        public DateTime Ende { get { return _ende; } internal set { _ende = value; } }

        /// <summary>
        /// Leerer Konstruktor.
        /// </summary>
        public Periode()
        {
        }

        /// <summary>
        /// Erstellt eine druckbare Darstellung der Periode.
        /// </summary>
        /// <returns>Druckbare Darstellung.</returns>
        public override string ToString()
        {
            return $"{Beginn:d} - {Ende:d}";
        }
    }
}
