 <img width="116" height="116" src="https://raw.githubusercontent.com/jasontaylordev/CleanArchitecture/main/.github/icon.png"  alt=""/>

 # DAD .NET5 Clean Architecture Web Api Solution Template
[![Build status](https://tfs.unitedtractors.com/DefaultCollection/Mobile%20Web%20Development/_apis/build/status/netca-CI)](https://tfs.unitedtractors.com/DefaultCollection/Mobile%20Web%20Development/_build/latest?definitionId=665)

[![DAD.Clean.Architecture.WebApi.Solution.Template package in dad-registry feed in Azure Artifacts](https://tfs.unitedtractors.com/DefaultCollection/_apis/public/Packaging/Feeds/f638be01-a0c6-4302-ae05-45ba95464364/Packages/dcd873c3-8f3b-4c96-a39d-4ad2a979a758/Badge)](https://tfs.unitedtractors.com/DefaultCollection/_Packaging?feed=f638be01-a0c6-4302-ae05-45ba95464364&package=dcd873c3-8f3b-4c96-a39d-4ad2a979a758&preferRelease=true&_a=package)
<br/>

This is a solution template for creating a ASP.NET Core following the principles of Clean Architecture. Create a new project based on this template by clicking the above **Use this template** button or by installing and running the associated NuGet package (see Getting Started for full details).

## Technologies

* ASP.NET Core 5
* [Entity Framework Core 5](https://docs.microsoft.com/en-us/ef/core/)
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [Quartz.NET](https://www.quartz-scheduler.net/)
* [Redis](https://redis.io/)
* [JsonApi](httpshttps://jsonapi.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)
* [Docker](https://www.docker.com/)

## Getting Started

The easiest way to get started is to install the [NuGet package](https://tfs.unitedtractors.com/DefaultCollection/Mobile%20Web%20Development/_packaging?feed=dad-registry%40Release&package=DAD.Clean.Architecture.WebApi.Solution.Template&version&protocolType=NuGet&_a=package) and run `dotnet new netca`:

1. Install the latest [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
2. Add packageSources & packageSourceCredentials to `Nuget.config` : 
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="ut" value="https://tfs.unitedtractors.com/DefaultCollection/_packaging/dad-registry/nuget/v3/index.json" />
  </packageSources>
  <packageSourceCredentials>
        <ut> 
            <add key="Username" value="cst_dev42" />
            <add key="ClearTextPassword" value="youre password or PAT" />
            <add key="ValidAuthenticationTypes" value="basic" /> 
        </ut>
    </packageSourceCredentials>
</configuration>
``` 
3. Run `dotnet new --install DAD.Clean.Architecture.WebApi.Solution.Template` to install the project template
4. Create a folder for your solution and cd into it (the template will use it as project name)
5. Run `dotnet new dad-ca-wa-sln` to create a new project
6. Navigate to `src/Api` and run `dotnet run` to launch the back end (ASP.NET Core Web API)

### Docker Configuration

In order to get Docker working, you will need to add a temporary SSL cert and mount a volume to hold that cert.
You can find [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-3.1) that describe the steps required for Windows, macOS, and Linux.

For Windows:
The following will need to be executed from your terminal to create a cert
`dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p Your_password123`
`dotnet dev-certs https --trust`

NOTE: When using PowerShell, replace %USERPROFILE% with $env:USERPROFILE.

FOR macOS:
`dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`
`dotnet dev-certs https --trust`

FOR Linux:
`dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p Your_password123`

In order to build and run the docker containers, execute `docker-compose -f 'docker-compose.yml' up --build` from the root of the solution where you find the docker-compose.yml file.  You can also use "Docker Compose" from Visual Studio for Debugging purposes.
Then open http://localhost:8080 on your browser.

To disable Docker in Visual Studio, right-click on the **docker-compose** file in the **Solution Explorer** and select **Unload Project**.

### Database Configuration

Verify that the **DefaultConnection** connection string within **appsettings.json** points to a valid SQL Server instance.

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations

To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/Api`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

`dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\Api --output-dir Persistence\Migrations`

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### Api

This layer is a REST API. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only Startup.cs should reference Infrastructure.

## Support

If you are having problems or having new idea, please let us know by [raising a new pr to main branch](https://tfs.unitedtractors.com/DefaultCollection/Mobile%20Web%20Development/_git/netca/pullrequests).

## Source

This project base on [JasonGT](https://github.com/JasonGT/CleanArchitecture)

## License

This project is licensed with the [Apache-2.0 license](LICENSE).