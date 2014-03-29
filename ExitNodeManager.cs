using System;
using System.Net;

namespace TorExitNodeManager
{
    public class ExitNodeManager
    {
        private static ExitNodeManager _current = null;
        private ExitNodeCache _cache;

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

        public Boolean IsAddressTorExitNode(String ipAddress)
        {
            return (_cache.Data.Contains(ipAddress));
        }

        public Boolean IsAddressTorExitNode(IPAddress ipAddress)
        {
            return IsAddressTorExitNode(ipAddress.ToString());
        }
    }
}