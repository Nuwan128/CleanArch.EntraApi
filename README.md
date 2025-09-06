# CleanArch.EntraApi

A .NET 8 Web API starter template following Clean Architecture principles and integrated with Azure Entra ID (Azure AD) for authentication and authorization. It provides a maintainable and scalable foundation for building secure enterprise-grade APIs.

## ✨ Features

- ✅ **Clean Architecture:** Full separation of concerns into Domain, Application, Infrastructure, and Presentation layers.  
- ✅ **Azure Entra ID Authentication:** Secure JWT-based authentication using Microsoft Identity Platform.  
- ✅ **Role-Based Authorization (RBAC):** Pre-configured Admin and User roles with policy-based access control.  
- ✅ **Swagger UI Integration:** Interactive API documentation with built-in OAuth2 authentication flow.  
- ✅ **Entity Framework Core:** Data access layer with a clean DbContext and repository pattern.  
- ✅ **CQRS with MediatR:** Implemented using the Mediator pattern for clean business logic.  
- ✅ **Test Projects:** Includes unit and integration test projects for sustainable development.  

## 🏗️ Solution Architecture

```
CleanArch.EntraApi/
├── src/
│   ├── CleanArch.EntraApi.Domain/          # Entities, Enums, Exceptions, Interfaces
│   ├── CleanArch.EntraApi.Application/     # Use Cases, CQRS, DTOs, Validators, Interfaces
│   ├── CleanArch.EntraApi.Infrastructure/  # Data, Identity, External Services
│   └── CleanArch.EntraApi.WebApi/          # Controllers, Middleware, Configuration
└── tests/
    ├── CleanArch.EntraApi.Application.Tests/
    └── CleanArch.EntraApi.Integration.Tests/
```

### Dependencies
- WebApi → depends on Infrastructure, Application  
- Infrastructure → depends on Application, Domain  
- Application → depends on Domain  

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK  
- An Azure Account (to configure Entra ID)  
- An IDE (e.g., Visual Studio 2022, Rider, or VS Code)  

### 1. Clone the Repository
```bash
git clone https://github.com/Nuwan128/CleanArch.EntraApi.git
cd CleanArch.EntraApi
```

### 2. Azure Entra ID (Azure AD) App Registration

1. Navigate to the Azure Portal.  
2. Go to **Azure Active Directory > App registrations > New registration.**  
   - **Name:** CleanArch.EntraApi  
   - **Supported account types:** Accounts in this organizational directory only (Single tenant)  
   - **Redirect URI:** Leave blank for now; we will add it later.  
3. Note down the **Application (client) ID** and **Directory (tenant) ID.**  

#### Configure Redirect URIs
- Go to **Authentication > Add a platform > Web.**  
- Add the following redirect URIs:  
  - `https://localhost:<PORT>/signin-oidc` (Replace `<PORT>` with your port, e.g., 7104)  
  - `https://localhost:<PORT>/swagger/oauth2-redirect.html`  

#### Expose an API & Define Roles
- Go to **Expose an API** in your app registration.  
  - Set the Application ID URI to `api://{your-client-id}`.  
  - Add a scope named `access_as_user`.  
- Go to **App roles > Create app role.**  
  - Admin (Users/Groups, Value: Admin)  
  - User (Users/Groups, Value: User)  
- Go to **API permissions > Add a permission > My APIs > Select your app > Select the access_as_user permission > Grant admin consent.**  

### 3. Configure the Application
Open `src/CleanArch.EntraApi.WebApi/appsettings.json` and update:

```json
"AzureAd": {
  "Instance": "https://login.microsoftonline.com/",
  "Domain": "yourdomain.onmicrosoft.com",
  "TenantId": "your-tenant-id",
  "ClientId": "your-client-id",
  "Scopes": "access_as_user",
  "CallbackPath": "/signin-oidc"
}
```

### 4. Run the Application
```bash
dotnet restore
dotnet run --project src/CleanArch.EntraApi.WebApi
```
Navigate to: `https://localhost:7104/swagger`  

### 5. Test the Authentication
- In Swagger UI, click **Authorize**.  
- Sign in with a user account from your Azure AD tenant.  
- Assign **Admin/User** role in Azure AD.  
- Call `GET /api/Projects`. You should receive `200 OK`.  

## 📖 API Endpoints

| Method | Endpoint              | Description                            | Authorization |
|--------|-----------------------|----------------------------------------|---------------|
| GET    | /api/Projects         | Returns a list of sample projects.     | Admin, User   |
| GET    | /api/Projects/claims  | Returns the claims of the user.        | Authenticated |

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test tests/CleanArch.EntraApi.Application.Tests/
```

### Run Integration Tests
```bash
dotnet test tests/CleanArch.EntraApi.Integration.Tests/
```

## 🔧 Troubleshooting Common Issues

| Error | Solution |
|-------|----------|
| AADSTS500113: No reply address is registered... | Add the correct Redirect URIs in Azure App Registration. |
| 403 Forbidden on authorized endpoints | Assign user to App Role (Admin/User) and check token claims. |
| AADSTS9002326: Cross-origin token redemption... | Ensure Implicit grant flow in Swagger config. |
| Invalid grant: code_verifier | PKCE issue. Implicit flow avoids this. |

## 📂 Project Structure Overview

| Project  | Responsibility |
|----------|----------------|
| Domain | Core business logic, entities, and interfaces. Free of external dependencies. |
| Application | Implements use cases, CQRS, validation, business rules. Depends on Domain. |
| Infrastructure | Data access (EF Core), identity (Azure Entra ID), infrastructure concerns. Depends on Application, Domain. |
| WebApi | Presentation layer. Controllers, API models, configuration. Depends on Infrastructure, Application. |

## 🤝 Contributing

1. Fork the project.  
2. Create your feature branch (`git checkout -b feature/AmazingFeature`).  
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`).  
4. Push to the branch (`git push origin feature/AmazingFeature`).  
5. Open a Pull Request.  

## 📜 License

This project is licensed under the MIT License. See the LICENSE file for details.
