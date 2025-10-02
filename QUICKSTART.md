# Quick Start Guide

This guide will help you get QueueInsight up and running in 5 minutes.

## Prerequisites Check

Before starting, verify you have the required tools:

```bash
# Check .NET version (should be 9.x)
dotnet --version

# Check Node.js version (should be 20+)
node --version

# Check npm version
npm --version

# Check Docker (optional, for RabbitMQ)
docker --version
```

## Step 1: Start RabbitMQ

### Option A: Using Docker (Recommended)
```bash
docker-compose up -d
```

This will start RabbitMQ with the management interface enabled.

### Option B: Local RabbitMQ
If you have RabbitMQ installed locally, ensure the management plugin is enabled:
```bash
rabbitmq-plugins enable rabbitmq_management
rabbitmq-server
```

### Verify RabbitMQ
Open http://localhost:15672 in your browser. You should see the RabbitMQ Management interface.
- Default username: `guest`
- Default password: `guest`

## Step 2: Start the Backend API

```bash
cd src/QueueInsight.Api
dotnet run
```

You should see output like:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

Keep this terminal open.

## Step 3: Start the Frontend

Open a new terminal:

```bash
cd src/QueueInsight.Web
npm install  # Only needed the first time
npm start
```

The Angular development server will start and automatically open your browser to http://localhost:4200.

## Step 4: Use the Application

1. **Browse Virtual Hosts**: On the left panel, you'll see a list of virtual hosts from RabbitMQ
2. **Select a Virtual Host**: Click on any virtual host (e.g., `/`) to view its queues
3. **View Queues**: Queues will appear below the virtual host list
4. **Select a Queue**: Click on a queue to view its messages
5. **Interact with Messages**:
   - Click "Publish" to add new messages
   - Click "Move" to transfer messages to another queue
   - Use the delete function to remove messages
   - Click on individual messages to see their full details

## Automated Start (Optional)

Instead of manually starting each component, use the provided scripts:

### Linux/Mac:
```bash
./start.sh
```

### Windows:
```cmd
start.bat
```

## Testing the Application

### Create a Test Queue

1. Open RabbitMQ Management UI: http://localhost:15672
2. Go to "Queues" tab
3. Click "Add a new queue"
4. Enter name: `test.queue`
5. Click "Add queue"

### Publish a Test Message

1. In QueueInsight, select the `/` virtual host
2. Click on `test.queue`
3. Click "Publish" button
4. Enter message: `{"test": "Hello from QueueInsight!"}`
5. Click "Send"

### View the Message

1. The message should appear in the messages list
2. Click on the message to see full details
3. Try the move and delete operations

## Troubleshooting

### Backend won't start
- **Issue**: Port 5000 is already in use
- **Solution**: Change the port in `src/QueueInsight.Api/Properties/launchSettings.json`

### Cannot connect to RabbitMQ
- **Issue**: Connection refused
- **Solution**: 
  - Ensure RabbitMQ is running: `docker ps` or `rabbitmqctl status`
  - Check credentials in `src/QueueInsight.Api/appsettings.json`

### Frontend build errors
- **Issue**: npm install fails
- **Solution**:
  ```bash
  cd src/QueueInsight.Web
  rm -rf node_modules package-lock.json
  npm install
  ```

### CORS errors in browser
- **Issue**: API calls fail with CORS error
- **Solution**: 
  - Ensure frontend is running on http://localhost:4200
  - Verify CORS policy in `src/QueueInsight.Api/Program.cs`

## Next Steps

- Explore the codebase in `src/`
- Read the full [README.md](README.md) for detailed documentation
- Check [CONTRIBUTING.md](CONTRIBUTING.md) if you want to contribute
- Customize the UI styling in the `.scss` files

## Stopping the Application

Press `Ctrl+C` in each terminal window, then:

```bash
# Stop RabbitMQ (if using Docker)
docker-compose down
```

## Support

For issues or questions:
- Check the [README.md](README.md) troubleshooting section
- Open an issue on GitHub
- Review RabbitMQ logs if connection issues persist

---

Happy exploring your RabbitMQ queues! 🐰📬
