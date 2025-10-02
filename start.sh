#!/bin/bash

# Start QueueInsight Application

echo "🚀 Starting QueueInsight..."

# Check if RabbitMQ is running
if ! curl -s http://localhost:15672 > /dev/null 2>&1; then
    echo "⚠️  RabbitMQ is not running. Starting with docker-compose..."
    docker-compose up -d
    echo "⏳ Waiting for RabbitMQ to be ready..."
    sleep 5
fi

# Start the backend API in the background
echo "🔧 Starting Backend API..."
cd src/QueueInsight.Api
dotnet run &
BACKEND_PID=$!
cd ../..

# Wait for backend to start
echo "⏳ Waiting for backend to start..."
sleep 5

# Start the frontend
echo "🎨 Starting Frontend..."
cd src/QueueInsight.Web
npm start &
FRONTEND_PID=$!
cd ../..

echo ""
echo "✅ QueueInsight is starting!"
echo ""
echo "📍 Frontend: http://localhost:4200"
echo "📍 Backend API: http://localhost:5000"
echo "📍 RabbitMQ Management: http://localhost:15672"
echo ""
echo "Press Ctrl+C to stop all services"

# Handle Ctrl+C
trap "echo '🛑 Stopping services...'; kill $BACKEND_PID $FRONTEND_PID; docker-compose down; exit" INT

# Wait for processes
wait
