// See https://aka.ms/new-console-template for more information


//Creare un nuovo database Ticketing con una sola tabella Tickets. Le colonne sono:
//ID(int, PK, auto - incrementale)
//Descrizione(varchar(500))
//Data(datetime)
//Utente(varchar(100))
//Stato(varchar(10)) – New, OnGoing, Resolved

using Academy.Week4.EsTicketing;

bool quit = false;
do
{
    Console.WriteLine("[1] Stampa la lista dei Ticket in ordine cronologico");
    Console.WriteLine("[2] Inserisci nuovo Ticket");
    Console.WriteLine("[3] Cancella Ticket");
    Console.WriteLine("[0] Chiudi");

    int choice = GetMenuChoice(0, 3);

    switch (choice)
    {
        case 1:
            StampaLista();
            break;
        case 2:
            InserisciNuovoTicket();
            break;
        case 3:
            CancellaTicket();
            break;
        case 0:
            quit = true;
            break;

    }
} while (!quit);

void CancellaTicket()
{
    Console.WriteLine("I Ticket presenti sono:");
    StampaLista();
    Console.WriteLine("Inserisci l'ID del ticket da eliminare:");
    int id = GetInt();

    bool isDeleted = BusinessLayer.DeleteTicket(id);
}

int GetInt()
{
    int choice;
    bool parse;
    do
    {
        parse = int.TryParse(Console.ReadLine(), out choice);
    } while (!parse);
    return choice;
}

void InserisciNuovoTicket()
{
    Console.WriteLine("Inserisci il nome Utente:");
    string utente = Console.ReadLine();
    Console.WriteLine("Inserisci la descrizione per il nuovo Ticket:");
    string descrizione = Console.ReadLine();

    bool isAdded = BusinessLayer.CreateNewTicket(utente, descrizione);
}

void StampaLista()
{

    List<Ticket> allTickets = BusinessLayer.GetAllByMostRecent();

    if (allTickets != null)
    {
        if (allTickets.Count != 0)
        {
            foreach (Ticket ticket in allTickets)
            {
                Console.WriteLine(ticket.ToString());
            }
        }
        else
            Console.WriteLine("--- Nessun Ticket ---");
    }
    


}

int GetMenuChoice(int min, int max)
{
    int choice;
    bool parse;
    do
    {
        parse = int.TryParse(Console.ReadLine(), out choice);
    } while (!parse || choice < 0 || choice > max);
    return choice;
}







