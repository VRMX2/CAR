using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace TP4
{
    static class Connection
    {
        private static string SQL;
        private static MySqlConnection conn;
        private static MySqlCommand cmd;
        private static MySqlDataAdapter da;
        private static DataSet ds;

        // Première méthode
        public static string Connect()
        {
            try
            {
                string mydb = "Server=localhost;Port=3306;Database=db_tp4;Uid=root;Pwd=lahcengrissi123;";
                conn = new MySqlConnection(mydb);
                conn.Open();
            }
            catch (MySqlException e)
            {
                return e.Message;
            }
            return conn.State.ToString();
        }

        private static void Afficher()
        {
            SQL = "SELECT * FROM Etudiant";
            cmd = new MySqlCommand(SQL, conn);
            cmd.CommandType = CommandType.Text;
            da = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine(ds.Tables[0].Rows[i][0].ToString() + "\t\t" +
                                  ds.Tables[0].Rows[i][1].ToString() + "\t\t" +
                                  ds.Tables[0].Rows[i][2].ToString());
            }
            Console.WriteLine("________________________________________________");
            Console.WriteLine();
        }

        // Deuxième méthode
        public static void GetEtudiants()
        {
            Afficher();
        }

        // Troisième méthode
        public static int PutEtudiants()
        {
            Console.Write("Matricule : "); int matricule = int.Parse(Console.ReadLine());
            Console.Write("Nom : "); string nom = Console.ReadLine();
            Console.Write("Prénom : "); string prenom = Console.ReadLine();

            SQL = "INSERT INTO Etudiant VALUES (@matricule, @nom, @prenom)";
            cmd = new MySqlCommand(SQL, conn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.AddWithValue("@matricule", matricule);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@prenom", prenom);
            int nombre = cmd.ExecuteNonQuery();

            Console.WriteLine();
            Console.WriteLine(nombre + " Enregistrement ajouté avec succès...");
            Console.WriteLine();

            Afficher();
            return ds.Tables[0].Rows.Count;
        }

        // Quatrième méthode
        public static void Terminate()
        {
            conn.Close();
        }
    }
}