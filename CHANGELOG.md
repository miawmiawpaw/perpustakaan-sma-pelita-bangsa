# Changelog

All notable project changes are recorded here. This project has no tagged releases yet.

## 2026-11-08 — MySQL-backed login and repository documentation

### Added

- `pengguna` MySQL table for login accounts and role.
- First-run creation of demo accounts only when `pengguna` is empty: `admin`/`admin` and `siswa`/`siswa`.
- PBKDF2-SHA256 password hashes with per-account cryptographically random salts; stored hash comparison uses fixed-time equality.
- Database authentication for username + password + selected role. Unknown users, wrong passwords, and role mismatches are rejected.
- Login fail-closed behavior when MySQL is unavailable. Flat-file fallback applies only to book and transaction data.
- `PERPUSTAKAAN_MYSQL_CONNECTION` environment override for local database configuration.
- `database/schema.sql` to create the MySQL/MariaDB schema.
- Self-check cases for valid demo credentials, wrong password, wrong role, unknown user, and unavailable database.
- README, project goals/remaining work, this changelog, and C#-only `.gitignore`.

### Changed

- Removed hard-coded login acceptance and quick-login bypasses.
- Removed demo passwords from the visible login screen; account administration stays outside the application for now.
- Updated `Draft PBO [Kel1].md` and refreshed login/dashboard/query evidence under `screenshots/`.
- Updated `MySqlConnector` to 2.6.2 after earlier package versions produced NuGet vulnerability warnings.

### Verification

- `dotnet build`: 0 warnings, 0 errors.
- `dotnet run -- --test`: passed with live MariaDB.
- Same self-check passed with a deliberately unavailable DB endpoint; authentication denied login.
- Published Windows executable launched; valid admin login reached dashboard and displayed active MySQL status.

### Known limitations / next work

- No UI for account creation, role changes, deactivation, or password reset. Extra accounts require direct DB administration and correctly computed PBKDF2 hash/salt.
- Development connection defaults to local XAMPP `root` with an empty password. Set `PERPUSTAKAAN_MYSQL_CONNECTION` to a least-privilege account before deployment.
- Book/transaction writes currently update both MySQL and text files, but no transactional recovery exists if one side fails mid-write; fallback files can become stale.
- Dates/statuses remain strings in the current DB schema.
- End-to-end UI coverage for every CRUD and circulation path remains incomplete.
- Demo transaction data contains example borrower names; repository is private. Anonymize before making repository public.
