# Build dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0  AS build-env
RUN apt-get update \ 
    && apt-get install -y --no-install-recommends libgdiplus libc6-dev \
    && apt-get clean \
    && apt-get install -y git \
    && rm -rf /var/lib/apt/lists/*

RUN mkdir -p logs
RUN chmod -R 777 logs


RUN cd /usr/lib && ln -s libgdiplus.so gdiplus.dll

VOLUME ["/app"]

WORKDIR /app
COPY . .


ENV DOCKER_HOST=tcp://docker:2375
ENV DOCKER_TLS_CERTDIR: ''
ENV DOCKER_DRIVER: overlay2

RUN dotnet restore src/Gtsc.CMS.Web/Gtsc.CMS.Web.csproj
RUN dotnet publish src/Gtsc.CMS.Web/Gtsc.CMS.Web.csproj -c Release -o out

ENV TZ=Asia/Ho_Chi_Minh
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app

COPY --from=build-env /app/out .
RUN apt-get update -y && apt-get install -y gss-ntlmssp libgdiplus
RUN dotnet dev-certs https
WORKDIR /app
EXPOSE 80/tcp
ENV ASPNETCORE_URLS http://0.0.0.0:8080
ENTRYPOINT  ["dotnet", "Gtsc.CMS.Web.dll"]
#RUN ["apt-get", "update"]
#RUN ["apt-get", "install", "-y", "vim"]
