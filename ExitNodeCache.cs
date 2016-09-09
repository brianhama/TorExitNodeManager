using System.Collections.Generic;
using System.Net;

namespace TorExitNodeManager
{
    internal class ExitNodeCache : MemoryCacheBase<HashSet<string>>
    {
        private const string EXIT_NODE_STATUS_URI =
            "http://torstatus.blutmagie.de/ip_list_exit.php/Tor_ip_list_EXIT.csv";

        protected override HashSet<string> FetchCacheData()
        {
            WebClient client = new WebClient();
            string data = client.DownloadString(EXIT_NODE_STATUS_URI);
            return new HashSet<string>(data.Split('\r', '\n'));
        }
    }
}