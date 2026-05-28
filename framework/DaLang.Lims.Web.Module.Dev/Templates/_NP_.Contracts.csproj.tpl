@{
    var gen = Model as DaLang.Lims.Web.Dev.Domain.CodeGen.CodeGenEntity;
    if (gen == null) return;
    var moduleNamePc = gen.ApiAreaName?.NamingPascalCase();
}
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <DocumentationFile>bin\$(MSBuildProjectName).xml</DocumentationFile>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\..\framework\DaLang.Lims.Web.Framework\DaLang.Lims.Web.Framework.csproj" />
  </ItemGroup>
</Project>
