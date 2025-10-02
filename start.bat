@echo off
REM Start QueueInsight Application

echo Starting QueueInsight...

REM Check if RabbitMQ is running (basic check)
curl -s http://localhost:15672 >nul 2>&1
if errorlevel 1 (
    echo RabbitMQ is not running. Starting with docker-compose...
    docker-compose up -d
    echo Waiting for RabbitMQ to be ready...
    timeout /t 5 /nobreak >nul
)

REM Start the backend API in a new window
echo Starting Backend API...
start "QueueInsight API" cmd /k "cd src\QueueInsight.Api && dotnet run"

REM Wait for backend to start
echo Waiting for backend to start...
timeout /t 5 /nobreak >nul

REM Start the frontend in a new window
echo Starting Frontend...
start "QueueInsight Web" cmd /k "cd src\QueueInsight.Web && npm start"

echo.
echo QueueInsight is starting!
echo.
echo Frontend: http://localhost:4200
echo Backend API: http://localhost:5000
echo RabbitMQ Management: http://localhost:15672
echo.
echo Press any key to stop all services (you'll need to close the terminal windows manually)
pause >nul

REM Stop docker-compose
docker-compose down
