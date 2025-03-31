# TourNhanh - Travel Booking Platform

TourNhanh is a comprehensive travel booking platform built with ASP.NET Core MVC, offering a wide range of tours and travel-related services.

## Features

### Tour Management
- **Tour Categories**: Organize tours by different categories
- **Tour Details**: Comprehensive tour information including:
  - Detailed itineraries
  - Maximum participants
  - Available slots
  - Pricing
  - Transportation details
- **Media Management**: Multiple images for each tour
- **Location Management**: Detailed information about tour destinations

### Booking System
- **Online Booking**: Easy tour booking process
- **Payment Integration**: 
  - VNPay payment gateway integration
  - Multiple payment method support
  - Payment status tracking
- **Booking Management**:
  - Booking history
  - Booking status tracking
  - Special notes and requirements

### Hotel Management
- Hotel information and details
- Room availability
- Location-based hotel search
- Hotel images and facilities

### User Features
- **Authentication**:
  - Local authentication
  - Facebook login integration
  - Google login integration
- **User Profiles**:
  - Booking history
  - Personal information management
  - Review and rating history

### Social Features
- **Blog System**: Travel-related articles and updates
- **Interaction**:
  - Comments on tours and blogs
  - Like functionality
  - Tour reviews and ratings
- **User Reviews**: Rating and review system for tours

### Admin Features
- **Dashboard**: Overview of bookings and system statistics
- **Tour Management**: Add, edit, and remove tours
- **User Management**: Manage user roles and permissions
- **Content Management**: Blog and review moderation

## Technical Stack

### Backend
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Identity Framework

### Frontend
- HTML/CSS/JavaScript
- Bootstrap
- jQuery
- Various JS libraries for enhanced UI

### Authentication
- ASP.NET Core Identity
- External authentication providers (Facebook, Google)

### Payment Integration
- VNPay payment gateway

## Architecture
- Repository Pattern
- Dependency Injection
- MVC Architecture

## Getting Started

### Prerequisites
- .NET 6.0 or later
- SQL Server
- Visual Studio 2022 or similar IDE

### Installation
1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run the following commands:
```bash
dotnet restore
dotnet ef database update
dotnet run
```

### Configuration
- Set up Facebook and Google authentication credentials
- Configure VNPay payment gateway settings
- Update contact information in application settings

## License
This project is proprietary software. All rights reserved.