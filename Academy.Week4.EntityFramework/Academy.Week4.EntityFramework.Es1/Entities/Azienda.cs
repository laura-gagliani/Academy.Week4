using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.EntityFramework.Es1.Entities
{
    public class Azienda
    {
        public int AziendaID { get; set; }

        [MaxLength(50)]
        public string Nome { get; set; }

        public int AnnoFondazione { get; set; }
        public List<Impiegato> Impiegati { get; set; } = new List<Impiegato>();
    }
}
