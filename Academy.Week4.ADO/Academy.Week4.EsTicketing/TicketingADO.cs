using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Academy.Week4.EsTicketing.Ticket;

namespace Academy.Week4.EsTicketing
{
    internal static class TicketingADO
    {
        //Realizzare una Console app che acceda al database Ticketing utilizzando il Connected Mode di ADO.NET e che:
        //      Stampi la lista dei Ticket in ordine cronologico (dal più recente al più vecchio)
        //      Permetta l'inserimento di nuovi Ticket (i dati devono essere inseriti dall'utente)
        //      Permetta la cancellazione di un Ticket (utilizzare l'ID univoco per identificarlo)

        internal static List<Ticket> GetAll(string queryDetails)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "select * from Tickets " + queryDetails;
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

        internal static bool Add(Ticket ticket)
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
        internal static bool Delete(int ticketID)
        {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Ticketing;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("delete from Tickets where ID=@id", connection);
                cmd.Parameters.AddWithValue("@id", ticketID);

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
    }
}
