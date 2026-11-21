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

            operation = (IRemoteOperation)RemotingServices.Connect(
                typeof(IRemoteOperation),
                "tcp://localhost:8085/RemoteOperation");

            connected = true;
        }
    }
}