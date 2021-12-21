using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Week4.ADOEF.DisconnectedMode
{
    public static class AdoNetDisconnected
    {
        static string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Cocktails;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public static void FillDataSet()
        {
            DataSet cocktailsDS = new DataSet();                //creo un oggetto DataSet vuoto
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("Connessione chiusa");

                InitializeDataSetAndDataAdapter(cocktailsDS, connection); //creo un metodo... con cui farò tutto il lavoro

                connection.Close();     //da qui in poi sono disconnesso! lavoro offline

                //VEDIAMO IL CONTENUTO DEL DATASET
                foreach (DataTable table in cocktailsDS.Tables)        //posso andare a vedere che c'è nella lista di tabelle del dataset
                {
                    Console.WriteLine($"{table.TableName} - righe: {table.Rows.Count}");
                }

                Console.WriteLine("--------------- Ingredienti ---------------");
                foreach (DataRow item in cocktailsDS.Tables["Ingrediente"].Rows)
                {
                    Console.WriteLine($"{item["IdIngrediente"]} - {item["Nome"]} - {item["Descrizione"]}");
                }


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

        public static void InsertRow()
        {
            DataSet cocktailsDS = new DataSet();                //creo un oggetto DataSet vuoto
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("Connessione chiusa");

                SqlDataAdapter ingredienteAdapter = InitializeDataSetAndDataAdapter(cocktailsDS, connection);
                connection.Close();

                //adesso dovrò creare una riga da aggiungere alla tabella del dataset

                DataRow newRow = cocktailsDS.Tables["Ingrediente"].NewRow(); //sto creando una nuova riga proprio in questa tabella
                newRow["Nome"] = "Cannella";
                newRow["Descrizione"] = "cannella in polvere";
                newRow["UnitaMisura"] = "gr";

                cocktailsDS.Tables["Ingrediente"].Rows.Add(newRow);
                //cosa manca adesso? manca l'update dell'adapter che mi porta le modifiche del dataset sul database
                ingredienteAdapter.Update(cocktailsDS, "Ingrediente");
                // questo update si accorgerà di dover fare una INSERT per gestire la nuova riga
                Console.WriteLine("Database aggiornato!");
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

        public static void UpdateRow()
        {
            DataSet cocktailsDS = new DataSet();                //creo un oggetto DataSet vuoto
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("Connessione chiusa");

                SqlDataAdapter ingredienteAdapter = InitializeDataSetAndDataAdapter(cocktailsDS, connection);
                connection.Close();

                //ora avrò da dirgli un campo da aggiornare + l'id per dove aggiornare
                DataRow rowToUpdate = cocktailsDS.Tables["Ingrediente"].Rows.Find(4); // search by PK
                if (rowToUpdate != null)
                {
                    //a questo punto chiediamo/diamo il nuovo dato da aggiornare
                    rowToUpdate["Descrizione"] = "prosecco brut";
                }

                //dopo aver modificato il dataset devo riconciliare dataset e database
                ingredienteAdapter.Update(cocktailsDS, "Ingrediente");
                Console.WriteLine("Database aggiornato!");
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

        public static void DeleteRow()
        {
            DataSet cocktailsDS = new DataSet();                //creo un oggetto DataSet vuoto
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("Connessione chiusa");

                SqlDataAdapter ingredienteAdapter = InitializeDataSetAndDataAdapter(cocktailsDS, connection);
                connection.Close();

                DataRow rowToDelete = cocktailsDS.Tables["Ingrediente"].Rows.Find(17);
                if (rowToDelete != null)
                {
                    rowToDelete.Delete();
                }
                ingredienteAdapter.Update(cocktailsDS, "Ingrediente");
                Console.WriteLine("Record eliminato!");

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


        public static void MultiTableFill()
        {
            DataSet cocktailsDS = new DataSet();                //creo un oggetto DataSet vuoto
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("Connessione chiusa");
                //----------------------------------------------------------------------
                //avremo un dataAdapter per ogni tabella:
                //uno che fa la select * da Libro, l'altro che fa la select * da Cocktail

                SqlDataAdapter libroAdapter = new SqlDataAdapter();
                libroAdapter.SelectCommand = new SqlCommand("select * from Libro", connection);
                libroAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                libroAdapter.Fill(cocktailsDS, "Libro");

                SqlDataAdapter cocktailAdapter = new SqlDataAdapter();
                cocktailAdapter.SelectCommand = new SqlCommand("select * from Cocktail", connection);
                cocktailAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                cocktailAdapter.Fill(cocktailsDS, "Cocktail");

                connection.Close(); //chiudo la connessione, non mi serve più

                foreach (DataTable table in cocktailsDS.Tables)
                {
                    Console.WriteLine($"Tabella {table.TableName}");
                }

                //----------------------------------------------------------------------
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

        public static void MultiTableConstraints()
        {
            DataSet cocktailsDS = new DataSet();                //creo un oggetto DataSet vuoto
            using SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("Connessione chiusa");

                //DATASET CARICATO CON TABELLE LIBRO E COCKTAIL:
                //----------------------------------------------------------------------
                //avremo un dataAdapter per ogni tabella:
                //uno che fa la select * da Libro, l'altro che fa la select * da Cocktail

                SqlDataAdapter libroAdapter = new SqlDataAdapter();
                libroAdapter.SelectCommand = new SqlCommand("select * from Libro", connection);
                libroAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                libroAdapter.Fill(cocktailsDS, "Libro");

                SqlDataAdapter cocktailAdapter = new SqlDataAdapter();
                cocktailAdapter.SelectCommand = new SqlCommand("select * from Cocktail", connection);
                cocktailAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                cocktailAdapter.Fill(cocktailsDS, "Cocktail");


                //AGGIUNTA DEI CONSTRAINS:
                //----------------------------------------------------------------------

                //unique
                DataTable tabellaLibro = cocktailsDS.Tables["Libro"];
                UniqueConstraint titoloLibroUnique = new UniqueConstraint(tabellaLibro.Columns["Titolo"]);
                tabellaLibro.Constraints.Add(titoloLibroUnique);

                //FK
                DataColumn fatherColumnLibro = cocktailsDS.Tables["Libro"].Columns["IdLibro"];
                DataColumn childColumnCocktail = cocktailsDS.Tables["Cocktail"].Columns["IdLibro"];
                ForeignKeyConstraint fk_Libro = new ForeignKeyConstraint(fatherColumnLibro, childColumnCocktail);
                fk_Libro.ConstraintName = "FK_Libro_Cocktail";
                cocktailsDS.Tables["Cocktail"].Constraints.Add(fk_Libro);   
                //----------------------------------------------------------------------

                connection.Close(); //chiudo la connessione, non mi serve più


                foreach (DataTable tab in cocktailsDS.Tables)
                {
                    Console.WriteLine($"{tab.TableName} - {tab.Rows.Count}");
                    foreach (Constraint constraint in tab.Constraints)
                    {
                        Console.WriteLine($"{tab.TableName} - {constraint.ConstraintName}");
                    }
                }

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


        private static SqlDataAdapter InitializeDataSetAndDataAdapter(DataSet cocktailsDS, SqlConnection connection)
        {

            SqlDataAdapter cocktailsAdapter = new SqlDataAdapter();

            //SELECT
            cocktailsAdapter.SelectCommand = new SqlCommand("select * from Ingrediente", connection);
            cocktailsAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            cocktailsAdapter.Fill(cocktailsDS, "Ingrediente"); //qui la chiamo uguale, ma di fatto sto dando il nome alla copia

            //INSERT
            cocktailsAdapter.InsertCommand = GenerateInsertCommand(connection);

            //UPDATE
            cocktailsAdapter.UpdateCommand = GenerateUpdateCommand(connection);

            //DELETE
            cocktailsAdapter.DeleteCommand = GenerateDeleteCommand(connection);

            return cocktailsAdapter;
        }

        private static SqlCommand GenerateDeleteCommand(SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "delete from Ingrediente where IdIngrediente=@id";
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));

            return cmd;
        }

        private static SqlCommand GenerateInsertCommand(SqlConnection connection)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandType = CommandType.Text;
            command.CommandText = ("insert into Ingrediente values (@nome, @descr,@unit)");
            command.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar, 30, "Nome"));
            command.Parameters.Add(new SqlParameter("@descr", SqlDbType.VarChar, 50, "Descrizione"));
            command.Parameters.Add(new SqlParameter("@unit", SqlDbType.VarChar, 10, "UnitaMisura"));

            return command;
        }

        private static SqlCommand GenerateUpdateCommand(SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "update Ingrediente set Nome=@nome, Descrizione=@descr, UnitaMisura=@unit where IdIngrediente=@id";
            cmd.Parameters.Add(new SqlParameter("@nome", SqlDbType.VarChar, 30, "Nome"));
            cmd.Parameters.Add(new SqlParameter("@descr", SqlDbType.VarChar, 50, "Descrizione"));
            cmd.Parameters.Add(new SqlParameter("@unit", SqlDbType.VarChar, 10, "UnitaMisura"));
            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int, 0, "IdIngrediente"));

            return cmd;
        }
    }
}
