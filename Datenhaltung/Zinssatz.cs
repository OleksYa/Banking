namespace EasyBankingZinsüberschuss.Datenhaltung
{
    /// <summary>
    /// Entitäts-/Transferklasse für die Zinssätze einer Periode bezüglich der sechs Bankprodukte.
    /// Autor: TODO Name, Matrikelnummer: TODO
    /// </summary>
    public class Zinssatz
    {
        private int _id;
        private int _periodenID;
        private double _konsumkredite;
        private double _autokredite;
        private double _hypothekenkredite;
        private double _girokonten;
        private double _spareinlagen;
        private double _termingelder;

        /// <summary>
        /// ID der Entität.
        /// </summary>
        public int ID { get { return _id; } internal set { _id = value; } }

        /// <summary>
        /// ID der zugehörigen Periode.
        /// </summary>
        public int PeriodenID { get { return _periodenID; } internal set { _periodenID = value; } }

        /// <summary>
        /// Zinssatz für Konsumkredite.
        /// </summary>
        public double Konsumkredite { get { return _konsumkredite; } internal set { _konsumkredite = value; } }

        /// <summary>
        /// Zinssatz für Autokredite.
        /// </summary>
        public double Autokredite { get { return _autokredite; } internal set { _autokredite = value; } }

        /// <summary>
        /// Zinssatz für Hypothekenkredite.
        /// </summary>
        public double Hypothekenkredite { get { return _hypothekenkredite; } internal set { _hypothekenkredite = value; } }

        /// <summary>
        /// Zinssatz für Girokonten.
        /// </summary>
        public double Girokonten { get { return _girokonten; } internal set { _girokonten = value; } }

        /// <summary>
        /// Zinssatz für Spareinlagen.
        /// </summary>
        public double Spareinlagen { get { return _spareinlagen; } internal set { _spareinlagen = value; } }

        /// <summary>
        /// Zinssatz für Termingelder.
        /// </summary>
        public double Termingelder { get { return _termingelder; } internal set { _termingelder = value; } }

        /// <summary>
        /// Leerer Konstruktor.
        /// </summary>
        public Zinssatz()
        {
        }
    }
}
