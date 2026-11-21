using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class client
    {
        private string clientHost = null;       // adresse du client
        private int clientPort = -1;            // port du client
        private string serveurHost = null;      // adresse du serveur
        private int serveurPort = -1;           // port du serveur

        UdpClient udp = null;                   // le client UDP

        // constructeur
        public client(string clientHost, int clientPort, string serveurHost, int serveurPort)
        {
            this.clientHost = clientHost;
            this.clientPort = clientPort;
            this.serveurHost = serveurHost;
            this.serveurPort = serveurPort;
            this.udp = new UdpClient(clientPort);

            // Sans cela, Receive() bloquerait indéfiniment si le serveur est arrêté
            this.udp.Client.ReceiveTimeout = 3000;
        }

        public void Close() { this.udp.Close(); }

        public bool Execute(string command, ref string resultat)
        {
            // commande à envoyer : ip:port:nb1:signe:nb2
            string req = this.clientHost + ":" + this.clientPort + ":" + command;

            // conversion de la commande en bytes
            byte[] rq = Encoding.UTF8.GetBytes(req);

            try
            {
                // envoi de la commande
                udp.Send(rq, rq.Length, this.serveurHost, this.serveurPort);

                // variable de réception du résultat
                IPEndPoint ip = null;

                // réception
                byte[] r = udp.Receive(ref ip);

                // conversion
                resultat = Encoding.UTF8.GetString(r);
            }
            catch (SocketException)
            {
                resultat = "Erreur : pas de reponse du serveur (verifier l'IP, le port et le pare-feu)";
                return false;
            }

            // (le code du TP faisait  !resultat.Equals(0)  : un string n'est jamais égal à 0, donc toujours vrai)
            return !resultat.StartsWith("Erreur");
        }
    }
}