# Examples

The ExampleWinForm app shows how to integrate:

- adding the VeryGoodProxy NuGet library 
- set App.config settings for `VeryGoodProxyUrl` and `VeryGoodProxyCertificate`. These are obtained from the Very Good Security web portal.
- Call VeryGoodSecurity.Init().  This step is only necessary in non-web applications.  It sets a default proxy for the app.

No changes need to be made to the certificate store.  The library creates root trust for the certificate provided in configuration.  
If the certificate is not provided in config, it must exist in the windows certificate store.
