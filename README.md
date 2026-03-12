# 💰 SmartWallet - Advanced Fintech Portfolio Project

**SmartWallet** to nowoczesna aplikacja webowa do zarządzania finansami, zbudowana w technologii **ASP.NET Core MVC**. Projekt symuluje realny system bankowy, kładąc nacisk na architekturę bezpieczeństwa, workflow transakcji oraz integralność danych.

## 🚀 Kluczowe Funkcjonalności

### 🛡️ Zaawansowany Workflow Transakcji
Aplikacja implementuje system dwuetapowej weryfikacji płatności:
- **Zlecenie:** Każdy przelew trafia do kolejki ze statusem `Pending`.
- **Weryfikacja:** Administratorzy posiadają dedykowany panel do akceptacji lub odrzucania operacji.
- **Księgowanie:** Środki są przesyłane między portfelami dopiero po finalnym zatwierdzeniu przez system.

### 📊 Panel Administratora (Admin Dashboard)
- **Zarządzanie Użytkownikami:** Podgląd sald, usuwanie kont z zachowaniem spójności bazy danych.
- **Filtrowanie Danych:** Zaawansowany system filtrowania transakcji po statusach (`Pending`, `Completed`, `Cancelled`).
- **Bezpieczeństwo sesji:** Mechanizmy zapobiegające usunięciu własnego konta przez aktywnego administratora.

### 👤 Strefa Użytkownika
- **Dynamiczny Dashboard:** Wizualizacja aktualnego salda i historii operacji.
- **Inteligentne Formularze:** Walidacja po stronie serwera i klienta, automatyczne bindowanie kategorii płatności.

## 🛠️ Stack Technologiczny
- **Backend:** .NET 8/9, ASP.NET Core MVC
- **Baza danych:** Entity Framework Core (SQL Server / Azure SQL)
- **Identity:** Zaawansowana konfiguracja ASP.NET Core Identity (Role-Based Access Control)
- **Frontend:** Bootstrap 5, Razor Views, Bootstrap Icons
- **Zasady:** SOLID, DRY, KISS

## ⚙️ Instrukcja Uruchomienia
1. Sklonuj repozytorium.
2. Projekt posiada wbudowany **Auto-Migration & Seeding** – wystarczy uruchomić aplikację, a baza danych i role zostaną utworzone automatycznie.
3. **Dane do logowania (Test Admin):**
   - **Login:** `admin@smartwallet.pl`
   - **Hasło:** `Admin123!`
   - *(Admin posiada doładowane konto testowe na start)*

## 🧠 Wyzwania i Rozwiązania
- **Integralność bazy:** Obsługa usuwania użytkowników powiązanych z historią transakcji (Cascade Delete/Cleanup).
- **Zarządzanie stanem:** Rozwiązanie problemu wygasania danych w formularzach (ViewBag/SelectList) po błędach walidacji.
- **Bezpieczeństwo:** Implementacja opcjonalnej trwałości sesji (Remember Me) oraz ochrona przed usunięciem konta zalogowanego admina.

---
*Projekt stworzony jako pokaz umiejętności technicznych w ekosystemie .NET.*
