# Why this library instead of OS?

- Setting HTTP proxy credentials (required) is not supported through .NET application configuration files. Credentials must be registered with winhttp.dll in the IE dialog for proxy config or by modifying registry keys.

- Not all hosts will allow the tenant to add trust for the certificate that Very Good Systems uses to sign the bumped SSL.

# What does it do?

.NET certificate validation is left intact.  The library adds a specific exception to the validation: trust for the certificate given in config.  If the certificate is not set in config, it must be trusted by the host.

# Integration

- Add VeryGoodProxy NuGet library 

- Set App.config settings for `VeryGoodProxyUrl` and (optionally) `VeryGoodProxyCertificate`. These are obtained from the Very Good Security web portal.

- Adding trust for the Very Good Systems certificate on the host is optional.  

- Call VeryGoodSecurity.Init().  This step is only necessary in non-web applications.  It sets the global HTTP web proxy for the .NET app ([WebRequest.DefaultWebProxy docs](https://docs.microsoft.com/en-us/dotnet/api/system.net.webrequest.defaultwebproxy?view=netframework-4.7.2)).

# Very Good Systems Inbound Route Configuration

![Route Config](https://raw.githubusercontent.com/lebowitz/VeryGoodProxy/master/VeryGoodProxy/inboundroute.bmp "Route Config")
![Filter Config](https://raw.githubusercontent.com/lebowitz/VeryGoodProxy/master/VeryGoodProxy/filters.bmp "Filter Config")

# ExampleWinForm

The example WinForm shows how adding the VeryGoodProxy package and calling VeryGoodSystems.Init() is all that is required to proxy requests through an Inbound VGS route.  
