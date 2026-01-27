# Milla-Web-DB

Database design and SQL implementation for the **MillaWeb** full-stack e-commerce project.

This repository contains the database schema, relationships, and SQL scripts used to support core e-commerce operations such as product management, orders, payments, and stock control.  
The project focuses on clean relational design and reliable data handling using Microsoft SQL Server.

---

## Overview

- Relational database design following **3NF**
- Modular SQL scripts for maintainability
- Support for transactional operations and data consistency
- Designed as part of a full-stack ASP.NET Core application

---

## Contents

- Database creation scripts
- Table definitions and relationships
- Seed / sample data
- Views, stored procedures, and triggers
- Visual Studio solution for the application layer

---

## Technologies

- **Microsoft SQL Server (T-SQL)**
- **ASP.NET Core / C#**
- **HTML**

---

## Repository Structure

- `MillaWeb/`  
  Application solution and backend logic

- `SqlScripts/`  
  Modular SQL scripts (tables, constraints, seed data, views, procedures)

- `script.sql`  
  Single consolidated script for quick database setup

- `MillaWeb.sln`  
  Visual Studio solution file

---

## Database Setup

### Option 1 — Single Script

1. Open SQL Server Management Studio or Azure Data Studio.
2. Open `script.sql` from the repository root.
3. Execute the script to create and populate the database.

> Database name or connection settings can be adjusted if needed.

---

### Option 2 — Modular Scripts

1. Navigate to the `SqlScripts/` directory.
2. Execute scripts in logical order:
   - Table creation
   - Constraints and relationships
   - Seed data
   - Views, stored procedures, and triggers

---

## Running the Application

1. Open `MillaWeb.sln` in Visual Studio.
2. Update the database connection string in the configuration file.
3. Run the project.

---

## Notes

- Designed primarily for learning and hands-on practice with relational databases.
- Scripts are intended for Microsoft SQL Server.
- Re-running scripts may require dropping existing objects.

---

## Author

Berat Metehan Çakmak  
Computer Engineering Student
