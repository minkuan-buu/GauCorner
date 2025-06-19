# --- Base image for runtime ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5152

# Dùng biến môi trường nếu PORT được set từ .env (ví dụ thông qua docker-compose)
ENV ASPNETCORE_URLS=http://+:5152

# (Không đặt USER app ở đây để tránh lỗi phân quyền khi copy file)

# --- Build stage ---
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src

# Copy project file và restore
COPY ["GauCorner.API/GauCorner.API.csproj", "GauCorner.API/"]
RUN dotnet restore "GauCorner.API/GauCorner.API.csproj"

# Copy toàn bộ source code
COPY . .

# Build
WORKDIR "/src/GauCorner.API"
RUN dotnet build "GauCorner.API.csproj" -c $configuration -o /app/build

# --- Publish stage ---
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "GauCorner.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# --- Final image for running ---
FROM base AS final
WORKDIR /app

# Copy từ publish stage
COPY --from=publish /app/publish .

# Nếu bạn chắc chắn thư mục này thuộc quyền của `app` user, thì mới dùng:
# USER app

ENTRYPOINT ["dotnet", "GauCorner.API.dll"]
