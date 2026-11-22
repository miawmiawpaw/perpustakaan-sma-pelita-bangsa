**‘’PENGEMBANGAN SISTEM INFORMASI MANAJEMEN PERPUSTAKAAN SMA PELITA BANGSA BERBASIS C\#’’**

Disusun untuk memenuhi tugas Ujian Akhir Semester pada mata kuliah: Pemograman Berbasis Objek

Dosen Pengampu: Arya

 

 

 

 

 

 

 

 

*Disusun oleh:*

  **Rena Ade Widya  		              2507431021**

**Fadlan Ahmad Kamil  		2507431064**

              **Ribby Julina Bangun  		  25074310**

**PROGRAM STUDI TEKNIK MULTIMEDIA DIGITAL**

**POLITEKNIK NEGERI JAKARTA**

**2026**

1.1 Latar Belakang

Perkembangan teknologi informasi telah memberikan kemudahan dalam berbagai bidang, salah satunya dalam pengelolaan data dan administrasi perpustakaan. Pengelolaan perpustakaan secara manual masih memiliki berbagai keterbatasan, terutama dalam pencatatan data buku, proses peminjaman dan pengembalian, serta pencarian informasi koleksi buku.

Pada studi kasus SMA Pelita Bangsa, perpustakaan memiliki lebih dari 5.000 koleksi buku yang masih dikelola menggunakan buku catatan secara manual. Kondisi tersebut dapat menimbulkan berbagai permasalahan, seperti antrean panjang, kesulitan melacak riwayat peminjaman, risiko kehilangan data, serta proses pencarian buku yang membutuhkan waktu cukup lama.

Berdasarkan permasalahan tersebut, dikembangkan sebuah aplikasi manajemen perpustakaan berbasis C\# (C-Sharp) dengan menerapkan konsep Pemrograman Berorientasi Objek (Object-Oriented Programming). Aplikasi ini dirancang untuk membantu pengelolaan data buku, pencarian koleksi, serta pencatatan transaksi peminjaman dan pengembalian buku secara digital.

Melalui pengembangan aplikasi ini, diharapkan pengelolaan perpustakaan SMA Pelita Bangsa menjadi lebih efektif, terstruktur, dan efisien. Selain itu, proyek ini juga menjadi bentuk penerapan konsep Pemrograman Berorientasi Objek dalam membangun perangkat lunak.

### 1.2 Rumusan Masalah

1. Bagaimana mengatasi pengelolaan data perpustakaan yang masih dilakukan secara manual?  
2. Bagaimana mempermudah proses pencarian dan pengelolaan koleksi buku?  
3. Bagaimana mengelola data peminjaman dan pengembalian buku secara digital?  
4. Bagaimana menerapkan konsep Pemrograman Berorientasi Objek menggunakan bahasa C\#?

### 1.3 Tujuan Pengembangan

1. Mengembangkan aplikasi manajemen perpustakaan berbasis C\#.  
2. Mempermudah pengelolaan dan pencarian data buku.  
3. Membantu pencatatan transaksi peminjaman dan pengembalian.  
4. Menerapkan konsep Pemrograman Berorientasi Objek dalam pengembangan aplikasi.

## **BAB II. PROGRES PENGERJAAN**

### **2.1 Progres Pengembangan**

### Berikut merupakan beberapa perkembangan yang telah dilakukan selama proses pengembangan aplikasi manajemen perpustakaan SMA Pelita Bangsa:

### 1\. Pengembangan Aplikasi Menggunakan C\#

### Aplikasi manajemen perpustakaan dikembangkan menggunakan bahasa pemrograman C\# dengan menerapkan konsep Pemrograman Berorientasi Objek (Object-Oriented Programming).

### 2. Uji Coba Kompilasi Aplikasi Menjadi File `.exe`

Aplikasi berhasil dikompilasi menjadi berkas executable mandiri (`PerpustakaanPelitaBangsa.exe`) berbasis .NET 10 Windows Forms (`win-x64`) pada direktori `publish/win-x64/`. Kendala path MSBuild akibat karakter spasi/apostrof telah diidentifikasi dan diselesaikan sehingga executable dapat didistribusikan dan dijalankan secara langsung tanpa crash.

### 3. Penambahan Fitur Login Multi-Peran

