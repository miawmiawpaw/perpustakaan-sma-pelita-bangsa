# Changelog

All notable project changes are recorded here. This project has no tagged releases yet.

## 2026-11-22 — SQL Server Express migration

### Changed

- Replaced `MySqlConnector` with `Microsoft.Data.SqlClient`.
- Migrated `DatabaseHelper` to SQL Server syntax and `localhost\SQLEXPRESS` Windows Authentication.
- Replaced MySQL schema types and clauses with SQL Server equivalents.
- Replaced `REPLACE INTO` writes with SQL Server-compatible update-then-insert statements.
- Updated self-check messages and project documentation from MySQL/MariaDB to SQL Server Express.
- Added `database/seed.sql` with 9 catalog books and 5 example transactions.

### Verification

- `dotnet restore` succeeded.
- `dotnet build --no-restore` succeeded with 0 warnings and 0 errors.
- SQL Server schema executed successfully in SSMS.
- Local SQL Server instance `MSSQL$SQLEXPRESS` was observed running.

### Setup

```text
Server=localhost\SQLEXPRESS;Database=pbo_perpustakaan;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=3;
```

Demo accounts remain `admin` / `admin` and `siswa` / `siswa`; passwords are seeded as PBKDF2-SHA256 hashes by the application.

## 2026-11-08 — MySQL-backed login and repository documentation

Historical entry. The application has since migrated to SQL Server Express.
