FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
# EXPOSE 5000
WORKDIR /app
# ENTRYPOINT ["dotnet", "api.dll"]

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build

WORKDIR /src

COPY ["src/api/api.csproj","src/api/"]
COPY ["src/Services/Services.csproj","src/Services/"]
COPY ["src/Entities/MongoEntities/MongoEntities.csproj","src/Entities/MongoEntities/"]
COPY ["src/Models/Models.csproj","src/Models/"]
COPY ["src/Middlewares/Middlewares.csproj","src/Middlewares/"]
COPY ["src/Helpers/Helpers.csproj","src/Helpers/"]
COPY ["src/Enums/Enums.csproj","src/Enums/"]
RUN dotnet restore "src/api/api.csproj"
COPY . .
WORKDIR "/src/src/api"
RUN dotnet build "api.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish

FROM base As final
WORKDIR /app
COPY --from=publish /app/publish .
# RUN apk add --no-cache tzdata
ENV TZ Asia/Taipei
# ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "api.dll"]
# RUN dotnet ef database update

#RUN dotnet ef database update

