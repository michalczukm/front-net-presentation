FROM mcr.microsoft.com/dotnet/core-nightly/sdk:3.1.100-alpine3.10 AS build-env
WORKDIR /app

COPY ./ ./

RUN dotnet publish -c Release -o out FrontNet.Survey

# Build runtime image
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build-env /app/out/FrontNet.Survey/dist .
COPY ./FrontNet.Survey/nginx.conf /etc/nginx/nginx.conf