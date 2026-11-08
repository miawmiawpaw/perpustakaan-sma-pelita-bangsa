# Sistem Informasi Manajemen Perpustakaan SMA Pelita Bangsa

Aplikasi desktop Windows Forms berbasis C# untuk mengelola katalog buku, peminjaman, pengembalian, transaksi, dan laporan perpustakaan. Project ini dikembangkan untuk tugas UAS Pemrograman Berorientasi Objek.

## Fitur

- Login dua peran: Pustakawan dan Siswa.
- Autentikasi username, password, dan role melalui tabel MySQL `pengguna`.
- Password akun disimpan sebagai PBKDF2-SHA256 hash dengan salt acak per akun.
- Pengelolaan katalog: tambah, ubah, hapus aman, pencarian, kategori, dan status ketersediaan.
- Sirkulasi peminjaman dan pengembalian, kuota maksimal 3 pinjaman aktif per siswa, dan pemeriksaan peminjaman ganda.
- Tenggat pinjam 7 hari dan denda keterlambatan Rp1.000 per hari.
- Dashboard, statistik koleksi, riwayat transaksi, dan laporan denda.
- Penyimpanan utama buku/transaksi di MySQL/MariaDB; fallback data buku/transaksi ke `buku.txt` dan `transaksi.txt` ketika MySQL tidak tersedia.
- Login **fail-closed**: kredensial tidak dapat diverifikasi dan login ditolak saat MySQL tidak tersedia. File `.txt` tidak pernah menjadi sumber autentikasi.

## Teknologi

- C# / .NET 10 Windows Forms, target `net10.0-windows`.
- MySQL atau MariaDB melalui XAMPP.
- NuGet package `MySqlConnector`.
- File teks pipe-delimited sebagai fallback data buku/transaksi.

## Prasyarat

1. Windows 10/11 x64.
2. .NET 10 SDK untuk build dari source.
3. XAMPP dengan MySQL/MariaDB aktif untuk login dan database.
4. Akses NuGet saat pertama restore package.

## Struktur project

```text
.
├── Controls/                 # Kontrol visual, grafik dashboard
├── Forms/                    # MainForm dan dialog aplikasi
├── Helpers/                  # Helper UI
├── Models/                   # Buku, Transaksi, Pengguna, role
├── Services/                 # PerpustakaanService dan DatabaseHelper
├── database/schema.sql       # Skema tabel MySQL
├── screenshots/              # Bukti UI dan query database
├── buku.txt                  # Data cadangan buku
├── transaksi.txt             # Data cadangan transaksi
├── Draft PBO [Kel1].md       # Latar belakang, progres, kendala, dan rencana
├── Program.cs                # Entry point dan self-check
└── C#.csproj
```

Repository ini hanya berisi project C#. Direktori project Java lama dikecualikan.

## Menyiapkan database lokal

1. Buka XAMPP Control Panel, jalankan **MySQL**.
2. Jalankan SQL di `database/schema.sql` melalui phpMyAdmin atau terminal MySQL. Aplikasi juga membuat tabel `pengguna` jika belum ada.
3. Secara default, koneksi lokal memakai `localhost:3306`, database `pbo_perpustakaan`, user `root`, password kosong (konfigurasi XAMPP lokal umum).
4. Saat tabel `pengguna` kosong, startup pertama mengisi dua akun demo dengan salt acak dan hash PBKDF2-SHA256:
   - Pustakawan: `admin` / `admin`
   - Siswa: `siswa` / `siswa`
5. Jika tabel sudah berisi akun, aplikasi tidak menambah ulang akun demo.

Untuk koneksi lokal yang berbeda, set environment variable `PERPUSTAKAAN_MYSQL_CONNECTION` sebelum menjalankan aplikasi. Contoh PowerShell:

```powershell
$env:PERPUSTAKAAN_MYSQL_CONNECTION = "Server=localhost;Port=3306;Database=pbo_perpustakaan;User ID=perpustakaan_app;Password=GANTI_PASSWORD;Connection Timeout=3;"
```

Gunakan akun database berhak minimum dan password kuat di luar mesin lokal. Jangan gunakan `root` tanpa password untuk deployment. Jangan commit connection string berisi password.

> Akun demo hanya untuk pengujian, bukan akun produksi. Belum ada UI untuk membuat, mengubah, menonaktifkan akun, atau mengganti password. Jangan gunakan password demo di lingkungan nyata.

## Build dan jalankan

Dari folder project:

