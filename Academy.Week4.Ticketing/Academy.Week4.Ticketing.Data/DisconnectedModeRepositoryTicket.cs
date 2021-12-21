using Academy.Week4.Ticketing.Core;
using Academy.Week4.Ticketing.Core.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Academy.Week4.Ticketing.Core.Ticket;

namespace Academy.Week4.Ticketing.Data
{
    public class DisconnectedModeRepositoryTicket : IRepositoryTicket
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        private SqlDataAdapter InitializeDataAdapter(DataSet dataset, SqlConnection connection)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            //select
            adapter.SelectCommand = new SqlCommand("select * from Tickets", connection);
            adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;

            //insert
            adapter.InsertCommand = new SqlCommand("insert into Tickets values (@descr, @data, @utente, @stato)", connection);
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@descr", SqlDbType.VarChar, 500, "Descrizione"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@data", SqlDbType.DateTime, 100, "Data"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@utente", SqlDbType.VarChar, 100, "Utente"));
            adapter.InsertCommand.Parameters.Add(new SqlParameter("@stato", SqlDbType.VarChar, 10, "Stato"));

            //delete
            adapter.DeleteCommand = new SqlCommand("delete from Tickets where ID=@id", connection);
            adapter.DeleteCommand.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "ID"));


            return adapter;
        }


        public bool Add(Ticket item)
        {
            DataSet dataset = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //INIZIALIZZAZIONE ADAPTER
                SqlDataAdapter adapter = InitializeDataAdapter(dataset, connection);

                //FILL DEL DATASET -> TRAMITE IL SelectCommand
                //se non faccio questo passaggio non ho nulla su cui lavorare! fondamentale
                adapter.Fill(dataset, "Tickets");

                connection.Close();

                //PREPARO NUOVO RECORD
                DataRow newTicket = dataset.Tables["Tickets"].NewRow();
                newTicket["Descrizione"] = item.Descrizione;
                newTicket["Data"] = item.Data;
                newTicket["Utente"] = item.Utente;
                newTicket["Stato"] = item.Stato.ToString();

                //AGGIUNGO NUOVO RECORD AL DATASET
                dataset.Tables["Tickets"].Rows.Add(newTicket);

                //RICONCILIO DATASET E DATABASE
                adapter.Update(dataset, "Tickets");

                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Clause Finally: connessione chiusa");
                }
            }
        }

        public bool Delete(Ticket item)
        {
            DataSet dataset = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlDataAdapter adapter = InitializeDataAdapter(dataset, connection);
                adapter.Fill(dataset, "Tickets");
                connection.Close();

                //PREPARO LA RIGA DA CANCELLARE
                DataRow ticketToDelete = dataset.Tables["Tickets"].Rows.Find(item.ID);
                if (ticketToDelete != null)
                {
                    ticketToDelete.Delete();
                }

                //RICONCILIO DATASET E DATABASE
                adapter.Update(dataset, "Tickets");

                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return false;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Clause Finally: connessione chiusa");
                }
            }
        }

        public List<Ticket> GetAll(Func<Ticket, bool> filter = null)
        {
            DataSet dataset = new DataSet();
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                //INIZIALIZZAZIONE ADAPTER
                SqlDataAdapter adapter = InitializeDataAdapter(dataset, connection);

                //FILL DEL DATASET -> TRAMITE IL SelectCommand
                //se non faccio questo passaggio non ho nulla su cui lavorare! fondamentale
                adapter.Fill(dataset, "Tickets");

                connection.Close();

                List<Ticket> allTickets = new List<Ticket>();

                foreach (DataRow row in dataset.Tables["Tickets"].Rows)
                {
                    Ticket t = new Ticket();
                    
                    t.ID = Convert.ToInt32(row[0]);
                    t.Descrizione = Convert.ToString(row[1]);
                    t.Data = Convert.ToDateTime(row[2]);
                    t.Utente = Convert.ToString(row[3]);
                    t.Stato = (StatoEnum)Enum.Parse(typeof(StatoEnum), Convert.ToString(row[4]));

                    allTickets.Add(t);

                }
                return allTickets;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
                return null;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Clause Finally: connessione chiusa");
                }
            }
        }

        public bool Update(Ticket item)
        {
            throw new NotImplementedException();
        }
    }
}
