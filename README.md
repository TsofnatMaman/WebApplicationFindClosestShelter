# Find Closest Shelter Web Application

This project is a multi-layered ASP.NET Core web application that helps users find the closest available shelter based on their current location. The application is designed for emergency situations, allowing people to quickly locate nearby public or private shelters, view details, and share feedback.

---

## Project Overview

The application exposes a RESTful API that enables clients (such as web or mobile apps) to:
- **Find the 10 closest shelters** to a given location (latitude/longitude).
- **View details** about each shelter, including capacity, contact person, and 24/7 availability.
- **Read and submit opinions/reviews** about shelters, including ratings and images.
- **Browse shelters** by name or ID.

The project is structured using Clean Architecture principles, with clear separation between the Core models, Data Access Layer (DAL), Business Logic Layer (BLL), and the Web API.

---

## What This Project Implements

- **Layered architecture**: The solution is divided into Core, DAL, BLL, and Web API projects.
- **Models**: The main entities are Address, Location, Shelter, and Opinion.
- **DTOs and Mapping**: Data Transfer Objects are used for API responses, and mapping is handled between entities and DTOs.
- **Endpoints**: The API allows adding new addresses, viewing recent addresses, finding the closest shelters, and managing opinions.
- **Distance Calculation**: The logic for finding the closest shelters is implemented in the BLL, and each result includes the distance from the user's location.
- **Recent Additions**: You can retrieve addresses added in the last month.
- **Opinions**: Users can add, view, update, and delete reviews for shelters, including star ratings and images.

**Note:**  
The current implementation does not include a separate BuildingType entity (with year built, level, etc.) and does not have user management or permissions. All shelters are represented by the Address entity with a name and code.

---

## Main Models and Properties

### Address

- **Code**: (int) Unique identifier for the address.
- **Location**: (Location) The geographic location (longitude and latitude) of the shelter.
- **Shelter**: (Shelter) The shelter entity associated with this address.
- **ShelterCode**: (int) Foreign key referencing the associated shelter.
- **IsOpen24_7**: (bool) Indicates if the shelter is open 24 hours a day, 7 days a week.
- **ContactPersonName**: (string) Name of the contact person for the shelter.
- **ContactPersonPhone**: (string) Phone number of the contact person.
- **ContactPersonHasSMS**: (bool) Indicates if the contact person can receive SMS messages.
- **Capacity**: (int) Maximum number of people the shelter can accommodate.
- **CurrentNumberPeople**: (int) Current number of people currently in the shelter.
- **AddedSystem**: (DateTime) The date and time when this address was added to the system (set automatically).

### Location

- **Longitude**: (double) The longitude coordinate of the shelter.
- **Latitude**: (double) The latitude coordinate of the shelter.

### Shelter

- **Code**: (int) Unique identifier for the shelter.
- **Name**: (string) The name of the shelter.

### Opinion

- **Code**: (int) Unique identifier for the opinion.
- **Address**: (Address) The address entity (shelter) this opinion is related to.
- **AddressCode**: (int) Foreign key referencing the associated address.
- **Stars**: (int) Star rating given by the user (e.g., 1-5).
- **Text**: (string) The textual content of the opinion/review.
- **Images**: (string[]) Array of image URLs or paths attached to the opinion.

---

## API Controllers and Endpoints

### AddressController

- **GET `api/Address/Closest/{location}`**  
  Returns a list of the closest shelters to the specified location.

- **GET `api/Address/lastMonth`**  
  Returns a list of addresses (shelters) that were added in the last month.

- **POST `api/Address`**  
  Adds a new shelter address.

- **PUT `api/Address`**  
  Updates an existing shelter address.

- **DELETE `api/Address?idAddress={idAddress}`**  
  Removes a shelter address by its ID.

### OpinionController

- **GET `api/Opinion`**  
  Retrieves all opinions in the system.

- **GET `api/Opinion/byAddress?addressCode={addressCode}`**  
  Retrieves all opinions for a specific shelter address.

- **GET `api/Opinion/{id}`**  
  Retrieves a specific opinion by its unique ID.

- **POST `api/Opinion`**  
  Adds a new opinion (review) for a shelter.

- **PUT `api/Opinion`**  
  Updates an existing opinion.

- **DELETE `api/Opinion?id={id}`**  
  Deletes an opinion by its unique ID.

### ShelterController

- **GET `api/Shelter`**  
  Retrieves all shelters in the system.

- **GET `api/Shelter/byId/{id}`**  
  Retrieves a specific shelter by its unique ID.

- **GET `api/Shelter/byName/{name}`**  
  Retrieves all shelters matching a specific name.

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
