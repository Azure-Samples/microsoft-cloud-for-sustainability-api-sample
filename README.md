# Microsoft Cloud for Sustainability (MCfS) API SDK & Client Sample

A repository containing an SDK for the MCfS API (Preview) and 
a function app client sample that consumes it through the SDK.

## Features

This project provides the following features:

* SDK written in C# for the MCfS API, which is currently in preview
* HTTP-Trigger based C# function app client sample that consumes the MCfS API through the SDK.

## Getting Started

### Prerequisites

To setup the demo code in this repo, it requires the following prerequisites:

- Gain access to the MCfS API (details for that below)

### Play with the MCfS API through its API Management Developer Portal

If your organization would like to gain access to the MCfS API, you should first [submit this sign up form](https://forms.office.com/r/4wWTvaLzkP). 

#### Sign in and create an instance

After you receive access to the MCfS APIs from Microsoft, sign in to the [MCfS API Portal](http://home.mcfs.microsoft.com/) 
using your Azure Active Directory (Azure AD) credentials.

You must create an instance to access the MCfS APIs. By creating the instance, you become the administrator of that instance. 
The administrator of the instance can add [users, groups, and applications](https://learn.microsoft.com/en-us/industry/sustainability/api-overview#assign-permissions) to the instance.

#### Configure data sources and enable the APIs

To successfully enable access to your organization's data, 
[this step must be performed by a billing account administrator with a role as billing account reader/contributor/owner on your Azure billing account](https://learn.microsoft.com/en-us/industry/sustainability/api-overview#configure-data-sources-and-enable-the-apis).

On the Data Sources tab, toggle the connection to `on` to connect Azure emissions data. Ensure the status is listed as `Available` before using the APIs.
 
Select the `API Management` tab, and then select `Enable`. Enabling the API generates primary and secondary API keys for your instance to use in API requests.
 
#### Try the APIs

On the `API Management` tab, click on the `Try API` button. 
A developer portal will open in a new tab where you can explore the request and response schemas and make live requests against the MCfS APIs. 
To make a live request, select `Try it`. Enter all required fields:
- **Authorization**: Automatically populated authorization token
- **Subscription key**: Automatically populated with the API key from the API Management tab
- **enrollmentId**: Your enrollment ID, also known as billing account ID (for returning mock data you can use “demodata”)
- **instanceId**: Found in the URL of for the MCfS API portal
 
Add all necessary query parameters. Scroll to the bottom of the side pane and select `Send`. 
The HTTP response will be displayed at the bottom of the pane.

### Develop a sample client that consumes the MCfS API

#### Prerequisites

In this repo, we examine how you can build a simple HTTP-trigger based Azure Function App that calls the MCfS API using a custom MCfS consumer library. 
All code samples are written in C# using NET 6.

The process is pretty much the same as before and you must follow the exact same steps as presented above. 
Apart from those, a few extra steps are required, to make our sample code able to successfully consume the MCfS API.

First, you must have sufficient permission to register an application with your Azure AD tenant and assign to the application a role in your Azure subscription. 
[Confirm you have sufficient permissions](https://learn.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal), 
or work with your administrator to create a service principal. 
You will use this service principal to give your Function App an identity with the correct permissions to be able to access the MCfS API. 
If you do have permissions, then go to your Azure Active Directory and click on `App registrations` and then on `+ New registration`. 
 
In the new registration page, give a name for your app registration (e.g., `mcfs-client` in our case), and click on `Register`. 
You can leave the other options as the default ones.
 
After the app registration creation process is complete, you will be redirected to your app registration page. 
Then, click on the `Certificates & secrets` blade and then on `+ New client secret`. 
You give your secret a name and when you want it to expire and then click on `Add`. 
After creation, make sure you grab and save the client secret value, as you will not be able to see it again.
 
You will use the client id and the client secret of this app registration you just created in the Function App code, 
to assign your Function App this service principal identity when trying to communicate with the MCfS API. 
For the client id, you can go into the `Overview` blade of your app registration page and grab the value of the `Application (client) ID` property.
 
After having created the app registration and client secret that will be used by our Function App sample code, 
then you should [install the Az PowerShell module](https://learn.microsoft.com/en-us/powershell/azure/install-az-ps) 
and connect to your Azure account using the [Connect-AzAccount cmdlet](https://learn.microsoft.com/en-us/powershell/module/az.accounts/connect-azaccount).

After having authenticated, the next step would be to [create an Azure service principal using Azure PowerShell](https://learn.microsoft.com/en-us/powershell/azure/create-azure-service-principal-azureps?view=azps-9.1.0), 
with the **New-AzADServicePrincipal** command. This service principal will actually be the instance of the MCfS API app registration in your own tenant. 

To do that, you can run the following Azure PowerShell command:
```
New-AzADServicePrincipal -ApplicationId 'c3163bf1-092f-436b-b260-7ade5973e5b9'
```
To confirm the above command completed successfully, you can run the following command: 
```
Get-AzADServicePrincipal -ApplicationId 'c3163bf1-092f-436b-b260-7ade5973e5b9'
```

What is this ```-ApplicationId 'c3163bf1-092f-436b-b260-7ade5973e5b9'```?

Well, if you followed the steps of the previous section and successfully sent a request to any endpoint that the MCfS API exposes, 
now feel free to grab the access token that was sent in the `Authorization` header of your request.
 
If you go ahead and decode it in [jwt.ms](https://jwt.ms/), you will see an “aud” property. 
This is the [audience](https://www.rfc-editor.org/rfc/rfc7519#section-4.1.3) claim of the access token, 
which identifies the recipients that the JWT is intended for. 
In our case, this matches the `-ApplicationId` property in the `New-AzADServicePrincipal` command you ran previously. 
So, if you now go to the Azure Active Directory page and you click on the `Enterprise applications` blade, 
you choose `Application type` to be `Microsoft Applications` and you search by application name by typing `MCFS`, 
you shall that the `-ApplicationId` parameter you passed in the `New-AzADServicePrincipal` command above is actually 
the application id of the MCfS API and with that you created an instance (enterprise application) of the MCfS API App Registration to your own tenant. 
With that in place, you can now go to your app registration (`mfcs-client`) page and click on `API permissions` blade. 
To add permission for your app registration to be able to call the MCfS API, you can click on `+ Add a permission`, 
then click on the `APIs my organization uses` option. 
Then, you search with the application id of the MCfS API (`c3163bf1-092f-436b-b260-7ade5973e5b9`), 
you click on the `MCFS SDS` API that will pop up and you select the `App.Emissions.Read` permission of the `Application permissions` blade.
 
You will now notice that this Application type permission needs to be given admin consent 
and you do that by clicking on the `Grant admin consent for {your-tenant}`.
 
The last step you need to do is go the [MCFS Home page](http://home.mcfs.microsoft.com/),
navigate to the `Permissions` blade, then click on `+ Add`. 
In the side panel that will appear you choose `Viewer` as the `Role` and in the `User, Group or Application` input 
you search for your application registration (`mcfs-client`) and you click on `Save`.

#### Architecture

![Here, we are seeing the architecture for our Function App Client.](/MCfS-API-Client-Architecture.png)

#### Setup

Now let’s turn our attention to the sample Azure Function App code. 

Go ahead and download / clone the sample code from this GitHub repo and open it in Visual Studio.
 
The `Azure.CfS.Library` project is the consumer library that acts like a sort of SDK for the MCfS API. 
The `Fta.CfsSample.Api` project is the sample HTTP-Trigger based Azure Function App that uses the `Azure.CfS.Library` and acts as a client of the MCfS API. 
To accomplish this, it needs:
- To be associated with the service principal identity you created in Azure AD (“mcfs-client” in our example).
- Be able to grab a `client-credentials` token from Azure AD and use it to call the MCfS API:
  - Remember that the `mcfs-client` service principal has been given permissions to access the MCfS API that you registered in your tenant with the `New-AzADServicePrincipal` command.

The Azure.Cfs.Library makes use of the `Microsoft.Identity.Client` nuget package, 
which contains the binaries of the [Microsoft Authentication Library](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet) 
for .NET (MSAL.NET), it abstracts away the processes of grabbing the `client-credentials` token, 
uses that to call the MCfS API properly and process its responses. To run the sample app locally, 
you will just have to do the following:
- Create a local.settings.json file inside the Fta.CfsSample.Api project and put inside it the following values:
  - **CfsApi:PrimaryKey**: Your CfS API Primary Key from the MCfS API Management Portal
  - **AzureAd:TenantId**: Your Azure AD tenant id, which you signed up to enable the MCfS API
  - **AzureAd:ClientId**: The client id of the service principal (app registration) you registered in your Azure AD tenant (`mcfs-client` in our example)
  - **AzureAd:ClientSecret**: The client secret you created earlier in the process for the service principal (app registration) you registered in your Azure AD tenant (`mcfs-client` in our example)

```

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "CfsApi": {
    "PrimaryKey": "<your-cfs-api-primary-key-from-cfs-management-portal>"
  },
  "AzureAd": {
    "TenantId": "<your-azure-ad-tenant-id>",
    "ClientId": "<your-azure-ad-registered-sp-client-id>",
    "ClientSecret": "<your-azure-ad-registered-sp-client-secret>"
  }
}

```

With those values in place, you are now ready to run the Function App. After doing so, you can see the endpoints that the sample API exposes.
 
You can now open a tool like Postman and start sending HTTP requests to your Function App. 
Your sample code will eventually use the MCfS Library, which will use its identity to grab a `client-credentials` token
from Azure AD and will use this token to call the MCfS API and get back the results.

[Video containing the use case demo](https://clipchamp.com/watch/OMk15KMKTPx)