#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ARG CONNECTIONSTRINGS__PRECOSELLOUT
ARG NHUBUSERS__BASEURL
ARG AWS_KEY_NC
ARG AWS_SECRET_NC

ENV CONNECTIONSTRINGS__PRECOSELLOUT=$CONNECTIONSTRINGS__PRECOSELLOUT
ENV ASPNETCORE_ENVIRONMENT=Production
ENV NHUBUSERS__BASEURL=${NHUBUSERS__BASEURL}
ENV AWS__AccessKey $AWS_KEY_NC
ENV AWS__SecretKey $AWS_SECRET_NC

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["api.Worker/api.Worker.csproj", "api.Worker/"]
COPY ["api.Data/api.Data.csproj", "api.Data/"]
COPY ["api.Domain/api.Domain.csproj", "api.Domain/"]
COPY ["api.Service/api.Service.csproj", "api.Service/"]
COPY ["api.CrossCutting/api.CrossCutting.csproj", "api.CrossCutting/"]
RUN dotnet restore "api.Worker/api.Worker.csproj"
COPY . .
WORKDIR "/src/api.Worker"
RUN dotnet build "api.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV CORECLR_ENABLE_PROFILING=1  
ENV CORECLR_PROFILER={846F5F1C-F9AE-4B07-969E-05C26BC060D8}  
ENV CORECLR_PROFILER_PATH=/app/datadog/linux-x64/Datadog.Trace.ClrProfiler.Native.so  
ENV DD_DOTNET_TRACER_HOME=/app/datadog  
ENV LD_PRELOAD=/app/datadog/linux-x64/Datadog.Linux.ApiWrapper.x64.so  
ENV DD_PROFILING_ENABLED=1  
ENV DD_PROFILING_ALLOCATION_ENABLED=1  
ENV DD_PROFILING_EXCEPTION_ENABLED=1  
ENV DD_PROFILING_HEAP_ENABLED=1  
ENV DD_PROFILING_LOCK_ENABLED=1   

RUN /app/datadog/createLogPath.sh

ENTRYPOINT ["dotnet", "api.Worker.dll"]