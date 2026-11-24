using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using RemotingInterfaces;

namespace RemotingClient
{
    public static class RemoteConnection
    {
        public static IRemoteOperation operation;
        private static bool connected = false;

        public static void Connect()
        {
            if (connected) return;

            TcpChannel channel = new TcpChannel(0);
            ChannelServices.RegisterChannel(channel, false);

            // Port must match the server (1069)
            operation = (IRemoteOperation)RemotingServices.Connect(
                typeof(IRemoteOperation),
                "tcp://localhost:1069/RemoteOperation");

            connected = true;
        }
    }
}