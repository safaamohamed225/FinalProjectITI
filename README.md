# 🏠 Wasset - Housing Services Platform

[![Live Demo](https://img.shields.io/badge/Live%20Demo-Visit%20Site-blue?style=for-the-badge)](https://wasset-one.vercel.app/home)
[![.NET](https://img.shields.io/badge/.NET-8.0-purple?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-17-red?style=for-the-badge&logo=angular)](https://angular.io/)
[![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)](LICENSE)

A comprehensive full-stack web application for housing and property listings, designed to help students find and book accommodation near universities. Built as a graduation project during the ITI (Information Technology Institute) program, showcasing modern web development practices and clean architecture principles.

![Wasset Screenshot](https://github.com/Bassam-Serag/Wasset/assets/105117034/2bc919ee-3fe6-48ef-825f-77a33cc1b293)
--------------------------------------------
## 🌟 Glimpse of the working solution: 🌟
<img width="1254" height="623" alt="wasset1" src="https://github.com/user-attachments/assets/b4d39e06-add7-4b46-b9d9-112873bad779" />

<img width="1149" height="823" alt="wasset2" src="https://github.com/user-attachments/assets/a2086c4d-d828-47d4-83f7-16e3fde3b10a" />

<img width="1181" height="577" alt="wasset3" src="https://github.com/user-attachments/assets/862b2755-c0da-4c5c-a161-bb14693228ed" />

<img width="1183" height="625" alt="wasset4" src="https://github.com/user-attachments/assets/bfb276b1-ec4d-42dd-8d9a-119b4da54885" />

<img width="1188" height="389" alt="wasset5" src="https://github.com/user-attachments/assets/725b35f3-cb25-434b-ba69-afd65f74e483" />

<img width="1180" height="589" alt="wasset6" src="https://github.com/user-attachments/assets/b52fa587-d3df-4b2f-bba5-32b2215c3f6b" />



## 🌟 Features

### 🔍 **Search & Discovery**
- Advanced property search with multiple filters
- Location-based search near universities
- Interactive map integration
- Real-time availability updates

### 👤 **User Management**
- Secure user registration and authentication
- Role-based access control (Students, Property Owners, Admins)
- Profile management and preferences
- Session management with ASP.NET Core Identity

### 🏢 **Property Management**
- Property listing creation and management
- Photo upload and gallery management
- Detailed property descriptions and amenities
- Pricing and availability management

### 💳 **Booking & Payments**
- Seamless booking workflow
- Secure payment processing with third-party gateway integration
- Booking history and management
- Automated confirmation and notifications

### 📱 **Responsive Design**
- Mobile-first responsive design
- Cross-browser compatibility
- Optimized for all screen sizes
- Progressive Web App capabilities

## 🛠️ Technology Stack

### Frontend
- **Framework:** Angular 17
- **Language:** TypeScript
- **Styling:** Bootstrap 5, CSS3, HTML5
- **Design Pattern:** Component-based architecture
- **Responsive:** Mobile-first design approach

### Backend
- **Framework:** ASP.NET Core Web API
- **Architecture:** Clean Architecture with MVC pattern
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Authentication:** ASP.NET Core Identity
- **API Design:** RESTful APIs with LINQ

### DevOps & Deployment
- **Frontend Deployment:** Vercel
- **Version Control:** Git & GitHub
- **Package Management:** npm (Frontend), NuGet (Backend)

## 🚀 Getting Started

### Prerequisites

Ensure you have the following installed on your development machine:

- [Git](https://git-scm.com/downloads) (latest version)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [.NET SDK](https://dotnet.microsoft.com/download) (8.0 or higher)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or Express)
- [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/safaamohamed225/Wasset.git
   cd Wasset
   ```

2. **Backend Setup**
   ```bash
   # Navigate to backend directory
   cd Backend
   
   # Restore NuGet packages
   dotnet restore
   
   # Update database connection string in appsettings.json
   # Run database migrations
   dotnet ef database update
   
   # Start the API server
   dotnet run
   ```

3. **Frontend Setup**
   ```bash
   # Navigate to frontend directory
   cd Frontend
   
   # Install npm dependencies
   npm install
   
   # Start the development server
   ng serve
   ```

4. **Access the application**
   - Frontend: http://localhost:4200
   - Backend API: http://localhost:5000
   - Swagger Documentation: http://localhost:5000/swagger

## 📁 Project Structure

```
Wasset/
├── Backend/
│   ├── Controllers/          # API Controllers
│   ├── Models/              # Data Models
│   ├── Services/            # Business Logic Layer
│   ├── Data/                # Entity Framework Context
│   ├── DTOs/                # Data Transfer Objects
│   └── Migrations/          # Database Migrations
├── Frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/  # Angular Components
│   │   │   ├── services/    # Angular Services
│   │   │   ├── models/      # TypeScript Models
│   │   │   └── guards/      # Route Guards
│   │   ├── assets/          # Static Assets
│   │   └── environments/    # Environment Configs
└── README.md
```

## 🔧 Configuration

### Backend Configuration

Update `appsettings.json` with your configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
  },
  "JwtSettings": {
    "SecretKey": "your-jwt-secret-key",
    "Issuer": "Wasset",
    "Audience": "WassetUsers"
  },
  "PaymentGateway": {
    "ApiKey": "your-payment-gateway-api-key",
    "SecretKey": "your-payment-gateway-secret"
  }
}
```

### Frontend Configuration

Update `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  mapApiKey: 'your-map-api-key'
};
```

## 📚 API Documentation

The API follows RESTful conventions and includes comprehensive Swagger documentation. Key endpoints include:

- `GET /api/properties` - Get all properties with filtering
- `POST /api/properties` - Create new property listing
- `GET /api/properties/{id}` - Get property details
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User authentication
- `POST /api/bookings` - Create booking
- `GET /api/bookings/user/{userId}` - Get user bookings

Visit `/swagger` endpoint when running the backend for complete API documentation.

## 🤝 Contributing

We welcome contributions to improve Wasset! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Authors

- **Safaa Mohamed** - *Lead Developer* - [@safaamohamed225](https://github.com/safaamohamed225)
- **Bassam Serag** - *Contributor* - [@Bassam-Serag](https://github.com/Bassam-Serag)

## 🙏 Acknowledgments

- **ITI (Information Technology Institute)** - For providing the educational framework and guidance
- **Angular Team** - For the amazing frontend framework
- **Microsoft** - For .NET Core and Entity Framework
- **Bootstrap Team** - For the responsive CSS framework
- **All Contributors** - Who helped make this project possible

## 📞 Support

If you have any questions or need support, please:
- Open an issue on GitHub
- Contact us at safaa.mohamed.ibrahem@gmail.com
- Visit our [documentation](https://wasset-one.vercel.app)

---

**⭐ Don't forget to give this project a star if you found it helpful! ⭐**
