using System;

namespace TP4
{
    class Program
    {
        static void Main(string[] args)
        {
            string strConn = Connection.Connect();
            Console.WriteLine("État de la connexion : " + strConn);
            if (strConn != "Open") { Console.Read(); return; }

            Console.WriteLine("Exemple de requête SELECT ...");
            Console.WriteLine();
            Connection.GetEtudiants();

            Console.WriteLine("Exemple de requête INSERT ...");
            Console.WriteLine();
            int compteur = Connection.PutEtudiants();
            Console.WriteLine(" La table 'Etudiant' contient " + compteur + " tuples");

            Connection.Terminate();
            Console.Read();
        }
    }
}