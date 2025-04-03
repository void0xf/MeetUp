# MeetUp - Microservices Event Platform

[![.NET Backend CI/CD](https://github.com/your-username/MeetUp/actions/workflows/dotnet.yml/badge.svg)](https://github.com/your-username/MeetUp/actions/workflows/dotnet.yml)

A modern, scalable event management platform built on microservices architecture, allowing users to create, discover, and participate in social events.

## 🌟 Project Overview

MeetUp is a full-featured event management platform that enables users to:

- Create and manage events
- Search for events by various criteria
- Join events and interact with event organizers
- Build user profiles
- Communicate with other users

The platform is built on a microservices architecture, providing high scalability, resilience, and modular development.

## 🛠️ Technologies Used

### Backend

- **.NET 8** - Core framework for microservices
- **ASP.NET Core** - Web API framework
- **MongoDB** - NoSQL database for flexible data storage
- **RabbitMQ** - Message broker for service communication
- **MassTransit** - Service bus implementation
- **IdentityServer4** - Authentication and authorization
- **AutoMapper** - Object-to-object mapping
- **YARP** - .NET Reverse Proxy for API Gateway

### DevOps & Infrastructure

- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration
- **GitHub Actions** - CI/CD pipeline

### Testing

- **xUnit** - Testing framework
- **Moq** - Mocking framework
- **Postman** - API testing

## 🏗️ Architecture Overview

MeetUp follows a microservices architecture pattern with the following core concepts:

- **Separation of Concerns**: Each service has its own specific responsibility
- **Database per Service**: Each microservice manages its own data store
- **Event-Driven Communication**: Services communicate through asynchronous messaging
- **API Gateway**: Single entry point that routes requests to appropriate services
- **Authentication/Authorization**: Centralized identity management

## 🧩 Service Descriptions

### 🔐 Identity Service (Port 5000)

Handles authentication, user registration, and token issuance using IdentityServer4. Manages:

- User registration
- Token issuance for authentication
- OAuth2 flows

### 📅 Event Service (Port 7001)

Core service for event management:

- Creating and updating events
- Managing event participants
- Retrieving event details
- Filtering events by criteria

### 🔍 Search Service (Port 7002)

Provides search functionality across the platform:

- Text-based search
- Filtering by various criteria
- Sorting and pagination
- Real-time indexing of new events

### 👤 User Service (Port 7003)

Manages user profiles and related functionality:

- User profile management
- User preferences
- Following/connection management

### 💬 Conversation Service (Port 7004)

Enables messaging functionality:

- Direct messaging between users
- Group conversations
- Message history

### 🌐 Gateway Service (Port 5165)

Acts as the single entry point for client applications:

- Request routing to appropriate services
- Request aggregation
- Load balancing
- Authentication validation

## 📊 Data Flow

1. Clients interact with the system exclusively through the API Gateway
2. Gateway routes requests to the appropriate service
3. Services process requests and return responses
4. For operations that affect other services, events are published to RabbitMQ
5. Interested services subscribe to relevant events and react accordingly

## 📋 Inter-Service Communication

Services communicate in two ways:

1. **Synchronous**: Direct HTTP calls (limited usage, primarily through the gateway)
2. **Asynchronous**: Message-based communication using RabbitMQ and MassTransit

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK
- Docker and Docker Compose
- MongoDB
- RabbitMQ

### Setup

1. Clone the repository

```bash
git clone https://github.com/yourusername/MeetUp.git
cd MeetUp
```

2. Start the infrastructure (MongoDB, RabbitMQ)

```bash
docker-compose up -d
```

### Using Postman

The repository includes a Postman collection (`MeetUp_API.postman_collection.json`) and environment (`MeetUp_API.postman_environment.json`) that you can import to test all API endpoints.

## 🌱 Future Enhancements

- Mobile applications (iOS/Android)
- Real-time notifications
- Event recommendations based on user preferences
- Payment integration for paid events
- Advanced analytics

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👥 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
