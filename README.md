# RS.JwtDemo

## Description
RS.JwtDemo is a demo project showcasing JWT (JSON Web Token) authentication and authorization using .NET 9.

## Features
- **JWT Token Generation**: Securely generate JWT tokens with custom claims.
- **Authentication Middleware**: Automatically add JWT tokens to HTTP requests.
- **API Clients**: Simplified API interaction with `ApiClient` and `AuthClient`.
- **Role-Based Authorization**: Protect endpoints based on user roles.

## Technologies Used
- .NET 9
- ASP.NET Core
- JWT (JSON Web Token)
- Entity Framework Core (if applicable)
- Microsoft IdentityModel Tokens

## Getting Started
1. **Clone the repository**:
    - git clone [RS.JwtDemo](https://github.com/RoslynSoftware/RS.JwtDemo.git)
    - cd RS.JwtDemo


2. **Update appsettings.json**:
    - Configure your JWT settings in `appsettings.json`.

3. **Run the application**:
    - Open the solution in Visual Studio 2022.
    - Restore the NuGet packages.
    - Build and run the application.

## Usage
- **User Login**: Authenticate users and generate JWT tokens.
- **Protected Endpoints**: Access secure API endpoints using the generated JWT tokens.
- **Role-Based Access**: Demonstrates role-based access control.

## Contributing
Contributions are welcome! Please fork the repository and submit pull requests.

## License
This project is licensed under the MIT License.