Fitur autentikasi memvalidasi username, password, dan peran melalui tabel `pengguna` pada SQL Server. Password demo disimpan sebagai hash PBKDF2-SHA256 dengan salt acak. Akun demo awal (`admin`/`admin` untuk Pustakawan dan `siswa`/`siswa` untuk Siswa) hanya dibuat saat tabel akun masih kosong. Jika database tidak tersedia, login ditolak; fallback flat-file hanya berlaku untuk data buku/transaksi.

* **Pustakawan**: hak akses pengelolaan buku, sirkulasi, dan laporan.
* **Siswa**: hak akses terbatas untuk katalog dan pemantauan pinjaman.

### 4. Integrasi Database SQL Server Express

Konektivitas menggunakan `Microsoft.Data.SqlClient` ke instance `localhost\SQLEXPRESS` pada database `pbo_perpustakaan`. `database/schema.sql` membuat tabel `pengguna`, `buku`, dan `transaksi`; `database/seed.sql` memasukkan 9 buku dan 5 transaksi contoh. Data buku/transaksi memakai fallback flat-file saat server berhenti; autentikasi login tidak memakai fallback.

### 2.2 Dokumentasi Progres

Dokumentasi pengujian dan tangkapan layar tersimpan pada direktori `screenshots/`:

1. **Tampilan halaman login saat aplikasi berjalan**: `screenshots/01_halaman_login.png`.
2. **Dashboard Pustakawan dengan koneksi SQL Server aktif**: `screenshots/02_dashboard_admin_mysql.png`.
3. **Dashboard Siswa dengan navigasi terbatas**: `screenshots/03_dashboard_siswa.png`.
4. **Penolakan kredensial salah**: `screenshots/04_login_rejected.png`; dialog menampilkan `Username, password, atau role salah.`.
5. **Katalog Siswa read-only**: `screenshots/05_siswa_katalog_readonly.png`; buku dan filter terlihat, tombol CRUD/sirkulasi tersembunyi.
6. **Riwayat peminjaman Siswa**: `screenshots/06_siswa_peminjaman_readonly.png`; aksi pinjam/kembali tersembunyi. Data contoh tidak memiliki transaksi atas nama `siswa`, sehingga tabel kosong; penyaringan data pemilik diperiksa pada kode.
7. **Manajemen buku Pustakawan**: `screenshots/07_admin_manajemen_buku.png`; menampilkan 9 buku dan kontrol tambah/edit/hapus/sirkulasi.
8. **Kompilasi proyek**: `screenshots/08_dotnet_build_success.png`; hasil `dotnet build`, 0 warning dan 0 error.
9. **Publikasi executable Windows**: `screenshots/09_dotnet_publish_success.png`; hasil publish untuk `win-x64` ke `publish/win-x64/` melalui direktori staging.
10. **Implementasi autentikasi database**: `screenshots/10_kode_autentikasi_database.png`; menampilkan sebagian method `Autentikasi` dan hash PBKDF2. Detail lengkap ada di `Services/DatabaseHelper.cs`.

11. **Gallery kontrol reusable**: `screenshots/27_gallery_disabled_focus.png`; rounded panel/button/input, status badge, disabled button, dan state fokus input teruji.
12. **Audit login final**: `screenshots/18_login_audit_final.png`; hero, field, role selector, tombol masuk, dan spacing tanpa clipping.
13. **Audit dashboard**: `screenshots/19_dashboard_audit_final.png`; metric card, tabel transaksi, denda, sidebar, dan koleksi populer.
14. **Audit Data Buku**: `screenshots/20_data_buku_audit.png`; toolbar, dropdown, tabel, badge status, dan tombol aksi.
15. **Audit Pencarian Buku**: `screenshots/21_pencarian_audit.png`; search, filter kategori, hasil, panel detail, dan aksi pinjam.
16. **Audit Peminjaman**: `screenshots/22_peminjaman_audit_final.png`; filter, tombol aksi, tabel, dan status badge compact.
17. **Audit Anggota**: `screenshots/23_anggota_audit.png`; direktori anggota, kuota, dan status.
18. **Audit Laporan**: `screenshots/24_laporan_audit.png`; kartu ringkasan dan tabel denda.
19. **Audit Dialog Tambah Buku**: `screenshots/25_tambah_buku_audit_final.png`; enam field, dropdown kategori, validasi layout, dan footer tombol.
20. **Audit Dialog Peminjaman**: `screenshots/26_pinjam_dialog_audit_final.png`; dropdown buku, input peminjam, notice aturan lengkap, dan tombol aksi.
21. **Login setelah fitur cover**: `screenshots/28_cover_feature_login.png`; startup UI tetap terbaca setelah penambahan fitur foto.
22. **Detail buku dengan cover**: `screenshots/29_book_detail_cover.png`; cover `Laskar Pelangi` tampil lokal bersama metadata dan tombol pinjam.
23. **Katalog buku dengan data cover**: `screenshots/30_book_catalog_cover.png`; data buku tetap termuat setelah path cover ditambahkan.
**Matriks uji aktual**:

