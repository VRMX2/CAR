using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace TP4_PART3
{
    class Program
    {
        private static string SQL;
        private static MySqlConnection conn;
        private static MySqlCommand cmd;
        private static MySqlDataAdapter da;
        private static DataSet ds;

        static void Main(string[] args)
        {
            // ----- Connexion -----
            try
            {
                string mydb = "Server=localhost;Port=3306;Database=db_tp4;Uid=root;Pwd=lahcengrissi123;";
                conn = new MySqlConnection(mydb);
                conn.Open();
                Console.WriteLine("Connexion : " + conn.State);
                Console.WriteLine();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Erreur de connection à la base de données .... ");
                Console.WriteLine(ex.Message);
                Console.Read();
                return;
            }

            // ----- Insertion d'un enregistrement variable -----
            bool response = true;
            while (response)
            {
                int matricule;
                string nom, prenom;

                Console.Write(" Donner le matricule : ");
                if (!int.TryParse(Console.ReadLine(), out matricule))
                {
                    Console.WriteLine(" Matricule invalide (entrez un nombre).");
                    Console.WriteLine();
                    continue;
                }
                Console.Write(" Donner le nom : "); nom = Console.ReadLine();
                Console.Write(" Donner le prenom : "); prenom = Console.ReadLine();

                SQL = "INSERT INTO Etudiant VALUES (@matricule, @nom, @prenom)";
                cmd = new MySqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@matricule", matricule);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@prenom", prenom);

                try
                {
                    int nombre = cmd.ExecuteNonQuery();
                    Console.WriteLine();
                    Console.WriteLine(nombre + " Enregistrement ajouté avec succès...");

                    // ----- Affichage de la table -----
                    SQL = "SELECT * FROM Etudiant ORDER BY matricule";
                    cmd = new MySqlCommand(SQL, conn);
                    cmd.CommandType = CommandType.Text;
                    da = new MySqlDataAdapter(cmd);
                    ds = new DataSet();
                    da.Fill(ds);

                    DataTable etude = ds.Tables[0];

                    Console.WriteLine();
                    Console.WriteLine("Affichage du contenu de la table : ETUDIANT ");
                    for (int i = 0; i < etude.Rows.Count; i++)
                    {
                        Console.WriteLine(etude.Rows[i][0].ToString() + "\t\t" +
                                          etude.Rows[i][1].ToString() + "\t\t" +
                                          etude.Rows[i][2].ToString());
                    }

                    Console.WriteLine();
                    Console.WriteLine("Le nombre d'enregistrement de la table : " + etude.Rows.Count);
                    Console.WriteLine();
                }
                catch (MySqlException ex) when (ex.Number == 1062)
                {
                    Console.WriteLine();
                    Console.WriteLine(" L'Enregistrement existe déjà......");
                    Console.WriteLine();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine();
                    Console.WriteLine(" Erreur : " + ex.Message);
                    Console.WriteLine();
                }

                Console.Write("Tapez 'O' pour insérer d'autres enregistrements : ");
                if (Console.ReadLine().ToUpper() != "O") { response = false; }
                Console.WriteLine();
            }

            conn.Close();
            Console.WriteLine("Connexion fermée. Appuyez sur une touche...");
            Console.Read();
        }
    }
}