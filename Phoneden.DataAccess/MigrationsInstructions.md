# How to create a new migrations:

## Package Manager Console

Make sure correct settings exist in appsettings.Development.json and set Phoneden.Web as Start Up Project

In "Package Manager Console" type:

`$env:ASPNETCORE_ENVIRONMENT='Development'`

Or what ever your target environment is, then in **Package Manager Console** type:

`Add-Migration InitialCreate -project Phoneden.DataAccess -startupproject Phoneden.Web`

## .NET Core CLI

In Bash, you run: export ASPNETCORE_ENVIRONMENT=Development

To add a migration using the CLI, simply run the following, whilst changing what you need.

`dotnet ef migrations add InitialCreate --project Phoneden.DataAccess --startup-project Phoneden.Web`

# How to create the database according to latest migrations

## Package Manager Console

To update your database. In "Package Manager Console" type:

`Update-Database -project Phoneden.DataAccess -startupproject Phoneden.Web`

## .NET Core CLI

Navigate to the project root directory, and run:

`dotnet ef database update --project Phoneden.DataAccess --startup-project Phoneden.Web`

# How to generate SQL Scripts

## Package Manager Console

Run the following command:

`Script-Migration -o phoneden-scripts.sql`
