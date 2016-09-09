using System.Net;

namespace TorExitNodeManager
{
    public class ExitNodeManager
    {
        private static ExitNodeManager _current;
        private readonly ExitNodeCache _cache;

        private ExitNodeManager()
        {
            _cache = new ExitNodeCache();
        }

        public static ExitNodeManager Current
        {
            get
            {
                if (_current == null)
                    _current = new ExitNodeManager();
                return _current;
            }
        }

        public bool IsAddressTorExitNode(string ipAddress)
        {
            if (_cache != null && _cache.Data != null && _cache.Data.Count > 0)
                return (_cache.Data.Contains(ipAddress));
            return false;
        }

        public bool IsAddressTorExitNode(IPAddress ipAddress)
        {
            return IsAddressTorExitNode(ipAddress.ToString());
        }
    }
}