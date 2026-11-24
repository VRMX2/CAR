using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;
using RemotingInterfaces;

namespace RemotingClient
{
    public static class RemoteConnection
    {
        // Change to the server IP when you use 3 machines, e.g. 192.168.2.15
        private const string SERVER = "127.0.0.1";

        public static IRemoteOperation operation;

        public static string Connect()
        {
            try
            {
                if (operation == null)
                {
                    // Full is required to receive DataTable objects
                    BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                    serverProv.TypeFilterLevel = TypeFilterLevel.Full;
                    BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                    Hashtable props = new Hashtable();
                    props["port"] = 0;

                    TcpChannel channel = new TcpChannel(props, clientProv, serverProv);
                    ChannelServices.RegisterChannel(channel, false);   // false, like the server

                    operation = (IRemoteOperation)Activator.GetObject(
                        typeof(IRemoteOperation),
                        "tcp://" + SERVER + ":1069/RemoteOperation");
                }
                return operation.Connect();    // "OK" or "ECHEC : ..."
            }
            catch (Exception ex)
            {
                return "Erreur de connexion au serveur : " + ex.Message;
            }
        }
    }
}