# ArcTriggerAlphaBackend

Backend system for the ArcTrigger Rvol Alpha trading engine.

---

## ✅ Purpose

Stores users, trade orders, stock snapshots, and update tasks.  
Designed for precision control in an automated trading assistant.

---

## 🧱 Tech Stack

- .NET 9  
- Entity Framework Core 9  
- SQLite (code-first)  
- Manual instantiation (no DI)

---

## 📦 Core Components

### Entities:
- `User` – login with email & hashed password  
- `Order` – trading order (original, alpha gen, second entry)  
- `TradeSetup` – container for full trade config  
- `Stock` – price/volume snapshot at market time  
- `StockUpdateTask` – background polling config per stock  

### Database:
- `MinimalContext` – SQLite DbContext  
  - Configures tables, FK, unique index (email)

---

## 🏁 Setup Instructions

1. Run migration:
```bash
dotnet ef migrations add Init
dotnet ef database update
