using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.ADO.ConsoleApp
{
    internal static class AdoNetDemo
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Cocktails;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void ConnectionDemo()
        {
            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            if (connection.State == ConnectionState.Open)
            {
                Console.WriteLine("Connessi al DB :)");
            }
            else
                Console.WriteLine("Non connessi :(");

            connection.Close();
        }

        public static void DataReaderDemo()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "select * from Ingrediente";

                #region creazione del comando
                //METODO 1
                SqlCommand command1 = new SqlCommand();
                command1.Connection = connection;
                command1.CommandType = CommandType.Text;
                command1.CommandText = query;

                //METODO 2
                SqlCommand command2 = new SqlCommand(query, connection);

                //METODO 3
                SqlCommand command3 = connection.CreateCommand();
                command3.CommandText = query;
                #endregion

                SqlDataReader reader = command1.ExecuteReader();

                Console.WriteLine("Ingredienti:");
                while (reader.Read())
                {
                    var id = reader.GetInt32(0); //LETTURA TIPIZZATA
                    var nome = reader.GetString(1);

                    var id2 = (int)reader["IdIngrediente"];//LETTURA CON NOME DEL CAMPO
                    var nome2 = (string)reader["Nome"];

                    Console.WriteLine($"{id} - {nome}");
                }

                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Connessione chiusa");
                }
            }

        }

        public static void InsertDemo()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string insert = "insert into Ingrediente values ('Limoncello', 'Limoncello dello zio Nino', 'ml')";
                SqlCommand cmd = new SqlCommand(insert, connection);
                int rows = cmd.ExecuteNonQuery();

                Console.WriteLine($"Righe inserite: {rows}");

                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Connessione chiusa");
                }
            }
        }

        public static void InsertWithParametersDemo(string nome, string descr, string unitMis)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string insert = "insert into Ingrediente values (@nome, @descr, @unit)";
                SqlCommand cmd = new SqlCommand(insert, connection);

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@nome";
                sqlParameter.Value = nome;
                sqlParameter.DbType = DbType.String;
                cmd.Parameters.Add(sqlParameter);   

                //cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@descr", descr);
                cmd.Parameters.AddWithValue("@unit", unitMis);

                


                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"Righe inserite: {rows}");

                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Connessione chiusa");
                }
            }
        }

        public static void DeleteWithParametersDemo(int idIngrediente)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                string insert = "delete from Ingrediente where IdIngrediente = @id";
                SqlCommand cmd = new SqlCommand(insert, connection);

                cmd.Parameters.AddWithValue("@id", idIngrediente);

                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"Righe cancellate: {rows}");

                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Connessione chiusa");
                }
            }
        }

        public static void StoredProcedureDemo(string nome, int minutiPrep, string prep, int numeroPers, string titoloLibro)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                SqlCommand spInsertCocktail = connection.CreateCommand();
                spInsertCocktail.CommandType = CommandType.StoredProcedure;
                spInsertCocktail.CommandText = "InserisciCocktail"; //il nome della procedure!
                spInsertCocktail.Parameters.AddWithValue("@nomeCocktail", nome);
                spInsertCocktail.Parameters.AddWithValue("@tempoPreparazione", minutiPrep);
                spInsertCocktail.Parameters.AddWithValue("@preparazione", prep);
                spInsertCocktail.Parameters.AddWithValue("@numeroPersone", numeroPers);
                spInsertCocktail.Parameters.AddWithValue("@libro", titoloLibro);

                int rows = spInsertCocktail.ExecuteNonQuery();
                Console.WriteLine($"Affected rows: {rows}");

                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Connessione chiusa");
                }
            }
        }

        public static void MultipleResultsDemo()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from Ingrediente, select * from  Libro";

                SqlDataReader reader = cmd.ExecuteReader();

                int index = 0;
                while (reader.HasRows)
                connection.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Specifico errore Sql: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore generico: {ex.Message}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Connessione chiusa");
                }
            }
        }
    }
}
