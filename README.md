A sample project combines:

1. asp.net identity with identityserver4 https://identityserver4.readthedocs.io/en/latest/quickstarts/6_aspnet_identity.html
2. efcore(sqlserver) https://identityserver4.readthedocs.io/en/latest/quickstarts/5_entityframework.html
3. identityserver with local webapi  https://identityserver4.readthedocs.io/en/latest/topics/add_apis.html
4. scoped authorize policy https://github.com/IdentityServer/IdentityServer4.AccessTokenValidation  https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies


## how to use

```
cd src\IdentityServerAspNetIdentity
dotnet run /seed
```

use postman import  `doc/postman_collection.json` 



## how to recreate this project

gen cert:

```
openssl req -newkey rsa:2048 -nodes -keyout key.pem -x509 -days 3650 -out cert.pem
openssl pkcs12 -export -in cert.pem -inkey key.pem -out cert.pfx
```

cert path and pwd is configured at:

```
"Cert": {
    "PfxPath": "cert.pfx",
    "Password": "xxx"
}
```

this demo is created by is4aspid template:

```
dotnet new -i IdentityServer4.Templates
dotnet new is4aspid -n IdentityServerAspNetIdentity
dotnet add package IdentityServer4.EntityFramework
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

efcore migration:

```
dotnet tool install --global dotnet-ef --version 3.1.24
dotnet tool update --global dotnet-ef --version 3.1.24
dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet ef migrations add InitialIdentityServerIdentityDbContext -c IdentityDbContext -o Data/Migrations/IdentityDb
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb

dotnet ef database update --context IdentityDbContext
dotnet ef database update --context PersistedGrantDbContext
dotnet ef database update --context ConfigurationDbContext
```


