#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
RUN apt-get update && apt-get install -y --no-install-recommends libldap-2.4-2
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY NuGet.Config ./
COPY ["*.props", "../"]
COPY *.props ../
COPY ["src/Mre.Sb.Base.HttpApi.Host/Mre.Sb.Base.HttpApi.Host.csproj", "./Mre.Sb.Base.HttpApi.Host/"]
COPY ["src/Mre.Sb.Base.HttpApi.Host/NuGet.Config", "./Mre.Sb.Base.HttpApi.Host/"]
COPY ["src/Mre.Sb.AuditoriaConf.HttpApi/Mre.Sb.AuditoriaConf.HttpApi.csproj", "./Mre.Sb.AuditoriaConf.HttpApi/"]
COPY ["src/Mre.Sb.AuditoriaConf.Application.Contracts/Mre.Sb.AuditoriaConf.Application.Contracts.csproj", "./Mre.Sb.AuditoriaConf.Application.Contracts/"]
COPY ["src/Mre.Sb.AuditoriaConf.Domain.Shared/Mre.Sb.AuditoriaConf.Domain.Shared.csproj", "./Mre.Sb.AuditoriaConf.Domain.Shared/"]
COPY ["src/Mre.Sb.AuditoriaConf.EntityFrameworkCore/Mre.Sb.AuditoriaConf.EntityFrameworkCore.csproj", "./Mre.Sb.AuditoriaConf.EntityFrameworkCore/"]
COPY ["src/Mre.Sb.AuditoriaConf.Domain/Mre.Sb.AuditoriaConf.Domain.csproj", "./Mre.Sb.AuditoriaConf.Domain/"]
COPY ["src/Mre.Sb.Base.Application/Mre.Sb.Base.Application.csproj", "./Mre.Sb.Base.Application/"]
COPY ["src/Mre.Sb.Base.Application.Contracts/Mre.Sb.Base.Application.Contracts.csproj", "./Mre.Sb.Base.Application.Contracts/"]
COPY ["src/Volo.Abp.Account.Application.Contracts/Volo.Abp.Account.Application.Contracts.csproj", "./Volo.Abp.Account.Application.Contracts/"]
COPY ["src/Volo.Abp.Identity.Application.Contracts/Volo.Abp.Identity.Application.Contracts.csproj", "./Volo.Abp.Identity.Application.Contracts/"]
COPY ["src/Volo.Abp.Identity.Domain.Shared/Volo.Abp.Identity.Domain.Shared.csproj", "./Volo.Abp.Identity.Domain.Shared/"]
COPY ["src/Mre.Sb.Base.Domain.Shared/Mre.Sb.Base.Domain.Shared.csproj", "./Mre.Sb.Base.Domain.Shared/"]
COPY ["src/Volo.Abp.Identity.Application/Volo.Abp.Identity.Application.csproj", "./Volo.Abp.Identity.Application/"]
COPY ["src/Volo.Abp.Identity.Domain/Volo.Abp.Identity.Domain.csproj", "./Volo.Abp.Identity.Domain/"]
COPY ["src/Mre.Sb.Base.Domain/Mre.Sb.Base.Domain.csproj", "./Mre.Sb.Base.Domain/"]
COPY ["src/Mre.Sb.Ldap.Application.Contracts/Mre.Sb.Ldap.Application.Contracts.csproj", "./Mre.Sb.Ldap.Application.Contracts/"]
COPY ["src/Mre.Sb.Notification.HttpApi.Client/Mre.Sb.Notification.HttpApi.Client.csproj", "./Mre.Sb.Notification.HttpApi.Client/"]
COPY ["src/Volo.Abp.Account.Application/Volo.Abp.Account.Application.csproj", "./Volo.Abp.Account.Application/"]
COPY ["src/Mre.Sb.AuditoriaConf.Application/Mre.Sb.AuditoriaConf.Application.csproj", "./Mre.Sb.AuditoriaConf.Application/"]
COPY ["src/Mre.Sb.Base.EntityFrameworkCore/Mre.Sb.Base.EntityFrameworkCore.csproj", "./Mre.Sb.Base.EntityFrameworkCore/"]
COPY ["src/Volo.Abp.Identity.EntityFrameworkCore/Volo.Abp.Identity.EntityFrameworkCore.csproj", "./Volo.Abp.Identity.EntityFrameworkCore/"]
COPY ["src/Mre.Sb.Cache/Mre.Sb.Cache.csproj", "./Mre.Sb.Cache/"]
COPY ["src/Mre.Sb.Ldap.Application/Mre.Sb.Ldap.Application.csproj", "./Mre.Sb.Ldap.Application/"]
COPY ["src/Mre.Sb.Base.HttpApi/Mre.Sb.Base.HttpApi.csproj", "./Mre.Sb.Base.HttpApi/"]
COPY ["src/Volo.Abp.Identity.HttpApi/Volo.Abp.Identity.HttpApi.csproj", "./Volo.Abp.Identity.HttpApi/"]
COPY ["src/Volo.Abp.Account.HttpApi/Volo.Abp.Account.HttpApi.csproj", "./Volo.Abp.Account.HttpApi/"]
RUN dotnet restore --configfile NuGet.Config "Mre.Sb.Base.HttpApi.Host/Mre.Sb.Base.HttpApi.Host.csproj"

