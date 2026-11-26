using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace RemotingServer
{
    public class RemoteMain
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                // Full type filter level is required to send DataTable objects
                BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
                provider.TypeFilterLevel = TypeFilterLevel.Full;

                Hashtable props = new Hashtable();
                props["port"] = 1069;

                TcpChannel channel = new TcpChannel(props, null, provider);
                ChannelServices.RegisterChannel(channel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(RemoteOperation), "RemoteOperation", WellKnownObjectMode.Singleton);

                Console.WriteLine("Le serveur a démarré avec succès (port 1069)");
                Console.WriteLine("Connexion à la base : " + new RemoteOperation().Connect());
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors du démarrage du serveur : " + e.Message);
                Console.ReadLine();
            }
        }
    }
}