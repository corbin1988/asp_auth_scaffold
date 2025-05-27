FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Auth.Core/Auth.Core.csproj", "src/Auth.Core/"]
COPY ["src/Auth.Tests/Auth.Tests.csproj", "src/Auth.Tests/"]
RUN dotnet restore "src/Auth.Core/Auth.Core.csproj"
COPY . .
WORKDIR "/src/src/Auth.Core"
RUN dotnet build "Auth.Core.csproj" -c Release -o /app/build
WORKDIR "/src/src/Auth.Tests"
RUN dotnet build "Auth.Tests.csproj" -c Release

FROM build AS test
WORKDIR "/src/src/Auth.Tests"
CMD ["dotnet", "test", "Auth.Tests.csproj", "--no-build", "--verbosity", "normal"]

FROM build AS publish
WORKDIR "/src/src/Auth.Core"
RUN dotnet publish "Auth.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auth.Core.dll"]