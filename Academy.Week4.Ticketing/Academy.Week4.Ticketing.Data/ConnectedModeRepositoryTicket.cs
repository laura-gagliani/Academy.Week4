using Academy.Week4.Ticketing.Core;
using Academy.Week4.Ticketing.Core.RepositoryInterfaces;
using System.Data.SqlClient;
using static Academy.Week4.Ticketing.Core.Ticket;

namespace Academy.Week4.Ticketing.Data
{
    public class ConnectedModeRepositoryTicket : IRepositoryTicket
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public bool Add(Ticket ticket)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("insert into Tickets values (@descr, @data, @ut, @stato)", connection);
                cmd.Parameters.AddWithValue("@descr", ticket.Descrizione);
                cmd.Parameters.AddWithValue("@data", ticket.Data);
                cmd.Parameters.AddWithValue("@ut", ticket.Utente);
                cmd.Parameters.AddWithValue("@stato", ticket.Stato.ToString());

                int rows = cmd.ExecuteNonQuery();
                if (rows == 1)
                    return true;
                else
                    return false;

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        public bool Delete(Ticket item)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("delete from Tickets where ID=@id", connection);
                cmd.Parameters.AddWithValue("@id", item.ID);

                int rows = cmd.ExecuteNonQuery();
                if (rows == 1)
                    return true;
                else
                    return false;

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        public List<Ticket> GetAll(Func<Ticket, bool> filter = null)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "select * from Tickets "; /*+ queryDetails;*/
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<Ticket> tickets = new List<Ticket>();
                while (reader.Read())
                {
                    Ticket ticket = new Ticket();

                    ticket.ID = reader.GetInt32(0);
                    ticket.Descrizione = reader.GetString(1);
                    ticket.Data = reader.GetDateTime(2);
                    ticket.Utente = reader.GetString(3);
                    ticket.Stato = (StatoEnum)Enum.Parse(typeof(StatoEnum), reader.GetString(4));

                    tickets.Add(ticket);

                }
                connection.Close();
                return tickets;

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        public bool Update(Ticket item)
        {
            throw new NotImplementedException();
        }
    }
}