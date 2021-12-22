using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.EntityFramework.Es1.Entities
{
    public class Impiegato
    {
        public int ImpiegatoID { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataDiNascita { get; set; }


        public int AziendaID { get; set; }
        public Azienda Azienda { get; set; }

    }
}
