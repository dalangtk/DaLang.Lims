# 请参阅 https://aka.ms/customizecontainer 以了解如何自定义调试容器，以及 Visual Studio 如何使用此 Dockerfile 生成映像以更快地进行调试。

# 此阶段用于在快速模式(默认为调试配置)下从 VS 运行时
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# 此阶段用于生成服务项目
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["framework/Directory.Build.props", "framework/"]
COPY ["hosts/DaLang.Lims.Web.Host/DaLang.Lims.Web.Host.csproj", "hosts/DaLang.Lims.Web.Host/"]
COPY ["framework/DaLang.Lims.Web.Framework/DaLang.Lims.Web.Framework.csproj", "framework/DaLang.Lims.Web.Framework/"]
COPY ["framework/DaLang.Lims.Web.Common/DaLang.Lims.Web.Common.csproj", "framework/DaLang.Lims.Web.Common/"]
COPY ["framework/DaLang.Lims.Web.DynamicApi/DaLang.Lims.Web.DynamicApi.csproj", "framework/DaLang.Lims.Web.DynamicApi/"]
COPY ["framework/DaLang.Lims.Web.ApiUI/DaLang.Lims.Web.ApiUI.csproj", "framework/DaLang.Lims.Web.ApiUI/"]
COPY ["framework/DaLang.Lims.Web.Module.Dev/DaLang.Lims.Web.Dev.csproj", "framework/DaLang.Lims.Web.Module.Dev/"]
COPY ["modules/BasicData/DaLang.Lims.BaseData.Application/DaLang.Lims.BaseData.Application.csproj", "modules/BasicData/DaLang.Lims.BaseData.Application/"]
COPY ["modules/Shared/DaLang.Lims.Apply.Domain/DaLang.Lims.Shared.Domain.csproj", "modules/Shared/DaLang.Lims.Apply.Domain/"]
COPY ["modules/BasicData/DaLang.Lims.BaseData.Contracts/DaLang.Lims.BaseData.Contracts.csproj", "modules/BasicData/DaLang.Lims.BaseData.Contracts/"]
COPY ["modules/BasicData/DaLang.Lims.BaseData.Core/DaLang.Lims.BaseData.Core.csproj", "modules/BasicData/DaLang.Lims.BaseData.Core/"]
COPY ["modules/BasicData/DaLang.Lims.BaseData.Domain/DaLang.Lims.BaseData.Domain.csproj", "modules/BasicData/DaLang.Lims.BaseData.Domain/"]
COPY ["modules/BasicData/DaLang.Lims.BaseData.Sqlsugar.Mysql/DaLang.Lims.BaseData.Sqlsugar.Mysql.csproj", "modules/BasicData/DaLang.Lims.BaseData.Sqlsugar.Mysql/"]
COPY ["modules/Exam/DaLang.Lims.Exam.Application/DaLang.Lims.Exam.Application.csproj", "modules/Exam/DaLang.Lims.Exam.Application/"]
COPY ["modules/Shared/DaLang.Lims.Apply.Contracts/DaLang.Lims.Shared.Contracts.csproj", "modules/Shared/DaLang.Lims.Apply.Contracts/"]
COPY ["modules/Exam/DaLang.Lims.Exam.Contracts/DaLang.Lims.Exam.Contracts.csproj", "modules/Exam/DaLang.Lims.Exam.Contracts/"]
COPY ["modules/Exam/DaLang.Lims.Exam.Core/DaLang.Lims.Exam.Core.csproj", "modules/Exam/DaLang.Lims.Exam.Core/"]
COPY ["modules/Exam/DaLang.Lims.Exam.Domain/DaLang.Lims.Exam.Domain.csproj", "modules/Exam/DaLang.Lims.Exam.Domain/"]
COPY ["modules/Exam/DaLang.Lims.Exam.Sqlsugar.Mysql/DaLang.Lims.Exam.Sqlsugar.Mysql.csproj", "modules/Exam/DaLang.Lims.Exam.Sqlsugar.Mysql/"]
COPY ["modules/Pretreatment/DaLang.Lims.Pretreatment.Application/DaLang.Lims.Pretreatment.Application.csproj", "modules/Pretreatment/DaLang.Lims.Pretreatment.Application/"]
COPY ["modules/Pretreatment/DaLang.Lims.Pretreatment.Contracts/DaLang.Lims.Pretreatment.Contracts.csproj", "modules/Pretreatment/DaLang.Lims.Pretreatment.Contracts/"]
COPY ["modules/Pretreatment/DaLang.Lims.Pretreatment.Core/DaLang.Lims.Pretreatment.Core.csproj", "modules/Pretreatment/DaLang.Lims.Pretreatment.Core/"]
COPY ["modules/Pretreatment/DaLang.Lims.Pretreatment.Domain/DaLang.Lims.Pretreatment.Domain.csproj", "modules/Pretreatment/DaLang.Lims.Pretreatment.Domain/"]
COPY ["modules/Pretreatment/DaLang.Lims.Pretreatment.Sqlsugar.Mysql/DaLang.Lims.Pretreatment.Sqlsugar.Mysql.csproj", "modules/Pretreatment/DaLang.Lims.Pretreatment.Sqlsugar.Mysql/"]
COPY ["modules/ReportTemplate/DaLang.Lims.ReportTemplate.Application/DaLang.Lims.ReportTemplate.Application.csproj", "modules/ReportTemplate/DaLang.Lims.ReportTemplate.Application/"]
COPY ["modules/ReportTemplate/DaLang.Lims.ReportTemplate.Contracts/DaLang.Lims.ReportTemplate.Contracts.csproj", "modules/ReportTemplate/DaLang.Lims.ReportTemplate.Contracts/"]
COPY ["modules/ReportTemplate/DaLang.Lims.ReportTemplate.Core/DaLang.Lims.ReportTemplate.Core.csproj", "modules/ReportTemplate/DaLang.Lims.ReportTemplate.Core/"]
COPY ["modules/ReportTemplate/DaLang.Lims.ReportTemplate.Domain/DaLang.Lims.ReportTemplate.Domain.csproj", "modules/ReportTemplate/DaLang.Lims.ReportTemplate.Domain/"]
COPY ["modules/ReportTemplate/DaLang.Lims.ReportTemplate.Sqlsugar.Mysql/DaLang.Lims.ReportTemplate.Sqlsugar.Mysql.csproj", "modules/ReportTemplate/DaLang.Lims.ReportTemplate.Sqlsugar.Mysql/"]
COPY ["modules/Statistics/DaLang.Lims.Statistics.Application/DaLang.Lims.Statistics.Application.csproj", "modules/Statistics/DaLang.Lims.Statistics.Application/"]
COPY ["modules/Statistics/DaLang.Lims.Statistics.Contracts/DaLang.Lims.Statistics.Contracts.csproj", "modules/Statistics/DaLang.Lims.Statistics.Contracts/"]
COPY ["modules/Statistics/DaLang.Lims.Statistics.Core/DaLang.Lims.Statistics.Core.csproj", "modules/Statistics/DaLang.Lims.Statistics.Core/"]
RUN dotnet restore "./hosts/DaLang.Lims.Web.Host/DaLang.Lims.Web.Host.csproj"
COPY . .
WORKDIR "/src/hosts/DaLang.Lims.Web.Host"
RUN dotnet build "./DaLang.Lims.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 此阶段用于发布要复制到最终阶段的服务项目
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DaLang.Lims.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 此阶段在生产中使用，或在常规模式下从 VS 运行时使用(在不使用调试配置时为默认值)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaLang.Lims.Web.Host.dll"]