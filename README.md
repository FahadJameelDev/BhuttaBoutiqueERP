# 👗 Bhutta Boutique ERP - Tailoring & Boutique Management Suite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.2-7952B3?style=flat-square&logo=bootstrap)
![Architecture](https://img.shields.io/badge/Architecture-Clean_Code-brightgreen?style=flat-square)

An enterprise-grade, comprehensive ERP system specifically architected for the **Tailoring and Boutique industry**. Built on **ASP.NET Core MVC (.NET 10)** with **Razor Views** for server-side rendering, backed by **Entity Framework Core 10** and **SQL Server**. The application features a highly responsive UI built with **Bootstrap 5** and dynamic client-side interactions via **AJAX/jQuery**. It seamlessly unifies complex workflows including Sales, Inventory, Production, HR/Payroll, and Financial Reporting into a single, cohesive platform.
## 🏗️ Architecture Overview

The system is designed following **Clean Architecture** principles within a single ASP.NET Core MVC project. The business logic is decoupled from the UI and Data Access layers, ensuring a highly maintainable and testable codebase.

```mermaid
graph TD
    classDef ui fill:#d4edda,stroke:#28a745,stroke-width:2px;
    classDef logic fill:#cce5ff,stroke:#007bff,stroke-width:2px;
    classDef data fill:#f8d7da,stroke:#dc3545,stroke-width:2px;
    classDef models fill:#fff3cd,stroke:#ffc107,stroke-width:2px;

    subgraph Presentation_Layer [Presentation Layer]
        V[Razor Views .cshtml]:::ui
        J[wwwroot/ - AJAX, jQuery, CSS]:::ui
        C[Controllers/ - Thin Controllers]:::ui
    end

    subgraph Business_Logic_Layer [Business Logic Layer]
        S[Services/ - Interfaces & Implementations]:::logic
        F[Filters/ - Global Exception & Audit]:::logic
    end

    subgraph Data_Access_Layer [Data Access Layer]
        D[Data/ - AppDbContext]:::data
        DB[(SQL Server Database)]:::data
    end

    subgraph Domain_Models [Domain Models]
        M[Models/ - Entities & ViewModels]:::models
    end

    V --> C
    J --> C
    C --> S
    S --> D
    D --> DB
    S --> M
    C --> M


### 📂 Backend Project Breakdown

The backend is structured as a robust, enterprise-grade ASP.NET Core MVC application, strictly following the **Service Layer Pattern** to keep controllers thin and business logic isolated.

*   **`Controllers/` (Presentation):** 
    *   Handles HTTP requests, routing, and model binding.
    *   Designed to be **thin**—they only validate input and call the appropriate Service.
    *   Protected using `[Authorize(Roles="...")]` for Role-Based Access Control.
*   **`Services/` (Business Logic):** 
    *   The core engine of the ERP.
    *   Contains interfaces (e.g., `IOrderService`, `IPayrollService`, `IInventoryService`) and their implementations.
    *   Handles complex logic like **Manual Cascade Deletion** (preventing foreign key crashes) and **Financial Calculations** (Payroll, P&L).
*   **`Data/` (Data Access):** 
    *   Houses the `ApplicationDbContext`.
    *   Configured using **Entity Framework Core (Code-First)**.
    *   Manages a highly normalized **24-table schema** with precise decimal configurations for financial accuracy.
*   **`Models/` (Domain Entities & ViewModels):** 
    *   **Entities:** Represent the actual SQL Server tables (e.g., `Order`, `Employee`, `StockIn`).
    *   **ViewModels:** Dedicated classes used to pass clean, aggregated data to the UI (preventing over-posting and exposing raw database models).
*   **`Filters/` (Cross-Cutting Concerns):** 
    *   `GlobalExceptionFilter`: Catches unhandled errors gracefully and returns premium error pages.
    *   `GlobalAuditFilter`: Automatically intercepts actions to log critical user activities for auditing.
*   **`Migrations/`:** 
    *   EF Core migrations that track the evolution of the database schema over time, ensuring version control for the database.

### 🎨 Frontend Integration

The frontend is not a disconnected SPA (like React), but a highly optimized **Server-Side Rendered (SSR)** architecture using Razor and dynamic AJAX calls.

*   **Razor Views (`.cshtml`):** 
    *   The UI is rendered on the server, meaning faster initial page loads and better SEO.
    *   Uses **Bootstrap 5** for a mobile-first, responsive layout across all devices.
*   **Dynamic Interactions (AJAX/jQuery):** 
    *   Instead of full-page reloads, critical operations (like fetching a customer's measurement, submitting a payment, or updating stock) happen asynchronously via AJAX.
    *   This provides a **Single Page Application (SPA) feel** without the heavy overhead of a JS framework.
*   **Custom UI/UX:** 
    * 





