using Academy.Week4.EntityFramework.Es1.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.EntityFramework.Es1.EF
{
    public class Context : DbContext
    {
        //questa classe mi farà da ponte! da "mappatura"
        // avrò un dbset per ogni entità/classe (in questo caso due)
        // avrò anche una stringa di connessione per dirgli dove fare sto db

        public DbSet<Impiegato> Impiegato { get; set; }
        public DbSet<Azienda> Azienda { get; set; }

        public Context() : base()
        {

        }
        public Context(DbContextOptions<Context> options) : base(options)   
        {
            //potremo mettergli come opzione la connection string
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionStringSQL = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EsempioAzienda;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                optionsBuilder.UseSqlServer(connectionStringSQL);

            }
        }
    }
}