COPY ["src/Mre.Sb.Base.HttpApi.Host", "./Mre.Sb.Base.HttpApi.Host/"]
COPY ["src/Mre.Sb.Base.HttpApi.Host", "./Mre.Sb.Base.HttpApi.Host/"]
COPY ["src/Mre.Sb.AuditoriaConf.HttpApi", "./Mre.Sb.AuditoriaConf.HttpApi/"]
COPY ["src/Mre.Sb.AuditoriaConf.Application.Contracts", "./Mre.Sb.AuditoriaConf.Application.Contracts/"]
COPY ["src/Mre.Sb.AuditoriaConf.Domain.Shared", "./Mre.Sb.AuditoriaConf.Domain.Shared/"]
COPY ["src/Mre.Sb.AuditoriaConf.EntityFrameworkCore", "./Mre.Sb.AuditoriaConf.EntityFrameworkCore/"]
COPY ["src/Mre.Sb.AuditoriaConf.Domain", "./Mre.Sb.AuditoriaConf.Domain/"]
COPY ["src/Mre.Sb.Base.Application", "./Mre.Sb.Base.Application/"]
COPY ["src/Mre.Sb.Base.Application.Contracts", "./Mre.Sb.Base.Application.Contracts/"]
COPY ["src/Volo.Abp.Account.Application.Contracts", "./Volo.Abp.Account.Application.Contracts/"]
COPY ["src/Volo.Abp.Identity.Application.Contracts", "./Volo.Abp.Identity.Application.Contracts/"]
COPY ["src/Volo.Abp.Identity.Domain.Shared", "./Volo.Abp.Identity.Domain.Shared/"]
COPY ["src/Mre.Sb.Base.Domain.Shared", "./Mre.Sb.Base.Domain.Shared/"]
COPY ["src/Volo.Abp.Identity.Application", "./Volo.Abp.Identity.Application/"]
COPY ["src/Volo.Abp.Identity.Domain", "./Volo.Abp.Identity.Domain/"]
COPY ["src/Mre.Sb.Base.Domain", "./Mre.Sb.Base.Domain/"]
COPY ["src/Mre.Sb.Ldap.Application.Contracts", "./Mre.Sb.Ldap.Application.Contracts/"]
COPY ["src/Mre.Sb.Notification.HttpApi.Client", "./Mre.Sb.Notification.HttpApi.Client/"]
COPY ["src/Volo.Abp.Account.Application", "./Volo.Abp.Account.Application/"]
COPY ["src/Mre.Sb.AuditoriaConf.Application", "./Mre.Sb.AuditoriaConf.Application/"]
COPY ["src/Mre.Sb.Base.EntityFrameworkCore", "./Mre.Sb.Base.EntityFrameworkCore/"]
COPY ["src/Volo.Abp.Identity.EntityFrameworkCore", "./Volo.Abp.Identity.EntityFrameworkCore/"]
COPY ["src/Mre.Sb.Cache", "./Mre.Sb.Cache/"]
COPY ["src/Mre.Sb.Ldap.Application", "./Mre.Sb.Ldap.Application/"]
COPY ["src/Mre.Sb.Base.HttpApi", "./Mre.Sb.Base.HttpApi/"]
COPY ["src/Volo.Abp.Identity.HttpApi", "./Volo.Abp.Identity.HttpApi/"]
COPY ["src/Volo.Abp.Account.HttpApi", "./Volo.Abp.Account.HttpApi/"]
RUN dotnet build "Mre.Sb.Base.HttpApi.Host/Mre.Sb.Base.HttpApi.Host.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Mre.Sb.Base.HttpApi.Host/Mre.Sb.Base.HttpApi.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mre.Sb.Base.HttpApi.Host.dll"]