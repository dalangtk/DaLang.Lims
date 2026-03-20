#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY Directory.Build.props ./
COPY ["framework/Directory.Build.props", "framework/"]
COPY ["hosts/DaLang.Lims.Web.Host/DaLang.Lims.Web.Host.csproj", "hosts/DaLang.Lims.Web.Host/"]
COPY ["framework/DaLang.Lims.Web.Framework/DaLang.Lims.Web.Framework.csproj", "framework/DaLang.Lims.Web.Framework/"]
COPY ["framework/DaLang.Lims.Web.Common/DaLang.Lims.Web.Common.csproj", "framework/DaLang.Lims.Web.Common/"]
COPY ["framework/DaLang.Lims.Web.DynamicApi/DaLang.Lims.Web.DynamicApi.csproj", "framework/DaLang.Lims.Web.DynamicApi/"]
COPY ["framework/DaLang.Lims.Web.ApiUI/DaLang.Lims.Web.ApiUI.csproj", "framework/DaLang.Lims.Web.ApiUI/"]
COPY ["framework/DaLang.Lims.Web.Module.Dev/DaLang.Lims.Web.Dev.csproj", "framework/DaLang.Lims.Web.Module.Dev/"]
COPY ["modules/BasicData/DaLang.Lims.BaseData.csproj", "modules/BasicData/"]
COPY ["modules/Pretreatment/DaLang.Lims.Pretreatment.csproj", "modules/Pretreatment/"]
COPY ["modules/Shared/DaLang.Lims.Shared.csproj", "modules/Shared/"]
RUN dotnet restore "./hosts/DaLang.Lims.Web.Host/DaLang.Lims.Web.Host.csproj"
COPY . .
WORKDIR "/src/hosts/DaLang.Lims.Web.Host"
RUN dotnet build "./DaLang.Lims.Web.Host.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DaLang.Lims.Web.Host.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaLang.Lims.Web.Host.dll"]