using Academy.Week4.Ticketing.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core
{
    public class MainBusinessLayer : IBusinessLayer
    {
        private readonly IRepositoryTicket repositoryTicket;

        public MainBusinessLayer(IRepositoryTicket repoTicket)
        {
            repositoryTicket = repoTicket;
        }





        public bool CreateNewTicket(string? utente, string? descrizione)
        {
            Ticket newTicket = new Ticket();

            newTicket.Descrizione = descrizione;
            newTicket.Utente = utente;

            newTicket.Data = DateTime.Now;
            newTicket.Stato = Ticket.StatoEnum.New;

            return repositoryTicket.Add(newTicket);
        }

        public bool DeleteTicket(int id)
        {

            Ticket ticket = repositoryTicket.GetAll().Where(t => t.ID == id).SingleOrDefault();
            if (ticket == null)
            {
                return false;
            }
            else
            {
                return repositoryTicket.Delete(ticket);

            }
        }

        public List<Ticket> GetAllByMostRecent()
        {
            return repositoryTicket.GetAll().OrderByDescending(t => t.Data).ToList();
        }
    }
}
