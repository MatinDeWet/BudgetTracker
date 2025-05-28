# BudgetTracker

A proof-of-concept (POC) budget tracking application built with Angular frontend and .NET 9 backend. This project demonstrates a modern full-stack approach to personal finance management.

## Project Overview

BudgetTracker helps users monitor their financial activities by tracking accounts, transactions, and organizing expenses with tags. The application uses a clean architecture approach with domain-driven design principles.

## Features

- **User Authentication**: Secure registration and login functionality
- **Account Management**: Create, update, delete, and search financial accounts
- **Transaction Tracking**: Record financial transactions with detailed information
- **Tagging System**: Organize and categorize transactions with customizable tags
- **Search Capabilities**: Find transactions and accounts using various criteria

## Architecture

### Backend (.NET 9)

The backend follows a clean architecture pattern with these core components:

- **Core/Domain**: Contains business entities, enums, and domain events
- **Core/Application**: Houses application logic, features, and repository interfaces
- **Infrastructure/Persistence**: Implements data access, repositories, and database context
- **Presentation/API**: Exposes RESTful endpoints for Angular frontend

### Frontend (Angular)

The Angular frontend connects to the backend API to provide a user-friendly interface for managing budget data.

## Technologies Used

### Backend
- .NET 9
- Entity Framework Core
- FastEndpoints
- JWT Authentication
- Serilog for logging

### Frontend
- Angular
- Angular Bootstrap
- Bootstrap
- TypeScript
- RxJS

## Setup and Installation

### Prerequisites
- .NET 9 SDK
- Node.js and npm
- Angular CLI
- PostgreSQL Server (or other configured database)

### Backend Setup
1. Clone the repository
2. Navigate to the project root directory
3. Configure connection strings in appsettings.json
5. Start the API: `docker compose up -d`

### Frontend Setup
1. Navigate to the Angular project directory
2. Install dependencies: `npm install`
3. Start the Angular development server: `ng serve`
4. Access the application at `http://localhost:4200`

## API Endpoints

The API includes endpoints for:
- Authentication (registration, login)
- Account management (CRUD operations)
- Transaction management (CRUD operations)
- Tag management (CRUD operations)
- Transaction-Tag relationships

## Development Notes

This project is a proof-of-concept demonstrating my first implementation with Angular integrated with a .NET backend. The application showcases best practices in:

- Clean Architecture
- Domain-Driven Design
- CQRS pattern (Command and Query Responsibility Segregation)
- RESTful API design
- Security with JWT authentication
- Cross-origin resource sharing (CORS) configuration
