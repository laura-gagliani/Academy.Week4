using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.Ticketing.Core
{
    public interface IBusinessLayer
    {
        bool DeleteTicket(int id);
        bool CreateNewTicket(string? utente, string? descrizione);
        List<Ticket> GetAllByMostRecent();
    }
}
