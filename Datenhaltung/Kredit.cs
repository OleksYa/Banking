namespace EasyBankingZinsüberschuss.Datenhaltung
{
    /// <summary>
    /// Entitäts-/Transferklasse mit Angaben der Verbindlichkeiten und Forderungen der Bank einer Periode.
    /// Autor: TODO Name, Matrikelnummer: TODO
    /// </summary>
    public class Kredit
    {
        private int _id;
        private int _periodenID;
        private decimal _überziehungskredit;
        private decimal _verbindlichkeiten;
        private decimal _forderungen;

        /// <summary>
        /// ID der Entität.
        /// </summary>
        public int ID { get { return _id; } internal set { _id = value; } }

        /// <summary>
        /// ID der zugehörigen Periode.
        /// </summary>
        public int PeriodenID { get { return _periodenID; } internal set { _periodenID = value; } }

        /// <summary>
        /// Höhe des Überziehungskredits dieser Periode.
        /// </summary>
        public decimal Überziehungskredit { get { return _überziehungskredit; } set { _überziehungskredit = value; } }

        /// <summary>
        /// Höhe der Verbindlichkeiten gegenüber anderen Banken in dieser Periode.
        /// </summary>
        public decimal Verbindlichkeiten { get { return _verbindlichkeiten; } set { _verbindlichkeiten = value; } }

        /// <summary>
        /// Höhe der Forderungen an andere Banken in dieser Periode.
        /// </summary>
        public decimal Forderungen { get { return _forderungen; } set { _forderungen = value; } }

        /// <summary>
        /// Leerer Konstruktor.
        /// </summary>
        public Kredit()
        {
        }
    }
}
