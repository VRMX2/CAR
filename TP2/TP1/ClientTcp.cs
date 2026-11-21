using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TP1
{
    public static class ClientTcp
    {
        // ===== A MODIFIER pour le test sur 2 machines : IP (wifi) de la machine du serveur =====
        public const string Hote = "127.0.0.1";
        public const int Port = 8080;
        // ========================================================================================

        private const int DelaiMs = 3000;
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        /// <summary>Envoie "a:signe:b" au serveur et lit la réponse. Retourne false + message en cas d'erreur.</summary>
        public static bool Envoyer(double a, string signe, double b, out double resultat, out string erreur)
        {
            resultat = 0;
            erreur = null;
            try
            {
                using (TcpClient c = new TcpClient())
                {
                    // Connexion avec délai maximal (sinon Connect() peut bloquer longtemps)
                    IAsyncResult ar = c.BeginConnect(Hote, Port, null, null);
                    if (!ar.AsyncWaitHandle.WaitOne(DelaiMs))
                    {
                        erreur = "Serveur injoignable (" + Hote + ":" + Port + ").\nVerifiez l'IP, le port et le pare-feu.";
                        return false;
                    }
                    c.EndConnect(ar);
                    c.SendTimeout = DelaiMs;
                    c.ReceiveTimeout = DelaiMs;

                    NetworkStream ns = c.GetStream();
                    StreamWriter w = new StreamWriter(ns, new UTF8Encoding(false));
                    w.AutoFlush = true;
                    StreamReader r = new StreamReader(ns, Encoding.UTF8);

                    // envoi de la commande
                    w.WriteLine(a.ToString("R", Inv) + ":" + signe + ":" + b.ToString("R", Inv));

                    // réception de la réponse
                    string rep = r.ReadLine();
                    if (rep == null) { erreur = "Le serveur a ferme la connexion."; return false; }

                    if (rep.StartsWith("OK:"))
                        return double.TryParse(rep.Substring(3), NumberStyles.Float, Inv, out resultat);

                    erreur = rep.StartsWith("ERR:") ? rep.Substring(4) : "Reponse inconnue : " + rep;
                    return false;
                }
            }
            catch (SocketException)
            {
                erreur = "Impossible de joindre le serveur (" + Hote + ":" + Port + ").\nVerifiez que le serveur est lance, l'IP et le pare-feu.";
                return false;
            }
            catch (IOException)
            {
                erreur = "Pas de reponse du serveur (delai depasse).";
                return false;
            }
        }
    }
}