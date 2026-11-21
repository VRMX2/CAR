using System;
using System.Globalization;

namespace Client
{
    class UDPClient
    {
        // ================= A MODIFIER pour le test sur 2 machines (étapes 7 et 8 du TP) =================
        // Sur une seule machine : laisser 127.0.0.1 partout.
        const string IP_CLIENT = "127.0.0.1";    // adresse IP (wifi) de la machine qui héberge le projet Client
        const string IP_SERVEUR = "127.0.0.1";   // adresse IP (wifi) de la machine qui héberge UdpServeur
        // =================================================================================================

        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        // Lit un nombre au clavier (accepte "2,5" et "2.5"). Retourne false si ce n'est pas un nombre.
        static bool LireNombre(string texte, out double valeur)
        {
            valeur = 0;
            if (texte == null) return false;
            return double.TryParse(texte.Trim().Replace(',', '.'), NumberStyles.Float, Inv, out valeur);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Initialisation du client...");
            client proc = new client(IP_CLIENT, 8081, IP_SERVEUR, 8080);
            string x, y, signe, command, resultat = "";
            double nx, ny;

            while (true)
            {
                Console.WriteLine(" ");
                Console.WriteLine("COMMANDE -> Pour quitter x = 0\n");

                // entrer x
                Console.Write("x     = "); x = Console.ReadLine();
                if (x == null) break;                                   // fin de l'entrée
                if (!LireNombre(x, out nx)) { Console.WriteLine("Nombre invalide !"); continue; }

                // terminer si x = 0
                if (nx == 0) break;

                // entrer le signe  (+  -  *  /   ou  <>  pour arrêter le serveur)
                Console.Write("signe : "); signe = Console.ReadLine();
                if (signe == null) break;
                signe = signe.Trim();
                if (signe == "" || signe.Contains(":")) { Console.WriteLine("Signe invalide !"); continue; }

                // entrer y
                Console.Write("y     = "); y = Console.ReadLine();
                if (!LireNombre(y, out ny)) { Console.WriteLine("Nombre invalide !"); continue; }

                command = nx.ToString("R", Inv) + ":" + signe + ":" + ny.ToString("R", Inv);

                proc.Execute(command, ref resultat);
                Console.WriteLine("RESULTAT -> " + resultat);
            }

            proc.Close();
        }
    }
}