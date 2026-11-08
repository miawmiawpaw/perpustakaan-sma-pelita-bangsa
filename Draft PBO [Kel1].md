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

Fitur autentikasi memvalidasi username, password, dan peran melalui tabel `pengguna` pada MySQL; aplikasi tidak memakai kredensial hard-coded untuk menerima login. Password demo disimpan sebagai hash PBKDF2-SHA256 dengan salt acak, bukan plaintext. Akun demo awal (`admin`/`admin` untuk Pustakawan dan `siswa`/`siswa` untuk Siswa) hanya dibuat saat tabel akun masih kosong. Jika database tidak tersedia, login ditolak; fallback flat-file hanya berlaku untuk data buku/transaksi.

* **Pustakawan**: hak akses pengelolaan buku, sirkulasi, dan laporan.
* **Siswa**: hak akses terbatas untuk katalog dan pemantauan pinjaman.

### 4. Integrasi Database SQL (MySQL / MariaDB via XAMPP)

Telah diimplementasikan konektivitas MySQL / MariaDB (XAMPP) melalui `MySqlConnector` pada database `pbo_perpustakaan`. Skema meliputi tabel `pengguna`, `buku`, dan `transaksi`. Password akun tersimpan sebagai hash PBKDF2-SHA256 dengan salt unik. Data buku/transaksi memakai fallback flat-file saat server berhenti; autentikasi login **tidak** memakai fallback dan harus berhasil di MySQL.

### 2.2 Dokumentasi Progres

Dokumentasi tangkapan layar (*screenshot*) progres pengembangan tersimpan pada direktori `screenshots/`:

1. **Halaman Autentikasi Pengguna**: `screenshots/01_halaman_login.png`  
   Menampilkan antarmuka login dua peran dengan form validasi username dan password terenkapsulasi.
2. **Dashboard Pustakawan & Indikator Database**: `screenshots/02_dashboard_admin_mysql.png`  
   Menampilkan dashboard, metrik perpustakaan, dan indikator `[Penyimpanan: 🟢 MySQL XAMPP Aktif]`.
3. **Dashboard Siswa**: `screenshots/03_dashboard_siswa.png`  
   Menampilkan dashboard khusus peran Siswa.
4. **Verifikasi Data Relasional**: `screenshots/bukti_query_mysql.txt`  
   Hasil query tabel `pengguna`, `buku`, dan `transaksi` pada database `pbo_perpustakaan`; akun hanya tampilkan username/role, bukan hash atau salt.

### 2.3 Kendala yang Dihadapi dan Solusi

1. **Bug Kompilasi MSBuild saat Publish `.exe`**: Ditemukan galat `MSB3094` saat eksekusi `dotnet publish` yang disebabkan oleh karakter khusus (apostrof dan spasi) pada path direktori sumber.  
   *Solusi*: Proses staging build dipisahkan ke direktori terstandarisasi sebelum didistribusikan ke `publish/win-x64/`.
2. **Ketergantungan Eksternal Server Database**: Apabila XAMPP belum dijalankan oleh pengguna, aplikasi berpotensi gagal inisialisasi.  
   *Solusi*: Diimplementasikan kelas `DatabaseHelper` dengan pola *graceful degradation* (fallback otomatis ke flat file `.txt`).
3. **Sinkronisasi Data Dua Arah**: Perubahan data pada file teks perlu tetap konsisten dengan tabel SQL.  
   *Solusi*: Operasi `SimpanSemua()` melakukan penulisan ganda (*dual-write*) ke MySQL dan berkas `.txt`.

### 2.4 Rencana Pengembangan Selanjutnya

1. Menyiapkan installer Windows (`.msi` / Inno Setup) untuk mempermudah distribusi ke laboratorium sekolah.  
2. Menambahkan fitur ekspor laporan sirkulasi ke format PDF / Excel.  
3. Menyusun laporan akhir dan presentasi UAS Pemrograman Berorientasi Objek.
