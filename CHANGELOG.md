# Changelog

All notable project changes are recorded here. This project has no tagged releases yet.

## 2026-11-22 — SQL Server Express migration

### Changed

- Replaced `MySqlConnector` with `Microsoft.Data.SqlClient`.
- Migrated `DatabaseHelper` to SQL Server syntax and `localhost\SQLEXPRESS` Windows Authentication.
- Migrated schema and seed data to SQL Server.
- Added `database/seed.sql` with 9 catalog books, 5 example transactions, and cover paths.
- Configured `book-covers/**/*.jpg` to copy into build and publish output.
- Assigned available covers to `BK-001`–`BK-006` and `BK-009`; `BK-007` and `BK-008` remain without covers because no matching files exist.
- Updated application and project documentation from MySQL/MariaDB to SQL Server Express.

### Verification

- `dotnet build --no-restore`: 0 warnings, 0 errors.
- SQL Server schema executed successfully in SSMS.
- Local SQL Server instance `MSSQL$SQLEXPRESS` observed running.
- Self-check passed with 9 books and 5 transactions loaded from SQL Server.

### Setup

```text
Server=localhost\SQLEXPRESS;Database=pbo_perpustakaan;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=3;
```

Demo accounts remain `admin` / `admin` and `siswa` / `siswa`; passwords are seeded as PBKDF2-SHA256 hashes by the application.
