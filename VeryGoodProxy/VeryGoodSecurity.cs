using System.Net;
using System.Web;
using VeryGoodProxy;

// Any project this dll is added to will have its startup hooked
[assembly: PreApplicationStartMethod(typeof(VeryGoodSecurity), "Start")]
namespace VeryGoodProxy
{
    public class VeryGoodSecurity
    {
        public static void Start()
        {
            if (VeryGoodProxy.IsEnabled)
            {
                // all .NET calls to go through the proxy (including WCF, WebRequest, WebClient)
                WebRequest.DefaultWebProxy = new VeryGoodProxy();
            }
        }
    }
}