using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServeur
{
    class serveur
    {
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        // Nombre -> texte (point décimal, 15 chiffres significatifs)
        static string Fmt(double v) { return v.ToString("G15", Inv); }

        // Envoie un message texte en UDP
        static void Repondre(UdpClient udp, string message, string host, int port)
        {
            byte[] rep = Encoding.UTF8.GetBytes(message);
            udp.Send(rep, rep.Length, host, port);
        }

        static void Main(string[] args)
        {
            int P = 8080;                       // port de connexion
            IPEndPoint ip = null;

            // Demarrage du serveur
            Console.WriteLine("Demarage du serveur...");
            UdpClient udp = new UdpClient(P);

            bool loop = true;

            while (loop)
            {
                // Attente de connexion
                Console.WriteLine("Attente de connexion(s)...");
                byte[] tmp = udp.Receive(ref ip);

                // Conversion des bytes en string
                string data = Encoding.UTF8.GetString(tmp);

                // Décodage de la commande --> ip:port:nb1:signe:nb2
                string[] cmd = data.Split(new char[] { ':' });

                string host = cmd[0];           // adresse ip du client
                int port = 0;                   // port du client
                double nb1 = 0, nb2 = 0;

                bool valide = cmd.Length == 5
                    && int.TryParse(cmd[1], out port)
                    && double.TryParse(cmd[2], NumberStyles.Float, Inv, out nb1)
                    && double.TryParse(cmd[4], NumberStyles.Float, Inv, out nb2);

                if (!valide)
                {
                    // Commande mal formée : on répond à l'expéditeur réel
                    Console.WriteLine("Commande invalide : " + data);
                    Repondre(udp, "Erreur : commande invalide", ip.Address.ToString(), ip.Port);
                    continue;
                }

                string signe = cmd[3];
                double resultat = 0;
                string erreur = null;

                Console.WriteLine("Operation --> " + Fmt(nb1) + " " + signe + " " + Fmt(nb2));

                // Exécution de la commande
                switch (signe)
                {
                    case "+": resultat = nb1 + nb2; break;
                    case "-": resultat = nb1 - nb2; break;
                    case "*": resultat = nb1 * nb2; break;
                    case "/":
                        if (nb2 == 0) erreur = "division par zero impossible";
                        else resultat = nb1 / nb2;
                        break;
                    case "<>": loop = false; break;                 // arrêt du serveur
                    default: erreur = "operateur inconnu (" + signe + ")"; break;
                } // fin du switch

                string reponse;
                if (!loop) reponse = "Arret du serveur";
                else if (erreur != null) reponse = "Erreur : " + erreur;
                else reponse = Fmt(nb1) + " " + signe + " " + Fmt(nb2) + " = " + Fmt(resultat);

                // Conversion du résultat en bytes puis envoi au client
                Repondre(udp, reponse, host, port);

            } // fin du while

            // Arrêt du serveur
            udp.Close();
            Console.WriteLine("Arret du serveur... *Entree* pour terminer.");
            Console.ReadLine();
        }
    }
}