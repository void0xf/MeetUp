FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ConversationService/ConversationService.csproj", "src/ConversationService/"]
RUN dotnet restore "src/ConversationService/ConversationService.csproj"
COPY . .
WORKDIR "/src/src/ConversationService"
RUN dotnet build "ConversationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConversationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConversationService.dll"] 