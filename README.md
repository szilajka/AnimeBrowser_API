[![Board Status](https://dev.azure.com/szilajka/47dca916-c248-4d1f-a169-8f30c47632c7/3c5a13b8-4b1f-4824-84ce-ce97ae64c5af/_apis/work/boardbadge/fee580ae-410c-40f3-950b-921a48b95a90?columnOptions=1)](https://dev.azure.com/szilajka/47dca916-c248-4d1f-a169-8f30c47632c7/_boards/board/t/3c5a13b8-4b1f-4824-84ce-ce97ae64c5af/Microsoft.RequirementCategory/)
# AnimeBrowser API
A REST API written in C#. This project is about animes, now there's no plan to add mangas.

This project uses Identity Server 4 with [Skoruba Identity Server UI](https://github.com/skoruba/IdentityServer4.Admin).
In the repo there is no description about how to setup Identity Server. The options that connects Identity Server 4 and this project are described in [this section](./docs/Appsettings.md);

## Prerequisites
- [PostgreSQL 13](https://www.postgresql.org/download/)
- [.NET 5 (5.0.3)](https://dotnet.microsoft.com/download/dotnet/5.0)
- Identity Server

## Installation
You download the latest release version or clone this repo.
If you don't have the database created, go to the [`scripts`](./scripts) folder, then check the [`create_anime_browser_db.sql`](./scripts/create_anime_browser_db.sql) file and run (you can modify it if you want to use other database or you want to make modifications in the code too).

After that, in the main folder restore nuget packages via Visual Studio 2019 or via dotnet CLI:

`dotnet restore`

In `appsettings.{Environment}.json` go to the (where `Environment` could be Development, Production or anything you put there):
- `ConnectionStrings` section
    - Change the connection string to your connection string, where your database can be found.
- `IdentityServerSettings` section
    - In that, change the `AuthorityUrl` address to your authority address (to your Identity Server address or to your other provider's address).
    - Then if you have to, change `ValidAudiences` to your audiences those are created in your Identity Server.
- `Serilog` section
    - Setup your own serilog if you need it.
    - There are two sinks (subloggers). One writes everything, and one only writes errors.

Build your project in Visual Studio or via dotnet CLI:

`dotnet build`

If the build succeeded, you can run the tests and view generated coverage reports.
I already wrote a PowerShell script to run tests and generate the reports that will be in the `test\coveragereport` folder.
You only need to run the [`run_tests_and_generate_report.ps1`](./run_tests_and_generate_report.ps1) file in powershell.

`.\run_tests_and_generate_report.ps1`

You can run the project by running in Visual Studio or with dotnet CLI:

`dotnet run --project .\src\AnimeBrowser.API\AnimeBrowser.API.csproj --verbosity n`

With the `--verbosity n` command you specify that the verbosity level is `normal` (if you miss it, in my case, you will only see `Building...` and no comments if it finished.)

Then you can go to `https://localhost:44386/swagger/index.html` or other ports (that you defined) to see if it really runs.