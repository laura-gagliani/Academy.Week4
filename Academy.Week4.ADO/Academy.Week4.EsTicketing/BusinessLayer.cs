using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.EsTicketing
{
    internal class BusinessLayer
    {
        internal static List<Ticket> GetAllByMostRecent()
        {
            return TicketingADO.GetAll("order by Data desc");

        }

        internal static bool CreateNewTicket(string? utente, string? descrizione)
        {
            Ticket newTicket = new Ticket();

            newTicket.Descrizione = descrizione;
            newTicket.Utente = utente;

            newTicket.Data = DateTime.Now;
            newTicket.Stato = Ticket.StatoEnum.New;

            return TicketingADO.Add(newTicket);

        }

        internal static bool DeleteTicket(int id)
        {
            return TicketingADO.Delete(id);
        }
    }
}
