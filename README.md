# Find Closest Shelter Web Application

This project is a multi-layered ASP.NET Core web application that helps users find the closest available shelter based on their current location. It is designed for use in emergency situations, allowing users to quickly locate nearby public shelters or protected spaces.

## Project Overview

The application exposes a RESTful API that enables clients (such as web or mobile apps) to:
- **Find the 10 closest shelters** to a given location (latitude/longitude).
- **View details** about each shelter, including capacity, contact person, and 24/7 availability.
- **Read and submit opinions/reviews** about shelters, including ratings and images.
- **Browse shelters** by name or ID.

## Architecture

The solution is organized into several projects, each with a specific responsibility:

- **Core/**  
  Contains shared models, DTOs, interfaces, and mapping logic.  
  Example: [`Core.Models.Address`](Core/Models/Address.cs), [`Core.Models.Location`](Core/Models/Location.cs)

- **Dal/**  
  Data Access Layer. Handles database operations and migrations using Entity Framework Core.  
  Example: [`Dal.ShelteredPlacesDb`](Dal/ShelteredPlacesDb.cs)

- **Bll_Services/**  
  Business Logic Layer. Implements logic for finding closest shelters, managing opinions, and more.  
  Example: [`Bll_Services.BllAddress`](Bll_Services/BllAddress.cs)

- **WebApplicationFindClosestShelter/**  
  ASP.NET Core Web API project. Exposes endpoints for clients to interact with the system.

## Features

- **Shelter Management:**  
  Store and manage information about shelters, including location, type, and capacity.

- **Find Closest Shelters:**  
  Calculate and return the nearest shelters to a user's location.

- **Opinions/Reviews:**  
  Users can add, update, and delete reviews for shelters, including star ratings and images.

- **API Endpoints:**  
  - Get closest shelters
  - Add/update/remove shelters and opinions
  - Browse shelters by name or ID

- **Swagger UI:**  
  Interactive API documentation available at `/swagger` when running the application.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or update connection string in `appsettings.json`)

### Build and Run

1. **Clone the repository**
2. **Restore dependencies**
   ```sh
   dotnet restore
   ```
3. **Build the solution**
   ```sh
   dotnet build
   ```
4. **Apply migrations and update the database**
   ```sh
   dotnet ef database update --project Dal/Dal.csproj
   ```
5. **Run the application**
   ```sh
   dotnet run --project WebApplicationFindClosestShelter/WebApplicationFindClosestShelter.csproj
   ```
