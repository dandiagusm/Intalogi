# Order System API (ASP.NET Core 8 + EF Core)

A simple Order Management REST API built with **.NET 8** and **Entity Framework Core**, designed to manage customers, products, product types, and orders — complete with analytical reports.

---

## Tech Stack

| Component | Technology |
|------------|-------------|
| Backend | ASP.NET Core 8 (C#) |
| Database | Microsoft SQL Server |
| Docs | Swagger |
| Architecture | Clean modular controllers & DTOs |

---

## Project Structure

```
OrderSystem/
├── Controllers/
│   ├── CustomersController.cs
│   ├── ProductsController.cs
│   ├── ProductTypesController.cs
│   ├── OrdersController.cs
│   └── ReportsController.cs
├── Data/
│   └── AppDbContext.cs
├── Models/
│   ├── Customer.cs
│   ├── ProductType.cs
│   ├── Product.cs
│   ├── Order.cs
│   └── OrderDetail.cs
├── Dtos/
│   ├── ProductDto.cs
│   ├── OrderCreateDto.cs
│   └── OrderResponseDto.cs
└── Program.cs
```

## Entity Relationship Diagram (ERD)

Below is the logical design for the Order System database:

![Entity Relationship Diagram](docs/ERD%20Intalogi.drawio.png)

### Tables Overview
- **Customers** → Stores customer info (name, address, etc.)
- **ProductTypes** → Categories or classifications of products
- **Products** → Items available for order
- **Orders** → Represents a customer's purchase
- **OrderDetails** → The many-to-many join between Orders and Products


---

## 🚀 Getting Started

### Clone the Project
```bash
git clone https://github.com/yourusername/OrderSystem.git
cd OrderSystem
```

### Restore Dependencies
```bash
dotnet restore
```

### Configure Database
Open **`appsettings.json`** and update your SQL Server connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OrderSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### Run EF Core Migrations
```bash
dotnet ef database update
```
This creates all necessary tables:
- Customers
- ProductTypes
- Products
- Orders
- OrderDetails

---

## Running the API

Start the project:
```bash
dotnet run
```

Output:
```
Now listening on: http://localhost:5042
Application started. Press Ctrl+C to shut down.
```

Then open your browser at:

[http://localhost:5042/swagger](http://localhost:5042/swagger)

---

## API Documentation (Swagger Examples)

### Customers
**POST `/api/customers`**
```json
{
  "name": "John Doe",
  "address": "Jl. Merdeka No. 1",
  "city": "Jakarta",
  "province": "DKI Jakarta",
  "phone": "08123456789"
}
```

**GET `/api/customers`**
```json
[
  {
    "id": 1,
    "name": "John Doe",
    "address": "Jl. Merdeka No. 1",
    "city": "Jakarta",
    "province": "DKI Jakarta",
    "phone": "08123456789"
  }
]
```

---

### Orders
**POST `/api/orders`**
```json
{
  "customerId": 1,
  "orderDate": "2025-11-07",
  "details": [
    { "productId": 1, "quantity": 2 },
    { "productId": 3, "quantity": 1 }
  ]
}
```

**GET `/api/orders`**
```json
[
  {
    "id": 1,
    "orderDate": "2025-11-07T00:00:00",
    "customerId": 1,
    "customerName": "John Doe",
    "totalAmount": 1500000,
    "details": [
      { "productId": 1, "productName": "Keyboard", "price": 500000, "quantity": 2, "totalPrice": 1000000 },
      { "productId": 3, "productName": "Mouse", "price": 500000, "quantity": 1, "totalPrice": 500000 }
    ]
  }
]
```

---

## 📊 Reports (Analytics Endpoints)

| Endpoint | Description | 
|-----------|--------------|
| `/api/reports/sales-before?date=2025-01-01` | Orders before date |
| `/api/reports/sales-by-product-type` | Grouped by product type |
| `/api/reports/sales-by-product` | Grouped by product |
| `/api/reports/products-above-average` | Products above average price |
| `/api/reports/sales-above-5m` | Orders with sales > 5M |
| `/api/reports/products-by-type` | List products by type |


## Common Commands

| Command | Description |
|----------|-------------|
| `dotnet build` | Build the project |
| `dotnet run` | Start the API |
| `dotnet ef migrations add <Name>` | Create a new migration |
| `dotnet ef database update` | Apply migrations |
| `dotnet clean` | Clean output folders |

---

## Author
**Dandi Agus Maulana**  


