using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace TorExitNodeManager
{
    public class OpenProxyManager
    {

        private static string[] PROXY_HEADERS = new string[]
        {
            "VIA", "FORWARDED", "X-FORWARDED-FOR", "PROXY-CONNECTION", "XPROXY_CONNECTION", "X-PROXY_CONNECTION",
            "HTTP-PC-REMOTE-ADDR", "HTTP-CLIENT-IP", "USERAGENT-VIA"
        };

        public static Boolean IsCurrentRequestFromProxy()
        {
            if (WebOperationContext.Current != null)
            {

                foreach (String header in PROXY_HEADERS)
                {
                    if (WebOperationContext.Current.IncomingRequest.Headers[header] != null
                        && WebOperationContext.Current.IncomingRequest.Headers[header].Length > 0)
                        return true;
                }
            }

            return false;
        }
    }
}
