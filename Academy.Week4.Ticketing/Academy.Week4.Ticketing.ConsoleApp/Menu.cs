using Academy.Week4.Ticketing.Core;
using Academy.Week4.Ticketing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.ConsoleApp
{
    static internal class Menu
    {
        private static readonly IBusinessLayer bl = new MainBusinessLayer(new DisconnectedModeRepositoryTicket());

        internal static void Start()
        {
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
        }

        private static void CancellaTicket()
        {
            Console.WriteLine("I Ticket presenti sono:");
            StampaLista();
            Console.WriteLine("\nInserisci l'ID del ticket da eliminare:");
            int id = GetInt();
            bool isDeleted = bl.DeleteTicket(id);

            if (isDeleted)
                Console.WriteLine("\nTicket correttamente eliminato!");
            else
                Console.WriteLine("\nQualcosa è andato storto :(");
        }

        private static int GetInt()
        {
            int choice;
            bool parse;
            do
            {
                parse = int.TryParse(Console.ReadLine(), out choice);
            } while (!parse);
            return choice;
        }

        private static void InserisciNuovoTicket()
        {
            Console.WriteLine("Inserisci il nome Utente:");
            string utente = Console.ReadLine();
            Console.WriteLine("Inserisci la descrizione per il nuovo Ticket:");
            string descrizione = Console.ReadLine();

            bool isAdded = bl.CreateNewTicket(utente, descrizione);

            if (isAdded)
                Console.WriteLine("\nTicket correttamente aggiunto in elenco!");
            else
                Console.WriteLine("\nQualcosa è andato storto :(");
        }

        private static void StampaLista()
        {
            List<Ticket> allTickets = bl.GetAllByMostRecent();

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

        private static int GetMenuChoice(int min, int max)
        {
            int choice;
            bool parse;
            do
            {
                parse = int.TryParse(Console.ReadLine(), out choice);
            } while (!parse || choice < min || choice > max);
            return choice;
        }
    }


}

