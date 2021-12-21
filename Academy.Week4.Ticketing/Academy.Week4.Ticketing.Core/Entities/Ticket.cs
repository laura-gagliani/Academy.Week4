namespace Academy.Week4.Ticketing.Core
{
    public class Ticket
    {
        //Creare un nuovo database Ticketing con una sola tabella Tickets. Le colonne sono:
        //ID(int, PK, auto - incrementale)
        //Descrizione(varchar(500))
        //Data(datetime)
        //Utente(varchar(100))
        //Stato(varchar(10)) – New, OnGoing, Resolved
        public int ID { get; set; }
        public string Descrizione { get; set; }
        public DateTime Data { get; set; }
        public string Utente { get; set; }
        public StatoEnum Stato { get; set; }

        public enum StatoEnum
        {
            New = 1,
            OnGoing,
            Resolved
        }

        public override string ToString()
        {
            return $"ID: {ID} - Data: {Data.ToShortDateString()} - Stato: {Stato}- Utente: {Utente} - Descrizione: {Descrizione}";
        }
    }
}