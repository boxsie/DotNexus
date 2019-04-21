namespace DotNexus.Core.Nexus
{
    public class NexusNode
    {
        public INexusClient Client { get; private set; }
        public NexusNodeParameters Parameters { get; private set; }

        public NexusNode(INexusClient nexusClient, NexusNodeParameters nodeParameters)
        {
            Client = nexusClient;
            Parameters = nodeParameters;

            Client.ConfigureHttpClient(nodeParameters);
        }
    }

    public class NexusNodeParameters
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public NexusNodeSettings Settings { get; set; }
    }

    public class NexusNodeSettings
    {
        public bool ApiSessions { get; set; }
        public bool IndexHeight { get; set; }
    }
}