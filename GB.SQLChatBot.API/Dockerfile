FROM mcr.microsoft.com/dotnet/aspnet:6.0

USER 0
COPY certificate/lcwroot.crt /usr/local/share/ca-certificates
COPY certificate/lcwsubroot.crt /usr/local/share/ca-certificates
RUN update-ca-certificates

WORKDIR /app

EXPOSE 8080
ENV DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:8080

COPY . .
USER 1001
ENTRYPOINT ["dotnet", " GB.SQLChatBot.API.dll"]