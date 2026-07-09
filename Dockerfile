# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app
COPY . . 
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Final Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app/publish .
COPY ./wwwroot /wwwroot
EXPOSE 9933
ENTRYPOINT ["dotnet", "dotnet_starter.dll", "--urls", "http://+:9933"]
