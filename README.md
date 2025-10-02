# QueueInsight

A lightweight web application for managing and monitoring RabbitMQ queues, similar to QueueExplorer. Built with ASP.NET Core 9 Web API and Angular 17.

## Features

- 🔍 **Browse Virtual Hosts**: View all available virtual hosts in your RabbitMQ instance
- 📋 **Queue Management**: List and inspect queues in each virtual host
- 📨 **Message Operations**:
  - Peek messages from queues (configurable count)
  - Publish new messages to queues
  - Delete messages from queues
  - Move messages between queues
- 💻 **Modern UI**: Clean, responsive interface built with Angular and SCSS
- 🔌 **RabbitMQ Integration**: Direct connection to RabbitMQ Management API

## Tech Stack

### Backend
- .NET 9 (ASP.NET Core Web API)
- RabbitMQ Management API integration

### Frontend
- Angular 17
- SCSS for styling
- Standalone components
- Reactive forms

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Node.js 20+](https://nodejs.org/)
- [RabbitMQ](https://www.rabbitmq.com/) with Management Plugin enabled

## Getting Started

### 1. RabbitMQ Setup

Ensure RabbitMQ is running with the Management Plugin enabled. The default management interface runs on `http://localhost:15672`.

To enable the management plugin:
```bash
rabbitmq-plugins enable rabbitmq_management
```

### 2. Backend Configuration

Update the RabbitMQ connection settings in `src/QueueInsight.Api/appsettings.json`:

```json
{
  "RabbitMq": {
    "ManagementUrl": "http://localhost:15672",
    "Username": "guest",
    "Password": "guest"
  }
}
```

### 3. Running the Application

#### Option 1: Run Both (Recommended)

**Terminal 1 - Backend:**
```bash
cd src/QueueInsight.Api
dotnet run
```
The API will start on `http://localhost:5000` (HTTP) and `https://localhost:5001` (HTTPS)

**Terminal 2 - Frontend:**
```bash
cd src/QueueInsight.Web
npm install  # First time only
npm start
```
The Angular app will start on `http://localhost:4200`

#### Option 2: Build for Production

**Backend:**
```bash
cd src/QueueInsight.Api
dotnet build -c Release
dotnet run -c Release
```

**Frontend:**
```bash
cd src/QueueInsight.Web
npm install
npm run build
```
The built files will be in `src/QueueInsight.Web/dist/queue-insight.web`

### 4. Access the Application

Open your browser and navigate to `http://localhost:4200`

## Usage

### Viewing Virtual Hosts
1. On application start, all virtual hosts will be listed on the left panel
2. Click on a virtual host to view its queues

### Managing Queues
1. After selecting a virtual host, queues will be displayed below it
2. Click on a queue to view its messages
3. Queue information includes:
   - Total messages
   - Ready messages
   - Unacknowledged messages

### Message Operations

#### Viewing Messages
- Select a queue to view its messages
- Adjust the "Messages to peek" count (1-100)
- Click "Load" to refresh the message list
- Click on any message to view full details

#### Publishing Messages
1. Click the "Publish" button
2. Enter your message payload
3. Click "Send"

#### Deleting Messages
1. Set the number of messages to delete
2. Click "Delete"
3. Confirm the action

#### Moving Messages
1. Click the "Move" button
2. Specify:
   - Destination virtual host
   - Destination queue
   - Number of messages to move
3. Click "Move"

## API Endpoints

### Virtual Hosts
- `GET /api/vhosts` - List all virtual hosts

### Queues
- `GET /api/queues/{vhost}` - List queues in a virtual host
- `GET /api/queues/{vhost}/{queue}/messages?count={n}` - Get messages from a queue

### Messages
- `POST /api/messages/publish` - Publish a message to a queue
- `DELETE /api/messages/delete` - Delete messages from a queue
- `POST /api/messages/move` - Move messages between queues

## Project Structure

```
QueueInsight/
├── src/
│   ├── QueueInsight.Api/          # ASP.NET Core Web API
│   │   ├── Controllers/           # API Controllers
│   │   ├── Models/               # Data models
│   │   ├── Services/             # Business logic
│   │   └── Program.cs            # Application entry point
│   └── QueueInsight.Web/          # Angular Application
│       └── src/
│           └── app/
│               ├── components/    # UI Components
│               ├── models/        # TypeScript models
│               └── services/      # API services
├── QueueInsight.sln              # Solution file
└── README.md
```

## Development

### Building the Backend
```bash
dotnet build
```

### Running Backend Tests
```bash
dotnet test
```

### Building the Frontend
```bash
cd src/QueueInsight.Web
npm run build
```

### Linting Frontend Code
```bash
cd src/QueueInsight.Web
npm run lint
```

## Configuration

### CORS Settings
By default, the API allows requests from `http://localhost:4200`. To modify CORS settings, edit `Program.cs`:

```csharp
policy.WithOrigins("http://localhost:4200", "https://your-domain.com")
      .AllowAnyHeader()
      .AllowAnyMethod();
```

### API Port Configuration
To change the API port, modify `src/QueueInsight.Api/Properties/launchSettings.json`

### Angular Proxy Configuration
If you change the API port, update the API URL in `src/QueueInsight.Web/src/app/services/rabbitmq.service.ts`:

```typescript
private apiUrl = 'http://localhost:YOUR_PORT/api';
```

## Troubleshooting

### Cannot connect to RabbitMQ
- Verify RabbitMQ is running: `rabbitmqctl status`
- Check Management Plugin is enabled: `rabbitmq-plugins list`
- Verify credentials in `appsettings.json`
- Check firewall settings for port 15672

### CORS errors
- Ensure the Angular dev server is running on `http://localhost:4200`
- Check the CORS policy in `Program.cs` matches your frontend URL

### Build errors
- Ensure you have .NET 9 SDK installed: `dotnet --version`
- Ensure Node.js is installed: `node --version`
- Try cleaning and rebuilding:
  ```bash
  dotnet clean
  dotnet build
  ```

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Acknowledgments

- Inspired by [QueueExplorer](https://www.cogin.com/mq/)
- Built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- UI powered by [Angular](https://angular.io/)
- RabbitMQ integration via [Management API](https://www.rabbitmq.com/management.html)
