using RemotingInterfaces;
using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace RemotingClient
{
    public static class RemoteConnection
    {
        public static IRemoteOperation operation;

        public static void Connect()
        {
            if (operation != null) return;

            if (ChannelServices.GetChannel("tcpClient") == null)
            {
                BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                serverProv.TypeFilterLevel = TypeFilterLevel.Full;
                BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                IDictionary props = new Hashtable();
                props["name"] = "tcpClient";
                props["port"] = 0;

                ChannelServices.RegisterChannel(new TcpChannel(props, clientProv, serverProv), false);
            }

            operation = (IRemoteOperation)Activator.GetObject(
                typeof(IRemoteOperation),
                "tcp://localhost:1069/RemoteOperation");
        }
    }
}