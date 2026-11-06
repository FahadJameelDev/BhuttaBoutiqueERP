# 👗 Bhutta Boutique ERP - Tailoring & Boutique Management Suite

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-0078D4?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoft-sql-server)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3.2-7952B3?style=flat-square&logo=bootstrap)
![Architecture](https://img.shields.io/badge/Architecture-Clean_Code-brightgreen?style=flat-square)

An enterprise-grade, comprehensive ERP system specifically architected for the **Tailoring and Boutique industry**. Built on **ASP.NET Core MVC (.NET 10)** with **Razor Views** for server-side rendering, backed by **Entity Framework Core 10** and **SQL Server**. The application features a highly responsive UI built with **Bootstrap 5** and dynamic client-side interactions via **AJAX/jQuery**. It seamlessly unifies complex workflows including Sales, Inventory, Production, HR/Payroll, and Financial Reporting into a single, cohesive platform.



---

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

```

---

## 🗄️ Database Entity-Relationship (ER) Diagrams

The database is highly normalized and designed using **Entity Framework Core (Code-First)**. To keep the diagrams readable, the model is categorized into logical modules.

### 1. Inventory & Production Module
Tracks item stock, supplier deliveries, fabric issuing, and order lifecycles.

```mermaid
erDiagram
    ITEMS ||--o{ STOCKIN : "receives"
    ITEMS ||--o{ STOCKOUT : "issues"
    ITEMS ||--o{ WASTAGE : "scrap"
    SUPPLIERS ||--o{ STOCKIN : "supplies"
    
    ORDERS ||--o{ DEMANDALLOCATION : "requires"
    ORDERS ||--o{ FABRICISSUE : "assigned"
    ORDERS ||--o{ ORDERLIFECYCLE : "tracks"
    
    ITEMS {
        int Id PK
        string Name
        decimal Stock
        decimal Price
    }
    STOCKIN {
        int Id PK
        int ItemId FK
        int SupplierId FK
        decimal Quantity
    }
    FABRICISSUE {
        int Id PK
        int OrderId FK
        int ItemId FK
        decimal Quantity
    }
```

### 2. Services, Customers & HR Module
Manages customer orders, tailoring measurements, employee attendance, and payroll.

```mermaid
erDiagram
    CUSTOMERS ||--o{ ORDERS : "places"
    CUSTOMERS ||--o{ MEASUREMENTS : "has"
    ORDERS ||--o{ PAYMENTS : "pays"
    ORDERS ||--o{ MEASUREMENTS : "for"
    
    EMPLOYEES ||--o{ ATTENDANCE : "logs"
    EMPLOYEES ||--o{ LEAVE : "requests"
    EMPLOYEES ||--o{ PAYROLL : "earns"
    EMPLOYEES ||--o{ ADVANCE : "takes"
    
    ORDERS {
        int Id PK
        string OrderNumber
        int CustomerId FK
        decimal Total
        decimal Remaining
    }
    EMPLOYEES {
        int Id PK
        string Name
        decimal BaseSalary
    }
```

---

## 🛡️ Security, Policies, and Audit Trails

### Authentication & Authorization
The application is secured using **Cookie-Based Authentication** and strict **Role-Based Access Control (RBAC)**.

*   **RBAC Enforcement:** Controllers are decorated with `[Authorize(Roles="...")]`. For example, only the `Admin` and `HRManager` roles can access the Payroll module.
*   **Password Security:** User passwords are securely hashed using **SHA256** before being stored in the database. No plain text passwords exist in the system.
*   **Session Management:** Uses `.NET Core Session` with HttpOnly cookies to manage user logins and prevent XSS attacks.

### Automated Auditing Mechanism
The system features an automated, transactional auditing design at the database level:

*   **Global Exception Filter:** `GlobalExceptionFilter` catches unhandled errors gracefully across the entire application, preventing application crashes.
*   **Global Audit Filter:** `GlobalAuditFilter` automatically intercepts actions to log critical user activities (e.g., deleting records, creating orders) into the `ActivityLogs` table.
*   **Immutable Logs:** Tables like `ProductionLogs` and `AttendanceLogs` are designed as append-only logs for strict traceability.
```

---

