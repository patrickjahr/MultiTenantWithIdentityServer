# SampleMultiTenantIdentityServer
This sample will show how you can use multi tenant architecture with identity server 4.
First of all what ist multi tenant?
- A tenant is responsible for your "group" and identitfy them. For example you have a web client for different departments. For every department is configured a tenant which manage the data, assets and so on. So you can seperate the data for the different departments with the different tenants.

Setup
* Tools for IdSrv and Sample Service:
    * Postgres [(Download Postgres here)](https://www.postgresql.org/download/)
    * .NET Core SDK >= 2 [(Download SDK here)](https://www.microsoft.com/net/download/dotnet-core/2.1)
* Tools for Web Client
    * Node.js [(Download Node.js here)](https://nodejs.org/en/download/)

If you want to setup Azure AD you need to add the client id in the app settings of the IdentityServer

```
"AzureAd":{
    "ClientId":"xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", --> set here the client id
    "Authority":"https://login.microsoftonline.com/common",
    "CallbackPath":"/signin-oidc"
},
```

...more information will coming soon