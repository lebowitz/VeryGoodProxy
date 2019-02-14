using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace VeryGoodProxy
{
    public class VeryGoodProxy : WebProxy
    {
        private static readonly X509Certificate2 RootCertificate;
        private static readonly string ProxyUrl;

        static VeryGoodProxy()
        {
            ProxyUrl = ConfigurationManager.AppSettings["VeryGoodProxyUrl"];

            string certificate = ConfigurationManager.AppSettings["VeryGoodProxyCertificate"];
            if (!string.IsNullOrEmpty(certificate))
            {
                RootCertificate = new X509Certificate2(Convert.FromBase64String(certificate));
            }
        }

        public VeryGoodProxy(string[] bypassList = null)
        {
            if (bypassList != null)
            {
                BypassList = bypassList;
            }

            BypassProxyOnLocal = false;
            
            Uri u = new Uri(ProxyUrl);
            base.Address = new Uri(u.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped));
            Trace.WriteLine($"VeryGoodProxy Proxy Address: {Address}");
            string[] userInfo = u.UserInfo.Split(':');
            base.Credentials = new NetworkCredential(userInfo[0], userInfo[1]);

            if (RootCertificate != null)
            {
                ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
            }
        }

        /*
         * This preserves normal validation, with the one exception of trusting the configured VeryGoodProxy certificate.
         */
        private static bool CertificateValidationCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {            
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            if (certificate != null)
            {
                if (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors)
                {
                    foreach (X509ChainElement elt in chain.ChainElements)
                    {
                        X509ChainStatus[] statuses = elt.ChainElementStatus;

                        foreach (X509ChainStatus status in statuses)
                        {
                            switch (status.Status)
                            {
                                case X509ChainStatusFlags.UntrustedRoot:
                                    if (RootCertificate.GetCertHashString() == elt.Certificate.GetCertHashString())
                                    {
                                        // exception for the trusted VeryGoodProxy root certificate
                                        Trace.WriteLine($"VeryGoodProxy Trusted Root Certificate: {certificate}");
                                        continue;
                                    }

                                    return false;
                                case X509ChainStatusFlags.NoError:
                                    continue;
                                default:
                                    return false; // no other excuse
                            }
                        }
                    }

                    return true; // the only error was trusting the known good root
                }
            }

            return false;
        }

        public static bool IsEnabled {
            get
            {
                return !string.IsNullOrEmpty(ProxyUrl);
            }
        } 
    }
}