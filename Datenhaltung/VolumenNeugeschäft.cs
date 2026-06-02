namespace EasyBankingZinsüberschuss.Datenhaltung
{
    /// <summary>
    /// Entitäts-/Transferklasse für die Volumina des Neugeschäfts einer Periode bezüglich der sechs Bankprodukte.
    /// Autor: TODO Name, Matrikelnummer: TODO
    /// </summary>
    public class VolumenNeugeschäft
    {
        private int _id;
        private int _periodenID;
        private decimal _konsumkredite;
        private decimal _autokredite;
        private decimal _hypothekenkredite;
        private decimal _girokonten;
        private decimal _spareinlagen;
        private decimal _termingelder;

        /// <summary>
        /// ID der Entität.
        /// </summary>
        public int ID { get { return _id; } internal set { _id = value; } }

        /// <summary>
        /// ID der zugehörigen Periode.
        /// </summary>
        public int PeriodenID { get { return _periodenID; } internal set { _periodenID = value; } }

        /// <summary>
        /// Volumen Neugeschäft für Konsumkredite.
        /// </summary>
        public decimal Konsumkredite { get { return _konsumkredite; } set { _konsumkredite = value; } }

        /// <summary>
        /// Volumen Neugeschäft für Autokredite.
        /// </summary>
        public decimal Autokredite { get { return _autokredite; } set { _autokredite = value; } }

        /// <summary>
        /// Volumen Neugeschäft für Hypothekenkredite.
        /// </summary>
        public decimal Hypothekenkredite { get { return _hypothekenkredite; } set { _hypothekenkredite = value; } }

        /// <summary>
        /// Volumen Neugeschäft für Girokonten.
        /// </summary>
        public decimal Girokonten { get { return _girokonten; } set { _girokonten = value; } }

        /// <summary>
        /// Volumen Neugeschäft für Spareinlagen.
        /// </summary>
        public decimal Spareinlagen { get { return _spareinlagen; } set { _spareinlagen = value; } }

        /// <summary>
        /// Volumen Neugeschäft für Termingelder.
        /// </summary>
        public decimal Termingelder { get { return _termingelder; } set { _termingelder = value; } }

        /// <summary>
        /// Leerer Konstruktor.
        /// </summary>
        public VolumenNeugeschäft()
        {
        }
    }
}
