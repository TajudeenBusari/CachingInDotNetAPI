# CachingInDotNetAPI
This is a simple API that demonstrates how to cache data in a .NET Core API using the in-memory cache, Redis cache, and SQL Server cache.
In this project, the database credentials are being stored in the appsettings.json file. This is not a good practice. 
In a production environment, you should store your database credentials in a secure location such as 
    -Azure Key Vault.
    -AWS Secrets Manager.
    -Google Cloud Secret Manager.
    -Environment variables.
    -User Secrets.
To use Environment variables:
1. Windows:
    -Open the command prompt.
    -Run the following command to set the environment variable:
        setx "ConnectionStrings:DefaultConnection" "Server=ServerName;Database=DatabaseName;User Id=UserName;Password=Password"
    -Restart Visual Studio.
OR
    -setx DB_USERNAME = "UserName"
    -setx DB_PASSWORD = "Password"
    -MODIFY THE CONNECTION STRING IN THE APPSETTINGS.JSON FILE AS FOLLOWS:
        "ConnectionStrings": {
            "DefaultConnection": "Server=ServerName;Database=DatabaseName;Username=${DB_USERNAME};Password=${DB_PASSWORD}"
    }
    -Read the environment variable in the Startup.cs file as follows:
        var username = Environment.GetEnvironmentVariable("DB_USERNAME");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
        var connectionString = Configuration.GetConnectionString("DefaultConnection").Replace("${DB_USERNAME}", username).Replace("${DB_PASSWORD}", password);'
2. Using user secrets:
    -Right-click on the project and select Manage User Secrets.
    -Add the following code to the secrets.json file:
        {
            "ConnectionStrings:DefaultConnection": "Server=ServerName;Database=DatabaseName;Username=UserName;Password=Password"
        }
    -Read the user secret in the Startup.cs file as follows:
        var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
OR
    - run this command:
    - dotnet user-secrets init
    - set the secret:
        dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=ServerName;Database=DatabaseName;Username=UserName;Password=Password"
    - In .Net, the secret will override the appsettings.json file.
   
3. Using Azure Key Vault:
    -Create an Azure Key Vault.
    -Add the database credentials to the Azure Key Vault.
    -Grant the API access to the Azure Key Vault.
    -Read the database credentials from the Azure Key Vault in the Startup.cs file.