| Skenario | Hasil |
|---|---|
| `dotnet build` | Lulus; 0 warning, 0 error |
| Self-check dengan SQL Server Express aktif | Lulus; koneksi `localhost\\SQLEXPRESS`, 9 buku, 5 transaksi |
| Login admin dan siswa dengan akun demo | Lulus; keduanya terautentikasi via SQL Server dan hash PBKDF2-SHA256 |
| Password salah, username tak dikenal, role salah | Ditolak |
| DB tak tersedia | Login ditolak; fallback hanya memuat data buku/transaksi dari file |
| Login admin melalui executable hasil publish | Lulus; dashboard menunjukkan SQL Server aktif |
| Login siswa melalui executable hasil publish | Lulus; Anggota/Laporan dan kontrol mutasi/sirkulasi tersembunyi |
| Audit UI login, dashboard, Data Buku, Pencarian, Peminjaman, Anggota, Laporan | Lulus visual; tidak ada clipping/overlap utama pada screenshot aktual |
| Audit state kontrol reusable | Lulus; default, disabled, focus input, rounded, dan kontras primary diuji di UiGallery |
| Audit dialog Tambah Buku dan Peminjaman | Lulus visual setelah spacing, tinggi dialog, placeholder, dan notice aturan diperbaiki |
| Fitur foto cover buku | Lulus; cover lokal dibaca dari `book-covers/`, preview detail tampil tanpa clipping |
| Input foto saat Tambah/Edit Buku | Lulus build; picker native, preview, hapus foto, validasi file gambar, dan penyimpanan path tersedia |
| Seed SQL Server | Lulus; 9 buku dan 5 transaksi masuk melalui `database/seed.sql` |

**Perintah uji ulang**:

```powershell
dotnet build --nologo -v q
dotnet run --no-build -- --test
$env:PERPUSTAKAAN_SQLSERVER_CONNECTION = 'Server=127.0.0.1,1;Database=missing;User ID=x;Password=x;TrustServerCertificate=True;Connection Timeout=1;'
dotnet run --no-build -- --test
```

Perintah terakhir menguji mode database tidak tersedia; hasil yang dicatat ialah autentikasi gagal tertutup, sementara data buku/transaksi memakai fallback berkas.

### 2.3 Kendala yang Dihadapi dan Solusi

1. **Bug Kompilasi MSBuild saat Publish `.exe`**: Ditemukan galat `MSB3094` saat eksekusi `dotnet publish` yang disebabkan oleh karakter khusus (apostrof dan spasi) pada path direktori sumber.  
   *Solusi*: Proses staging build dipisahkan ke direktori terstandarisasi sebelum didistribusikan ke `publish/win-x64/`.
2. **Ketergantungan Eksternal Server Database**: Apabila SQL Server Express belum dijalankan oleh pengguna, aplikasi berpotensi gagal inisialisasi.
   *Solusi*: Diimplementasikan kelas `DatabaseHelper` dengan pola *graceful degradation* (fallback otomatis ke flat file `.txt`).
3. **Sinkronisasi Data Dua Arah**: Perubahan data pada file teks perlu tetap konsisten dengan tabel SQL.
   *Solusi*: Operasi `SimpanSemua()` melakukan penulisan ganda (*dual-write*) ke SQL Server dan berkas `.txt`.

### 2.4 Rencana Pengembangan Selanjutnya

1. Menyiapkan installer Windows (`.msi` / Inno Setup) untuk mempermudah distribusi ke laboratorium sekolah.  
2. Menambahkan fitur ekspor laporan sirkulasi ke format PDF / Excel.  
3. Menyusun laporan akhir dan presentasi UAS Pemrograman Berorientasi Objek.
