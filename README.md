# WCF Demo App

This basic 2 tier demo app can be used as a sample to learn how to build and deploy Azure Cloud Services for WCF & ASP.NET.

## Local Setup

* You will need a SQL server
    * Don't have one? You can create one in Azure (see *Step 3* below)
* IF using this sample solution you will need to create a database and execute the `Resources\dbo.Products.sql` script against it to populate the database
* Then you will need to rename `DemoWCFService\ConnectionStrings - Sample.config` to `DemoWCFService\ConnectionStrings.config` and configure it appropriately for your database connection
* However you chose to host your local WCF service you will need to edit `DemoWebApp\web.config` and set the correct client endpoint.

## Testing

* If all goes well you should be able to run both the WCF service & the web app and see a listing of 3 products.

## Converting to Azure Cloud Services

### Prerequisites

1. Download and install the appropriate .NET SDK (~ 280MB)
    * [Visual Studio 2015](http://go.microsoft.com/fwlink/?linkid=518003&clcid=0x409)
    * [Visual Studio 2013](http://go.microsoft.com/fwlink/?linkid=323510&clcid=0x409)
1. Create an Azure subscription (trial, MSDN, pay as you go)

### Step 1: Define a resource group

> Resource groups are a way to group resources related to a specific application for easier management, reporting and deployment. Learn more about [Resource Groups](https://azure.microsoft.com/en-us/documentation/articles/resource-group-portal)

1. Create a resource group in the [Preview Portal](https://portal.azure.com)
1. Click `+ New` > `Management` > `Resource Group`
1. Enter a `Resource group name` for the group such as `WCFDemoApplication`
    * This resource group will contain all related Azure resources (databases & web roles) for your deployment and provide a single place to view/manage them.
1. Select the `subscription` to deploy into or keep the default if there is only one Azure Subscription
1. Chose your `Resource group location` which best matches where you wish to physically locate your services.
    * *Tip:* You should record this location and ensure `all` related resources are in the same location for performance (except of course for geo-redundancy/disaster recovery)
1. Click `Create`

### Step 2: Create a storage account

> A Storage Account gives you access to Azure Blob, Queue, Table, File Services and more in Azure. It's a good idea to create one account per resource group to contain all related storage requirments. Learn more about [Azure Storage Accounts](https://azure.microsoft.com/en-us/documentation/articles/storage-create-storage-account/)

1. Create a Storage Account in the [Preview Portal](https://portal.azure.com) in the resource group created above
1. Click `+ New` > `Data + Storage` + `Storage Account`
1. Click `Create`
1. Provide a `Name` for this account such as *"wcfdemostorage"*
    * Note this name must be globaly unique
1. Select `Pricing Tier` and switch to `Geo-Redundant` & click `Select`
1. For `Resource Group` select the existing resource group you create above (in step 1).
1. Click `location` should match that of your storage group by default
1. Click `Create`

### Step 3: Create a SQL Database

> Azure SQL is a relational database-as-a-service that provides you an easy and quick way to build and scale your databases. Learn more about [Azure SQL Database](https://azure.microsoft.com/en-us/services/sql-database/)

1. Create a Azure SQL Database in the [Preview Portal](https://portal.azure.com) in the resource group created above
1. Click `+ New` > `Data + Storage` + `SQL Database`
1. Enter a `name` for your database such as *"wcfdemodb"*
1. For `Server` click `Configure required settings` > `Create a new server` (unless you already have a server you want to reuse, in which case select that one
    1. Enter a `name` for your server such as *"wcfdemoserver"*
    1. Enter a `Server admin login` account name
        * *Tip:* You cannot use names like sa, admin, administrator, root, dba etc.
    1. Enter a `Password` & `Confirm Password`
    1. Select a **location** for your server
        * *Tip:* this should match your resource group's location
1. Click `OK`
1. Click **Pricing Tier** & select your desired level and click `Select`
    * For production servers I suggest *P2 Premium* or above.
1. Click **Resource group** & select the resource group you created in *Step 1*.
1. If you have multiple **Subscriptions** select the appropriate one
1. Click **Create**

### Step 4: Create a Cloud Service for your WCF Service (API)

> Azure Cloud Services is a PaaS (Platform-as-a-Service) offering that allows you to deploy highly-available, infinitely-scalable applications and APIs on Azure without having to focus on setting up servers (virtual machines). Learn more about [Azure Cloud Services](https://azure.microsoft.com/en-us/services/cloud-services/)

> We need to create **two** cloud services, one for the web application, and one for the WCF service. Although you could create them in the same Cloud Service using multiple web roles it's easier to seperate them.

1. Create a Cloud Service in the [Preview Portal](https://portal.azure.com) in the resource group created above
1. Click `+ New` > `Compute` > `Cloud Service` and finally **Create**
1. Enter a `DNS Name` for your cloud service
    * e.g. *"wcfdemoapp"* or *"wcfdemoapi"*
1. If you have multiple `Subscriptions` select the appropriate one
1. Click `Resource group` & select the resource group you created in *Step 1*.
1. Ensure that the `location` for your cloud service matches that of your resource group
1. Click **Create**

### Step 5: Create a Azure Cloud Services Solutions

> We will now create two new solutions, one for the WCF Service and one for the web app which will be Azure Cloud Service solutions each containing a web role. Learn more about [Web Roles & Cloud Service projects](https://azure.microsoft.com/en-gb/documentation/articles/vs-azure-tools-configure-roles-for-cloud-service/)

#### Step 5a: Create a Cloud Service Solution in Visual Studio for the WCF Application

1. Open Visual Studio (2013 or 2015)
1. Click `File` > `New` > `Project`
1. Under `Templates` click `Cloud` then `Azure Cloud Service`
1. Supply a Name for your project & for your solution
    * e.g. *"WCFCloudService"*
1. Do NOT add any roles
1. Click `OK`

> Next we tell the Cloud Service to use your existing WCF API

1. Right click on your `Solution` and click `Add` > `Existing Project`
    * Navigate to your web project `WCFDemoApp\DemoWCFService\DemoWCFService.csproj` and click on the file then click `Open`
1. In your **Cloud Service** project right click on `Roles` and select `Add` > `Web Role Project in Solution`
1. Select your **Web App** and click `OK`

> Let's test to make sure everything is working as expected

1. Build & run the solution (`CTRL` + `SHIFT` + `B` > `CTRL` + `F5`)
1. This should start the **Compute Emulator** which can locally emulate multiple web & worker roles as well as other Azure storage services.
1. To test the web roles WCF service you can use the same **WCF Test Client** as you would for regular WCF projects. For Visual Studio 2015 on Windows 10 it can be found in `Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE`
    * Simply launch `WcfTestClient.exe` and point it to your new service
    * You can find the port in the output window, or you can open IIS Express (by expanding hidden icons in your taskbar), right clicking the IIS Express icon and selecting `Show all sites`
    * be sure to add your service extenion (e.g. productsapi.svc to the end of the URL)

#### Step 5b: Configure the  Cloud Service for the WCF Application

> Here we will set instance count & VM sizes for your Azure deployment

1. In your newly created `Cloud Service` project expand the `Roles` folder and right click your `Web Role` and select `Properties`
1. For `Services Configuration` ensure that `All Configurations` is selected
1. Set `Instance Count` to 2, 3, 4 or  your preferred number of instances of the API to be load balanced automatically
    * For availability and scale this should be at least 2
1. Set `VM Size` to your preferred size (for a production server I'd recomend at least Standard_D2)
1. Change `Services Configuration` to `Cloud`
1. Under `Diagnostics` > `Specify the storage account credentials for the Diganostics results:" select `...`
    1. Change `Connect Using` to `Your Subscription`
    1. For `Account Name` select the storage account you created in *Step 3*
1. Save (`CTRL` + `S`) and close `X` the web roles properties

#### Step 5c: Publish your API Cloud Service to Azure

1. Right click on your `Cloud Service` project and select **Publish**
1. If promted either add your account or enter your credentials for Azure
1. Select the appropriate **subscription** and tap **Next >**
1. For **Cloud Service** select the service you created in **Step 3**
1. For **Environment** select the appropriate one, for initial testing **Production** is fine
1. For **Build configuration** select the appropriate one (you may want to use *Debug* if you anticipate any issues, but use *Release* for any perf/load testing)
1. For **Service Configuration** select **Cloud**
1. Tap **Next >** then **Publish**

> Note: This will deploy your cloud service with the desired number of web roles (which are your WCF services) and automatically setup a load balancer for them on the configured endpoint. Should any instances ever fail, Azure will automatically re-deploy a new one using the last published package.

>  You can use the same `WcfTestClient.exe` tool (as mentioned above) to test your new cloud hosted WCF service.


### Step 6: Create a Cloud Service for your Web App (ASP.NET MVC)

1. Create a Cloud Service in the [Preview Portal](https://portal.azure.com) in the resource group created above
1. Click `+ New` > `Compute` > `Cloud Service` and finally **Create**
1. Enter a `DNS Name` for your cloud service
    * e.g. *"webdemoapp"*
1. If you have multiple `Subscriptions` select the appropriate one
1. Click `Resource group` & select the resource group you created in *Step 1*.
1. Ensure that the `location` for your cloud service matches that of your resource group
1. Click **Create**


#### Step 6a: Create a Cloud Service Solution in Visual Studio for the Web Application

1. Open Visual Studio (2013 or 2015)
1. Click `File` > `New` > `Project`
1. Under `Templates` click `Cloud` then `Azure Cloud Service`
1. Supply a Name for your project & for your solution
    * e.g. *"WWWCloudService"*
1. Click `OK`
1. Do NOT add any roles
1. Click `OK`
1. Right click on your `Solution` and click `Add` > `Existing Project`
    * Navigate to your web project `WCFDemoApp\DemoWebApp\DemoWebApp.csproj` and click on the file then click `Open`
1. In your **Cloud Service** project right click on `Roles` and select `Add` > `Web Role Project in Solution`
1. Select your **Web App** and click `OK`

#### Step 6b: Modify your web.config

> Depending on how you manage your environments and configuration files you will need to ensure you have updated the web.config file for your web application to use the Azure hosted WCF Service and not your local one. 

> For simplicity sake we'll just edit the web apps web.config

1. Open the `web.config` file for your web application
1. Change the end `endpoint address` to point to your Azure Cloud Service enpoint for your recently deployed WCF Web Role.

#### Step 6c: Publish your Web App Cloud Service

1. Right click on your `Cloud Service` project and select **Publish**
1. If promted either add your account or enter your credentials for Azure
1. Select the appropriate **subscription** and tap **Next >**
1. For **Cloud Service** select the service you created in **Step 5** (The Cloud Service for your web app, not your WCF API)
1. For **Environment** select the appropriate one, for initial testing **Production** is fine
1. For **Build configuration** select the appropriate one (you may want to use *Debug* if you anticipate any issues, but use *Release* for any perf/load testing)
1. For **Service Configuration** select **Cloud**
1. Tap **Next >** then **Publish**

> Note: This will deploy your cloud service with the desired number of web roles (which are your WCF services) and automatically setup a load balancer for them on the configured endpoint. Should any instances ever fail, Azure will automatically re-deploy a new one using the last published package.


### Step 7: Test

> You should now be able to connect to your Azure Hosted Cloud Service Web App and start testing everything!