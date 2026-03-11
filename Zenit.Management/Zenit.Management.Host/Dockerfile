FROM mcr.microsoft.com/dotnet/sdk:10.0 as builder

WORKDIR /src

# Restore dependencies as distinct layers
COPY ["Zenit.Share/Zenit.Share.Common/Zenit.Share.Common.csproj", "Zenit.Share/Zenit.Share.Common/"]
RUN dotnet restore "Zenit.Share/Zenit.Share.Common"
COPY ["Zenit.Share/Zenit.Share.Contract/Zenit.Share.Contract.csproj", "Zenit.Share/Zenit.Share.Contract/"]
RUN dotnet restore "Zenit.Share/Zenit.Share.Contract"
COPY ["Zenit.Share/Zenit.Share.Data/Zenit.Share.Data.csproj", "Zenit.Share/Zenit.Share.Data/"]
RUN dotnet restore "Zenit.Share/Zenit.Share.Data"
COPY ["Zenit.Share/Zenit.Share.Business/Zenit.Share.Business.csproj", "Zenit.Share/Zenit.Share.Business/"]
RUN dotnet restore "Zenit.Share/Zenit.Share.Business"
COPY ["Zenit.Share/Zenit.Share.Host/Zenit.Share.Host.csproj", "Zenit.Share/Zenit.Share.Host/"]
RUN dotnet restore "Zenit.Share/Zenit.Share.Host"

COPY ["Zenit.Management/Zenit.Management.Common/Zenit.Management.Common.csproj", "Zenit.Management/Zenit.Management.Common/"]
RUN dotnet restore "Zenit.Management/Zenit.Management.Common"
COPY ["Zenit.Management/Zenit.Management.Contract/Zenit.Management.Contract.csproj", "Zenit.Management/Zenit.Management.Contract/"]
RUN dotnet restore "Zenit.Management/Zenit.Management.Contract"
COPY ["Zenit.Management/Zenit.Management.Data/Zenit.Management.Data.csproj", "Zenit.Management/Zenit.Management.Data/"]
RUN dotnet restore "Zenit.Management/Zenit.Management.Data"
COPY ["Zenit.Management/Zenit.Management.Business/Zenit.Management.Business.csproj", "Zenit.Management/Zenit.Management.Business/"]
RUN dotnet restore "Zenit.Management/Zenit.Management.Business"
COPY ["Zenit.Management/Zenit.Management.Host/Zenit.Management.Host.csproj", "Zenit.Management/Zenit.Management.Host/"]
RUN dotnet restore "Zenit.Management/Zenit.Management.Host"

# Copy the entire project and build
COPY ["Zenit.Share/Zenit.Share.Common/", "Zenit.Share/Zenit.Share.Common/"]
RUN dotnet build "Zenit.Share/Zenit.Share.Common/Zenit.Share.Common.csproj" -c Release
COPY ["Zenit.Share/Zenit.Share.Data/", "Zenit.Share/Zenit.Share.Data/"]
RUN dotnet build "Zenit.Share/Zenit.Share.Data/Zenit.Share.Data.csproj" -c Release
COPY ["Zenit.Share/Zenit.Share.Contract/", "Zenit.Share/Zenit.Share.Contract/"]
RUN dotnet build "Zenit.Share/Zenit.Share.Contract/Zenit.Share.Contract.csproj" -c Release
COPY ["Zenit.Share/Zenit.Share.Business/", "Zenit.Share/Zenit.Share.Business/"]
RUN dotnet build "Zenit.Share/Zenit.Share.Business/Zenit.Share.Business.csproj" -c Release

COPY ["Zenit.Share/Zenit.Share.Host/", "Zenit.Share/Zenit.Share.Host/"]
RUN dotnet build "Zenit.Share/Zenit.Share.Host/Zenit.Share.Host.csproj" -c Release

COPY ["Zenit.Management/Zenit.Management.Common/", "Zenit.Management/Zenit.Management.Common/"]
RUN dotnet build "Zenit.Management/Zenit.Management.Common/Zenit.Management.Common.csproj" -c Release
COPY ["Zenit.Management/Zenit.Management.Data/", "Zenit.Management/Zenit.Management.Data/"]
RUN dotnet build "Zenit.Management/Zenit.Management.Data/Zenit.Management.Data.csproj" -c Release
COPY ["Zenit.Management/Zenit.Management.Contract/", "Zenit.Management/Zenit.Management.Contract/"]
RUN dotnet build "Zenit.Management/Zenit.Management.Contract/Zenit.Management.Contract.csproj" -c Release
COPY ["Zenit.Management/Zenit.Management.Business/", "Zenit.Management/Zenit.Management.Business/"]
RUN dotnet build "Zenit.Management/Zenit.Management.Business/Zenit.Management.Business.csproj" -c Release

COPY ["Zenit.Management/Zenit.Management.Host/", "Zenit.Management/Zenit.Management.Host/"]
RUN dotnet build "Zenit.Management/Zenit.Management.Host/Zenit.Management.Host.csproj" -c Release
RUN dotnet publish "Zenit.Management/Zenit.Management.Host/Zenit.Management.Host.csproj" -c Release -o "./dist"

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR app
COPY --from=builder /src/dist .
ENTRYPOINT ["dotnet", "Zenit.Management.Host.dll"]
