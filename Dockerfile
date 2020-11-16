# set base image as the dotnet 3.1 SDK.
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

# set the working directory for any RUN, CMD, ENTRYPOINT, COPY and ADD
# instructions that follows the WORKDIR instruction.
WORKDIR /app

# debug purposes - see where we are
RUN pwd

# our current working directory within the container is /app
# we now copy all the files (from local machine) to /app (in the container).
COPY . ./

# debug purposes - list files in current folder
RUN ls -la

# run unit tests within the solution.
RUN dotnet test Phoneden.sln

# again, on the container (we are in /app folder)
# we now publish the project into a folder called 'out'.
RUN dotnet publish Phoneden.Web/Phoneden.Web.csproj -c Release -o out

# debug purposes - list files in current folder
RUN ls -la

# set base image as the dotnet 3.1 runtime.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

# telling the application what port to run on.
ENV ASPNETCORE_URLS=http://*:5005

# set the working directory for any RUN, CMD, ENTRYPOINT, COPY and ADD
# instructions that follows the WORKDIR instruction.
WORKDIR /app

# copy the contents of /app/out in the `build-env` and paste it in the
# `/app` directory of the new runtime container.
COPY --from=build-env /app/out .

# set the entry point into the application.
ENTRYPOINT ["dotnet", "Phoneden.Web.dll", "-seed"]