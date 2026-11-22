# Sistem Informasi Manajemen Perpustakaan SMA Pelita Bangsa

Aplikasi desktop Windows Forms berbasis C# untuk mengelola katalog buku, peminjaman, pengembalian, transaksi, dan laporan perpustakaan.

## Fitur

- Login dua peran: Pustakawan dan Siswa.
- Autentikasi username, password, dan role melalui tabel SQL Server `pengguna`.
- Hash password PBKDF2-SHA256 dengan salt acak per akun.
- CRUD katalog buku, pencarian, kategori, status ketersediaan, dan cover lokal.
- Peminjaman, pengembalian, kuota maksimal 3 pinjaman aktif, tenggat 7 hari, dan denda.
- Fallback file untuk data buku/transaksi saat SQL Server tidak tersedia.
- Login fail-closed: file `.txt` tidak pernah menjadi sumber autentikasi.

## Teknologi dan prasyarat

- Windows 10/11 x64.
- C# / .NET 10 Windows Forms.
- SQL Server Express, instance `SQLEXPRESS`, aktif.
- SQL Server Management Studio (SSMS).
- NuGet package `Microsoft.Data.SqlClient`.

## Struktur penting

```text
Services/DatabaseHelper.cs   koneksi dan operasi SQL Server
database/schema.sql          schema database SQL Server
database/seed.sql            9 buku dan 5 transaksi contoh
buku.txt                     fallback data buku
transaksi.txt                fallback data transaksi
Program.cs                   entry point dan self-check
```

## Setup SQL Server

1. Jalankan service **SQL Server (SQLEXPRESS)**.
2. Buka SSMS.
3. Connect ke `localhost\SQLEXPRESS` dengan Windows Authentication.
4. Buka dan jalankan `database/schema.sql`.
5. Buka dan jalankan `database/seed.sql`.
6. Pastikan database `pbo_perpustakaan` berisi tabel `pengguna`, `buku`, dan `transaksi`.

Aplikasi membuat akun demo saat tabel `pengguna` masih kosong:

```text
Pustakawan: admin / admin
Siswa:      siswa / siswa
```

Connection string default:

```text
Server=.\SQLEXPRESS;Database=pbo_perpustakaan;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=3;
```

Untuk mengatur koneksi sementara di PowerShell:

```powershell
$env:PERPUSTAKAAN_SQLSERVER_CONNECTION = "Server=localhost\SQLEXPRESS;Database=pbo_perpustakaan;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=3;"
```

Jangan commit connection string berisi password.

## Build dan jalankan

```powershell
dotnet restore
dotnet build -c Release
dotnet run
```

Self-check:

```powershell
dotnet run -- --test
```

Self-check menguji serialisasi model, pemuatan service, koneksi SQL Server, login benar/salah, role salah, user tidak dikenal, proteksi hapus, dan kuota pinjaman.

Tes database tidak tersedia:

```powershell
$env:PERPUSTAKAAN_SQLSERVER_CONNECTION = "Server=127.0.0.1,1;Database=missing;User ID=x;Password=x;TrustServerCertificate=True;Connection Timeout=1;"
dotnet run -- --test
Remove-Item Env:PERPUSTAKAAN_SQLSERVER_CONNECTION
```

## Publish Windows

```powershell
dotnet publish -c Release -r win-x64 --self-contained false -o publish/win-x64
```

Jika path sumber mengandung spasi/apostrof dan publish gagal dengan `MSB3094`, jalankan publish dari path staging tanpa karakter tersebut. Folder `bin/`, `obj/`, dan `publish/` tidak disimpan di Git.

## Data dan privasi

Data pada file teks, database seed, dan screenshot adalah data contoh tugas. Jangan commit password, connection string berisi password, hash/salt produksi, atau data anggota nyata.

## Dokumentasi

- `Draft PBO [Kel1].md` — laporan progres dan pengujian.
- `CHANGELOG.md` — riwayat perubahan.
- `database/schema.sql` — schema SQL Server.
- `database/seed.sql` — data awal buku dan transaksi.
