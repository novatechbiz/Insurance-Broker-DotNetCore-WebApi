
# My .NET Core Minimal API Project

## Overview

This is a .NET Core project built using the Minimal API structure. The project serves as a backend service for managing various entities such as Companies, Projects, Users, Teams, and more. It follows a clean architecture and leverages the following technologies:

- **.NET Core 6+**
- **Minimal APIs**
- **Entity Framework Core**
- **MediatR** for handling CQRS
- **FluentValidation** for input validation
- **Dependency Injection** for managing services
- **Automapper** for mapping between entities and DTOs

## Project Structure

The project is organized into the following layers:

- **Models:** Contains entity classes that represent the database tables.
- **DTOs:** Data Transfer Objects used for encapsulating data exchanged between layers.
- **Services:** Contains business logic and interactions with the repository layer.
- **Handlers:** MediatR handlers for managing queries and commands.
- **Repositories:** Abstract layer for database operations using Entity Framework Core.
- **Helpers:** Utility classes for mapping, patching, and other reusable functionalities.
- **Validators:** Contains validation logic for incoming requests using FluentValidation.
- **Endpoints:** Defines all the Minimal API endpoints and their mappings.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or any other supported database
- [Postman](https://www.postman.com/downloads/) for testing API endpoints

### Installation

1. **Clone the repository:**

   ```bash
   git clone https://github.com/your-repo-url.git
   cd your-repo-name
   ```

2. **Set up the database:**

   - Update the connection string in `appsettings.json` to point to your database.
   - Run the following command to apply migrations and create the database:

     ```bash
     dotnet ef database update
     ```

3. **Run the application:**

   ```bash
   dotnet run
   ```

4. **Test the API:**

   You can use Postman to test the API endpoints. Import the provided Postman collection (`postman_collection.json`) to get started.

### API Endpoints

The API exposes the following endpoints:

- **Users**
  - `GET /users` - Get all users (with pagination)
  - `GET /users/{id}` - Get user by ID
  - `POST /users` - Create a new user
  - `PUT /users/{id}` - Update an existing user
  - `DELETE /users/{id}` - Delete a user

- **Companies**
  - `GET /companies` - Get all companies (with pagination)
  - `GET /companies/{id}` - Get company by ID
  - `POST /companies` - Create a new company
  - `PUT /companies/{id}` - Update an existing company
  - `DELETE /companies/{id}` - Delete a company

... and so on for other entities like Projects, Teams, etc.

### Pagination

Most `GET` endpoints support pagination using the following query parameters:

- `PageNumber`: The page number to retrieve (default is 1).
- `PageSize`: The number of items per page (default is 10).

Example:

```bash
GET /companies?pageNumber=2&pageSize=5
```

### Validation

All requests are validated using FluentValidation. If a request is invalid, a `400 Bad Request` response is returned with details of the validation errors.

### Dependency Injection

The project uses .NET Core's built-in dependency injection. Services are registered in `Program.cs` and injected wherever needed.

### Error Handling

Global error handling is implemented to catch and handle exceptions gracefully. The API returns structured error responses in case of failures.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. Fork the repository.
2. Create a feature branch.
3. Make your changes and commit them with clear and concise messages.
4. Push your changes to your forked repository.
5. Create a pull request to the main branch of the original repository.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
