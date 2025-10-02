# Contributing to QueueInsight

Thank you for your interest in contributing to QueueInsight! This document provides guidelines for contributing to the project.

## Getting Started

1. Fork the repository
2. Clone your fork: `git clone https://github.com/YOUR_USERNAME/QueueInsight.git`
3. Create a feature branch: `git checkout -b feature/your-feature-name`
4. Make your changes
5. Test your changes thoroughly
6. Commit with clear messages: `git commit -m "Add feature: description"`
7. Push to your fork: `git push origin feature/your-feature-name`
8. Open a Pull Request

## Development Setup

### Prerequisites
- .NET 9 SDK
- Node.js 20+
- RabbitMQ (can use docker-compose)

### Local Development

1. Start RabbitMQ:
```bash
docker-compose up -d
```

2. Run the backend:
```bash
cd src/QueueInsight.Api
dotnet run
```

3. Run the frontend:
```bash
cd src/QueueInsight.Web
npm install
npm start
```

## Coding Standards

### Backend (.NET)
- Follow C# coding conventions
- Use async/await for I/O operations
- Add XML documentation comments for public APIs
- Handle exceptions appropriately
- Write unit tests for new features

### Frontend (Angular)
- Follow Angular style guide
- Use TypeScript strict mode
- Components should be standalone
- Use SCSS for styling
- Follow component naming conventions

## Pull Request Process

1. Update the README.md with details of changes if applicable
2. Update documentation for any API changes
3. Ensure all tests pass
4. Ensure the code builds without warnings
5. Request review from maintainers

## Code Review

All submissions require review. We use GitHub pull requests for this purpose.

## Reporting Bugs

Use GitHub Issues to report bugs. Include:
- Clear description of the issue
- Steps to reproduce
- Expected behavior
- Actual behavior
- Screenshots if applicable
- Environment details (OS, .NET version, Node version)

## Feature Requests

Feature requests are welcome! Please provide:
- Clear description of the feature
- Use cases
- Benefits to users
- Potential implementation approach

## Questions?

Feel free to open an issue with your question or reach out to the maintainers.

## License

By contributing, you agree that your contributions will be licensed under the GNU General Public License v3.0.