```powershell
dotnet restore
dotnet build -c Release
dotnet run
```

Kompilasi executable Windows x64:

```powershell
dotnet publish -c Release -r win-x64 --self-contained false -o publish/win-x64
```

Hasil publish adalah folder aplikasi, bukan satu file mandiri; .NET Desktop Runtime harus tersedia pada komputer target. Direktori `bin/`, `obj/`, dan `publish/` tidak disimpan di Git.

Jika `dotnet publish` dari path project yang mengandung apostrof/spasi gagal dengan `MSB3094`, jalankan publish dari path staging tanpa karakter tersebut. Contoh:

```powershell
$stage = Join-Path $env:TEMP "pbo_perpustakaan_build"
Remove-Item $stage -Recurse -Force -ErrorAction SilentlyContinue
New-Item $stage -ItemType Directory | Out-Null
Copy-Item .\* $stage -Recurse -Force -Exclude JAVA,bin,obj,publish
Push-Location $stage
dotnet publish -c Release -r win-x64 --self-contained false -o publish/win-x64
Pop-Location
```

Periksa dan salin folder hasil publish dari staging setelah build berhasil.

## Pemeriksaan cepat

Self-check aplikasi menguji serialisasi model, pemuatan service, koneksi DB, kredensial demo benar, password salah, role salah, username tak dikenal, proteksi hapus, dan kuota pinjaman.

```powershell
dotnet run -- --test
```

Dengan MySQL aktif, self-check harus menunjukkan login MySQL berhasil. Untuk menguji penolakan saat koneksi DB gagal, atur sementara koneksi ke port tak aktif:

```powershell
$env:PERPUSTAKAAN_MYSQL_CONNECTION = "Server=127.0.0.1;Port=1;Database=missing;User ID=x;Password=x;Connection Timeout=1;"
dotnet run -- --test
Remove-Item Env:PERPUSTAKAAN_MYSQL_CONNECTION
```

Login harus ditolak saat MySQL tidak tersedia; fallback file hanya melayani data buku dan transaksi.

## Data dan privasi

`buku.txt`, `transaksi.txt`, dan tangkapan layar adalah data contoh untuk tugas. Transaksi contoh memuat nama peminjam. Repository dibuat **private**; sebelum membuatnya public, anonimisasi data pribadi dan periksa seluruh screenshot/dokumen. Jangan menambahkan dump database berisi password hash/salt, data anggota nyata, atau kredensial.

## Status goals

### Selesai

- Aplikasi C# Windows Forms dan konsep PBO.
- Login multi-peran diverifikasi oleh MySQL, hash PBKDF2-SHA256 + salt acak.
- Fail-closed untuk autentikasi saat database tidak tersedia.
- Database `pbo_perpustakaan` dengan tabel `pengguna`, `buku`, dan `transaksi`.
- Fallback file buku/transaksi ketika MySQL mati.
- Uji build, self-check autentikasi berhasil/gagal/DB unavailable, dan smoke run executable Windows.
- Laporan progres dan screenshot pengembangan.

### Perlu dilanjutkan

- UI admin untuk membuat, mengubah, menonaktifkan akun, dan reset password; saat ini akun tambahan dikelola di database.
- Simpan konfigurasi koneksi di konfigurasi lokal yang tidak di-commit, lalu hilangkan default akun DB `root` tanpa password.
- Pastikan operasi buku/transaksi konsisten saat MySQL gagal di tengah dual-write; fallback file dapat menjadi stale jika ada perubahan ketika server mati.
- Migrasi dari status tanggal/string ke tipe SQL `DATE` serta `CHECK`/constraint yang sesuai.
- Uji end-to-end UI untuk seluruh alur CRUD, pinjam, kembali, validasi login dan akses per role.
- Evaluasi ulang data contoh/identitas sebelum repository dibuka ke publik.
- Finalisasi installer dan instruksi deployment komputer lab.

## Dokumentasi dan bukti

- `Draft PBO [Kel1].md` — latar belakang, progres, kendala, dan rencana pengerjaan.
- `screenshots/01_halaman_login.png` — antarmuka login.
- `screenshots/02_dashboard_admin_mysql.png` — dashboard admin dengan indikator DB.
- `screenshots/03_dashboard_siswa.png` — dashboard Siswa.
- `screenshots/bukti_query_mysql.txt` — username/role demo dan jumlah record; tidak berisi hash/salt.

## Perubahan terbaru

Lihat [`CHANGELOG.md`](CHANGELOG.md).
