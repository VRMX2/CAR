using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpServeur
{
    class serveur
    {
        static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        static string Fmt(double v) { return v.ToString("G15", Inv); }

        static void Main(string[] args)
        {
            int P = 8080;                                   // port d'écoute
            TcpListener ecoute = new TcpListener(IPAddress.Any, P);
            ecoute.Start();
            Console.WriteLine("Demarage du serveur TCP (port " + P + ")...");
            Console.WriteLine("Attente de connexion(s)... (Entree pour arreter)");

            Thread accepteur = new Thread(delegate ()
            {
                while (true)
                {
                    TcpClient c;
                    try { c = ecoute.AcceptTcpClient(); }
                    catch (SocketException) { break; }          // listener arrêté
                    catch (InvalidOperationException) { break; }

                    Thread t = new Thread(Servir);              // 1 thread par client
                    t.IsBackground = true;
                    t.Start(c);
                }
            });
            accepteur.IsBackground = true;
            accepteur.Start();

            Console.ReadLine();
            ecoute.Stop();
            Console.WriteLine("Arret du serveur...");
        }

        // Boucle de service d'un client : 1 ligne reçue = 1 ligne renvoyée
        static void Servir(object obj)
        {
            TcpClient c = (TcpClient)obj;
            string distant = c.Client.RemoteEndPoint.ToString();
            Console.WriteLine("Connexion de " + distant);
            try
            {
                using (c)
                using (NetworkStream ns = c.GetStream())
                using (StreamReader lecteur = new StreamReader(ns, Encoding.UTF8))
                using (StreamWriter ecrivain = new StreamWriter(ns, new UTF8Encoding(false)))
                {
                    ecrivain.AutoFlush = true;
                    string ligne;
                    while ((ligne = lecteur.ReadLine()) != null)
                    {
                        ecrivain.WriteLine(Traiter(ligne));
                    }
                }
            }
            catch (IOException) { }
            Console.WriteLine("Deconnexion de " + distant);
        }

        // Requête  : nb1:signe:nb2
        // Réponse  : OK:resultat   ou   ERR:message
        static string Traiter(string requete)
        {
            string[] cmd = requete.Split(':');
            double nb1, nb2;

            if (cmd.Length != 3
                || !double.TryParse(cmd[0], NumberStyles.Float, Inv, out nb1)
                || !double.TryParse(cmd[2], NumberStyles.Float, Inv, out nb2))
            {
                Console.WriteLine("Commande invalide : " + requete);
                return "ERR:Commande invalide";
            }

            string signe = cmd[1];
            Console.WriteLine("Operation --> " + Fmt(nb1) + " " + signe + " " + Fmt(nb2));

            double resultat;
            switch (signe)
            {
                case "+": resultat = nb1 + nb2; break;
                case "-": resultat = nb1 - nb2; break;
                case "*": resultat = nb1 * nb2; break;
                case "/":
                    if (nb2 == 0) return "ERR:Division impossible !";
                    resultat = nb1 / nb2;
                    break;
                default: return "ERR:Operateur inconnu (" + signe + ")";
            }

            if (double.IsInfinity(resultat) || double.IsNaN(resultat))
                return "ERR:Depassement de capacite !";

            return "OK:" + Fmt(resultat);
        }
    }
}