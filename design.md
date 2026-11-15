# design.md — Sistem Perpustakaan Digital SMA Pelita Bangsa

Dokumen ini adalah **sumber kebenaran tunggal** untuk tampilan aplikasi desktop (C# / Windows).
Semua ukuran, warna, dan jarak HARUS diambil dari token di bawah. Jangan menulis angka literal di Designer/XAML/kode UI.

- **Platform:** Windows desktop, C# (.NET 8). Asumsi: Windows Forms. Jika WPF, pakai token yang sama sebagai `ResourceDictionary`.
- **Karakter visual:** bersih, tenang, mirip aplikasi akademik. Navy sebagai identitas, banyak ruang putih, satu aksi utama per layar.
- **Prinsip:** konsisten > selera pribadi; semua nilai dari skala 4/8pt; kontras WCAG 2.2 AA; jangan mengubah teks/fungsi yang sudah ada.

---

## 1. Masalah di UI saat ini (yang harus diperbaiki)

| # | Layar | Masalah | Perbaikan |
|---|---|---|---|
| 1 | Login | Judul "Masuk ke Akun Anda" menimpa subjudul; headline kiri menimpa deskripsi; deskripsi terpotong ("proteksi kuota,") | Susun dengan `TableLayoutPanel`/`FlowLayoutPanel` (tanpa posisi absolut), label `AutoSize`, jarak antar teks 8–16 |
| 2 | Login | Kolom Username/Password hampir tak terlihat (abu muda di atas putih, tanpa border) | Input: border 1px `border-strong` (3.46:1), radius 8, tinggi 40, fokus ring primary |
| 3 | Login | Panel "Akun Demo" kosong & tinggi tak perlu; form tidak di tengah vertikal | Hapus ruang kosong, form dipusatkan vertikal, lebar form maks 400 |
| 4 | Semua | Item sidebar aktif tidak konsisten (Dashboard: blok kosong di atas menu; Data Buku abu, Pencarian biru) | Satu gaya aktif: latar `primary-50`, teks `primary-700`, bar kiri 3px primary |
| 5 | Semua | Subjudul header: spasi ganda & "[Penyimpanan: ● MySQL XAMPP Aktif]" dalam kurung | Pisah jadi *status chip* kecil di kanan header (titik hijau + "MySQL terhubung") |
| 6 | Dashboard | Panel "Koleksi Terpopuler" kosong total | Isi daftar peringkat (judul + jumlah pinjam) atau **empty state** dengan ikon + teks |
| 7 | Dashboard | "Total Denda Rp 21.000" berwarna hijau (hijau = sukses, denda bukan hal positif) | Angka netral (`neutral-900`), ikon chip amber |
| 8 | Dashboard | Status transaksi hanya teks; "Dikembalikan (Denda: Rp 3.000)" membungkus dua baris | Badge status + kolom **Denda** terpisah |
| 9 | Data Buku | Status memakai sel berwarna penuh (hijau/merah memenuhi sel) | Badge pill di dalam sel, latar baris tetap netral |
| 10 | Data Buku | Teks seluruh baris pertama berwarna biru (state hover/terpilih bocor ke teks) | Baris terpilih: latar `primary-50`, teks tetap `neutral-900` |
| 11 | Data Buku | "Rak B-12, Lantai 1" membungkus 2 baris; header "Judul Buku" membungkus | Lebar kolom ditetapkan, `Judul` fleksibel (Fill), rak min. 160 |
| 12 | Data Buku | Tabel terpotong di bawah (BK-009 tak terlihat), tombol aksi menempel di tepi | Tabel `Dock=Fill` dalam kontainer, tombol aksi di toolbar atas atau footer tetap dengan jarak 16 |
| 13 | Data Buku | Tinggi toolbar tak seragam (cari ±24, dropdown ±28, tombol ±34) | Semua kontrol toolbar tinggi **36** |
| 14 | Tambah Buku | Tombol "Batal" menempel & terpotong di tepi kanan dialog; label menempel ke input; kotak input persegi hitam | Padding dialog 24, label↔input 4, antar field 16, tombol rata kanan, input gaya standar |
| 15 | Pencarian | Kolom pencarian hanya garis bawah; judul detail punya strip putih; panel detail banyak ruang kosong | Input gaya standar; panel detail = card dengan padding 24 |
| 16 | Semua | Emoji sebagai ikon (🤚, 🔍) dengan ukuran & warna acak | Ikon font **Segoe Fluent Icons** (fallback Segoe MDL2 Assets), 20px, warna `neutral-600` |

---

## 2. Design tokens

### 2.1 Warna

**Primary (navy, identitas yang sudah ada)**

| Token | Hex | Pemakaian |
|---|---|---|
| `primary-900` | `#0F2A5C` | Latar panel kiri login (lebih gelap) |
| `primary-700` | `#1E3A8A` | **Tombol utama, header tabel, teks aktif, logo** |
| `primary-800` | `#1E40AF` | Hover tombol utama |
| `primary-600` | `#1D4ED8` | Link, fokus ring |
| `primary-100` | `#E0E7FF` | Pressed item ringan, avatar |
| `primary-50` | `#EFF6FF` | Item sidebar aktif, baris terpilih/hover tabel |

**Neutral (slate)**

| Token | Hex | Pemakaian |
|---|---|---|
| `neutral-0` | `#FFFFFF` | Card, dialog, panel kanan login |
| `neutral-50` | `#F8FAFC` | Latar jendela/konten, zebra baris |
| `neutral-100` | `#F1F5F9` | Tombol sekunder, hover netral |
| `neutral-200` | `#E2E8F0` | Divider, border card |
| `border-strong` | `#7C8BA1` | **Border input & kontrol** (kontras 3.46:1 ≥ 3:1) |
| `neutral-500` | `#64748B` | Teks sekunder/placeholder (4.76:1 di atas putih) |
| `neutral-600` | `#475569` | Ikon, label |
| `neutral-900` | `#0F172A` | Teks utama |

> Aturan: teks `neutral-500` hanya di atas `neutral-0` / `neutral-50`. Jangan di atas `neutral-100` (4.34:1, gagal).
> `#94A3B8` dan `#CBD5E1` **bukan untuk teks atau border input** (kontras < 3:1); hanya untuk dekorasi/disabled.

**Semantic (selalu dipasangkan dengan ikon/teks, bukan warna saja)**

| Makna | Teks | Latar | Kontras | Dipakai untuk |
|---|---|---|---|---|
| Success | `#047857` | `#D1FAE5` | 4.84:1 | Status **Tersedia**, Dikembalikan, "Terhubung" |
| Warning | `#B45309` | `#FEF3C7` | 4.51:1 | Status **Dipinjam**, denda |
| Danger | `#B91C1C` | `#FEE2E2` | 5.30:1 | **Terlambat**, Hapus, error validasi |
| Info | `#1E3A8A` | `#EFF6FF` | 9.52:1 | Info netral |

Perubahan makna: **Dipinjam = amber** (kondisi normal, bukan error), **Terlambat = merah**.

### 2.2 Spacing (skala 4pt)

| Token | px | Pemakaian |
|---|---|---|
| `s1` | 4 | Label ↔ input, ikon ↔ teks rapat |
| `s2` | 8 | Gap ikon ↔ label tombol, antar elemen kecil |
| `s3` | 12 | Padding sel tabel (horizontal), antar item sidebar |
| `s4` | 16 | Antar field form, padding card kecil |
| `s5` | 24 | Padding card/dialog/konten utama |
| `s6` | 32 | Antar bagian dalam halaman |
| `s7` | 48 | Padding panel login |

Aturan: jarak di dalam grup < jarak antar grup. Tidak boleh angka ganjil (13, 15, 22).

### 2.3 Tipografi — Segoe UI

Satu family (**Segoe UI**) + ikon (**Segoe Fluent Icons**). Hanya weight Regular (400), Semibold (600), Bold (700).
Ukuran dalam **px** (di WinForms: `GraphicsUnit.Pixel`, atau konversi pt = px × 0.75).

| Peran | px / line | Weight | Contoh |
|---|---|---|---|
| Caption | 12 / 16 | 600 | Label kartu statistik (kapital), header tabel |
| Small | 14 / 20 | 400 | Sel tabel, helper text, placeholder |
| **Body** | **14 / 20** | **400** | Teks umum desktop (kerapatan data) |
| Label | 14 / 20 | 600 | Label form, tombol |
| H3 | 20 / 28 | 600 | Judul card/panel, judul dialog |
| H2 | 24 / 32 | 700 | Sapaan "Selamat pagi, Bu Rina", judul form login |
| H1 | 32 / 40 | 700 | Headline panel kiri login, angka statistik |

Minimum 12px untuk semua teks. Judul/headline tidak boleh dalam kotak yang lebih sempit dari teksnya (hindari overlap): beri `MaximumSize` lebar + `AutoSize = true`.

### 2.4 Radius, border, elevasi

| Token | Nilai | Pemakaian |
|---|---|---|
| `r-sm` | 4 | Badge kecil |
| `r-md` | 8 | **Input, tombol, item sidebar** |
| `r-lg` | 12 | Card, panel |
| `r-xl` | 16 | Dialog/modal |
| `r-pill` | 999 | Badge status, avatar |

Card: **border 1px `neutral-200`, tanpa shadow** (pilih satu). Dialog: shadow lembut dari OS (`FormBorderStyle.FixedDialog`) + border.

### 2.5 Ukuran komponen

| Komponen | Ukuran |
|---|---|
| Input / ComboBox | tinggi **36** (login: **40**), padding-x 12, radius 8 |
| Tombol md | tinggi **36**, padding-x 16 |
| Tombol lg (CTA login) | tinggi **44**, lebar penuh form |
| Tombol sm | tinggi 32, padding-x 12 |
| Ikon | 20px (di tombol sm 16px) |
| Baris tabel | tinggi **44**; header **40** |
| Item sidebar | tinggi **40**, padding-x 12, radius 8 |
| Sidebar | lebar **240** |
| Header halaman | tinggi **72**, padding-x 24 |
| Avatar | 40 × 40, pill |
| Jarak antar target klik | ≥ 8 |

Ukuran jendela minimum: **1024 × 640**. Konten di bawah itu harus scroll, bukan terpotong.

### 2.6 State interaksi (wajib untuk setiap kontrol)

| State | Tombol utama | Input | Baris tabel |
|---|---|---|---|
| Default | bg `primary-700`, teks putih | border `border-strong`, bg putih | bg putih / zebra `neutral-50` |
| Hover | bg `primary-800` | border `neutral-600` | bg `primary-50` |
| Focus | ring 2px `primary-600` offset 2 | border `primary-600` 2px | — |
| Pressed | bg `#172554` | — | — |
| Selected | — | — | bg `primary-100`, teks tetap `neutral-900` |
| Disabled | bg `neutral-200`, teks `neutral-500` | bg `neutral-100` | — |
| Error | — | border `#B91C1C` + pesan 12px merah di bawah, ikon ⚠ | — |

Tombol sekunder: bg `neutral-100`, teks `neutral-900`, border `neutral-200`. Tombol bahaya (Hapus): bg putih, teks & border `#B91C1C`; saat hover bg `#FEE2E2`. Hapus selalu minta konfirmasi dialog.

Keyboard: `Tab` urut logis; `Enter` = tombol utama dialog; `Esc` = Batal; `Ctrl+F` fokus ke pencarian; `Ctrl+N` Tambah Buku; fokus selalu terlihat.

---

## 3. Layout kerangka aplikasi

```
┌──────────────┬──────────────────────────────────────────────┐
│ Sidebar 240  │ Header 72 : Sapaan (H2)      [chip DB] Avatar│
│  Logo        ├──────────────────────────────────────────────┤
│  Menu ×5     │ Konten  (padding 24, latar neutral-50)       │
│              │   ┌ card ─────────────────────────────────┐  │
│  ─────────   │   │ ...                                   │  │
│  Keluar      │   └───────────────────────────────────────┘  │
└──────────────┴──────────────────────────────────────────────┘
```

- Gunakan `TableLayoutPanel` 2 kolom (240 | fill) pada root; konten `Dock = Fill`. **Tanpa koordinat absolut** agar tidak menimpa pada DPI 125–150%.
- `AutoScaleMode = Dpi`, aktifkan `PerMonitorV2` (`Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)`).
- Aktifkan `DoubleBuffered` pada panel/DataGridView untuk mencegah kedip.

### Sidebar
- Latar putih, border kanan 1px `neutral-200`.
- Atas: logo ikon Library 24px `primary-700` + "Perpustakaan" (H3, `primary-700`) + "SMA Pelita Bangsa" (12px, `neutral-500`). Padding 24.
- Menu (jarak 4 antar item): Pencarian Buku, Data Buku, Peminjaman, Anggota, Laporan. Ikon 20px + label 14/600. Aktif: bg `primary-50`, teks & ikon `primary-700`, bar kiri 3px.
- Bawah: tombol **Keluar Akun** — gaya bahaya ringan (teks `#B91C1C`, bg putih, border `#FEE2E2`), tinggi 40, margin 16.

### Header
- Kiri: "Selamat pagi, Bu Rina" (H2) + subjudul 14px `neutral-500`: "Kelola koleksi dan sirkulasi hari ini." (perbaiki spasi ganda). Ikon tangan emoji dihapus atau diganti ikon kecil.
- Kanan: chip status DB (pill success: ● "MySQL terhubung") lalu avatar inisial 40px (`primary-700`, teks putih) + nama (14/600) + peran (12 `neutral-500`).
- Salam menyesuaikan jam (pagi/siang/sore/malam) — opsional.

---

## 4. Spesifikasi per layar

### 4.1 Login
Dua panel sama lebar (50/50), tinggi penuh; minimum jendela 1024×640.

**Panel kiri** (`primary-900`, padding 48, konten dari atas ke bawah dengan `FlowLayoutPanel` vertikal):
1. Logo ikon + "Perpustakaan SMA Pelita Bangsa" (H3, putih)
2. Badge "SISTEM DIGITALISASI PUSTAKA MODERN" (caption 12/600, teks `#BFDBFE`, latar `primary-700`, radius pill, padding 4×12)
3. Headline H1 putih, **satu paragraf** dengan wrap: "Kelola Koleksi Sirkulasi Buku Lebih Cepat" (`MaximumSize.Width = 440`)
4. Deskripsi 14/20 `#CBD5E1`: "Aplikasi manajemen perpustakaan terpadu sekolah. Mendukung katalog otomatis, pelacakan peminjaman, proteksi kuota, …" (lengkapi kalimat; jangan berakhir koma)
5. Card info "AKUN DEMO DATABASE" (latar putih 8% opacity / `#14357A`, border `#2A4A94`, radius 12, padding 16, **tinggi mengikuti isi**): ikon kunci amber `#F59E0B` + judul 12/600; isi 14px "Admin dan Siswa login memakai kredensial MySQL."

**Panel kanan** (putih; form dipusatkan vertikal & horizontal, lebar maks **400**):
1. "Masuk ke Akun Anda" (H2) → jarak 8 → subjudul 14 `neutral-500`
2. Jarak 24 → **segmented control** peran: "Pustakawan | Siswa" (tinggi 40, radius 8; aktif = `primary-700` teks putih, nonaktif = `neutral-100` teks `neutral-900`; ikon 16px)
3. Jarak 24 → label "Nama Pengguna" → input 40 (ikon Contact di kiri, placeholder "Masukkan username")
4. Jarak 16 → label "Kata Sandi" → input 40 (ikon kunci kiri, tombol mata kanan untuk tampilkan/sembunyikan)
5. Jarak 24 → tombol **"Masuk ke Sistem →"** lebar penuh, tinggi 44, `primary-700`
6. Jarak 16 → helper 12px `neutral-500` "Akun dikelola melalui database oleh admin sistem."
- Error login: banner danger di atas tombol (ikon ⚠ + pesan), bukan MessageBox.
- `AcceptButton` = tombol masuk; fokus awal di Username.

### 4.2 Dashboard
Konten (padding 24), urutan vertikal, jarak 24:

**Baris 1 — 4 kartu statistik** (`TableLayoutPanel` 4 kolom sama lebar, gap 16, tinggi 112, card `r-lg`, padding 20):
- Ikon dalam chip 40×40 (`r-md`, latar tint) + label caption kapital `neutral-600`
- Angka H1 (32/40 bold)
- Total Koleksi: ikon Library, chip `primary-50`, angka `primary-700`
- Dipinjam: ikon Sync, chip `#FEF3C7`, angka `neutral-900`
- Terlambat: ikon Warning, chip `#FEE2E2`, angka `#B91C1C`
- Total Denda: ikon Money, chip `#FEF3C7`, angka `neutral-900` ("Rp 21.000", format `id-ID`)

**Baris 2 — dua card** (rasio kolom 2 : 1, gap 24, tinggi mengisi sisa):
- **Sirkulasi Transaksi Terbaru** (kiri): judul H3 + ikon; tabel kolom `Nama Peminjam | Judul Buku | Tgl Pinjam | Status | Denda`. Tanggal diformat `dd MMM yyyy` ("05 Okt 2026"). Status = badge (Dikembalikan=success, Dipinjam=warning, Terlambat=danger). Denda = rata kanan "Rp 3.000" atau "—".
- **Koleksi Terpopuler** (kanan): daftar peringkat 5 buku: nomor bulat 24px `primary-50`, judul (14/600), penulis (12 `neutral-500`), jumlah pinjam rata kanan ("12×"). Data: `GROUP BY` buku pada tabel peminjaman, `ORDER BY COUNT DESC LIMIT 5`. Jika kosong → **empty state**: ikon bintang 32px `neutral-500`, "Belum ada data peminjaman", helper 12px.

### 4.3 Pencarian Buku
- **Toolbar card** (padding 16, `r-lg`): input pencarian (lebar fleksibel, tinggi 36, ikon Search kiri, placeholder "Cari judul, pengarang, topik, atau kode…") + ComboBox kategori (lebar 200). Pencarian live dengan debounce 250ms. `Ctrl+F` fokus ke kolom ini.
- **Dua kolom** (gap 24): tabel hasil (fleksibel) | **panel detail** (lebar 360, card).
- Tabel: `Kode | Judul Buku | Penulis | Kategori | Status` (status = badge). Judul = Fill, kolom lain lebar tetap.
- Panel detail (padding 24): overline "INFORMASI DETAIL BUKU" (caption, ikon buku) → judul H2 (tanpa strip putih) → "Karya: Andrea Hirata (2005)" (14 italic `neutral-500`) → daftar properti (label 14/600 `neutral-600` lebar 96 | nilai 14) → badge status → tombol **"Pinjam Buku Ini"** (primary, lebar penuh, tinggi 40; **disabled** jika status Dipinjam, dengan teks "Sedang dipinjam"). Belum ada pilihan → empty state "Pilih buku untuk melihat detail".

### 4.4 Data Buku
- **Baris ringkasan** (chip): "Total: 9 buku" (`primary-50`), "Tersedia: 8 eks" (success), "Dipinjam: 1 eks" (warning) — tinggi 28, pill, 12/600.
- **Toolbar** (tinggi 36, gap 8): pencarian (fleksibel) · kategori (180) · status (160) · spacer · tombol **"+ Tambah Buku"** (primary, kanan). Semua kontrol seragam tinggi 36.
- **Tabel** (`Dock=Fill`, card `r-lg`): kolom `Kode (96) | Judul Buku (Fill, min 220) | Penulis (200) | Kategori (120) | Tahun (80, rata kanan) | Status (120, badge) | Lokasi Rak (170, tanpa wrap)`. Header bg `primary-700` teks putih 12/600, tinggi 40; baris tinggi 44, zebra `neutral-50`, tanpa garis vertikal, hanya garis horizontal `neutral-200`. Scrollbar tipis; tidak boleh ada baris terpotong tanpa scroll.
- **Aksi baris** (footer card, tinggi 56, padding 12 16, aktif hanya jika ada baris dipilih): `Edit Buku` (sekunder) · `Pinjamkan` (primary-outline, disabled jika Dipinjam) · `Kembalikan` (success-outline, disabled jika Tersedia) · spacer · `Hapus` (bahaya, kanan, konfirmasi). Tombol memakai ikon 16px + label, gap 8.

### 4.5 Dialog Tambah / Edit Buku
- Ukuran **520 × auto** (`AutoSize`, `StartPosition = CenterParent`, `FixedDialog`, tanpa tombol maximize), padding 24, latar putih.
- Judul H3 "Tambah Buku Baru" (+ teks bantu 14 `neutral-500`: "Lengkapi data koleksi. Kolom bertanda * wajib diisi.").
- Form 1 kolom (field 6 buah) atau 2 kolom untuk pasangan pendek (Kategori | Tahun Terbit). Urutan: Kode Buku*, Judul Buku*, Nama Penulis*, Lokasi Rak*, Kategori*, Tahun Terbit.
- Per field: label 14/600 → 4 → input 36 → helper/error 12px (tinggi disediakan 16 agar layout tidak melompat). Placeholder pakai contoh ("BK-010", "Rak B-12, Lantai 1") — bukan di label.
- Validasi saat simpan/blur: wajib terisi; kode unik (BK-###); tahun 4 digit antara 1900 dan tahun ini. Error inline (border merah + pesan), fokus pindah ke field pertama yang salah.
- Footer: garis 1px `neutral-200`, jarak 24, tombol **rata kanan** `Batal` (sekunder) lalu `Simpan Buku` (primary), keduanya tinggi 36, gap 8, **margin kanan 0 dari padding dialog** (tidak menyentuh tepi). `AcceptButton` = Simpan, `CancelButton` = Batal.
- Setelah sukses: tutup dialog, baris baru terpilih & scroll ke sana, toast sukses 3 detik "Buku berhasil disimpan".

### 4.6 Layar lain (Peminjaman, Anggota, Laporan)
Pakai pola Data Buku: ringkasan chip → toolbar 36 → tabel → footer aksi; dialog mengikuti 4.5. Jangan membuat gaya baru.

---

## 5. Komponen: spesifikasi ringkas

**Badge status** — pill tinggi 24, padding-x 10, teks 12/600, titik 6px di kiri. Tersedia (success), Dipinjam (warning), Terlambat (danger), Dikembalikan (success).

**Toast** — kanan-bawah, lebar 320, card `r-lg`, border kiri 4px semantic, durasi 3 dtk, tidak mengambil fokus.

**Empty state** — ikon 32px `neutral-500`, judul 14/600, helper 12 `neutral-500`, dipusatkan di card.

**Dialog konfirmasi** — judul "Hapus buku?", isi "BK-002 · Bumi Manusia akan dihapus permanen.", tombol `Batal` (default/fokus) + `Hapus` (danger fill).

---

## 6. Ikon (Segoe Fluent Icons / fallback Segoe MDL2 Assets)

Set `Font = new Font("Segoe Fluent Icons", 20, GraphicsUnit.Pixel)`; jika font tidak ada, fallback `Segoe MDL2 Assets`. Kode berikut sama di keduanya; **verifikasi tiap glyph di Character Map** sebelum dipakai.

| Fungsi | Glyph | Fungsi | Glyph |
|---|---|---|---|
| Pencarian | `\uE721` | Tambah | `\uE710` |
| Edit | `\uE70F` | Hapus | `\uE74D` |
| Simpan | `\uE74E` | Batal/Tutup | `\uE711` |
| Sukses/Centang | `\uE8FB` | Pengguna | `\uE77B` |
| Anggota | `\uE716` | Perpustakaan | `\uE8F1` |
| Sinkron/Pinjam | `\uE895` | Peringatan | `\uE7BA` |
| Lihat sandi | `\uE7B3` | Kalender | `\uE787` |
| Keluar | `\uE7E8` | Beranda | `\uE80F` |

Ikon dekoratif tidak memakai warna semantic kecuali menandakan makna.

---

## 7. Catatan implementasi (WinForms)

1. **Satu file token**: buat `Theme.cs` (static class) berisi `Colors`, `Spacing`, `Fonts`, `Radius`, `Sizes`. Tidak ada `Color.FromArgb(...)` atau angka ukuran literal di form lain.
2. **Kontrol kustom** (satu kali, dipakai ulang): `RoundedPanel` (card/radius), `RoundedButton` (varian: Primary/Secondary/Danger/Ghost + state hover/pressed/disabled/focus), `RoundedTextBox` (ikon + border + state error), `StatusBadge`, `SidebarButton`, `StatCard`, `EmptyState`, `Toast`.
3. **Rounded corners**: `GraphicsPath` + `Region`/`FillPath` dengan `SmoothingMode.AntiAlias`; atau pakai library (mis. Guna.UI2 / ReaLTaiizor) hanya bila tidak ingin membuat kontrol sendiri — jangan campur gaya dua library.
4. **DataGridView**: `EnableHeadersVisualStyles = false`, `RowHeadersVisible = false`, `CellBorderStyle = SingleHorizontal`, `AllowUserToAddRows = false`, `SelectionMode = FullRowSelect`, `ReadOnly = true`, `AutoSizeColumnsMode = None` (kolom Judul `Fill`), `DefaultCellStyle.WrapMode = False`. Badge status digambar lewat event `CellPainting`.
5. **Tidak ada posisi absolut** untuk teks yang panjangnya dinamis; gunakan `TableLayoutPanel` / `FlowLayoutPanel` / `Dock` / `Anchor`, `AutoSize = true`, `MaximumSize` untuk wrap.
6. **Data**: jangan memuat UI di thread utama untuk query MySQL panjang; gunakan `async/await` + indikator memuat kecil.
7. **Aksesibilitas**: setiap kontrol punya `AccessibleName`, `TabIndex` berurutan, fokus terlihat, tidak mengandalkan warna saja.

---

## 8. Checklist sebelum selesai

- [ ] Tidak ada teks yang saling menimpa pada DPI 100%, 125%, 150%
- [ ] Semua tinggi kontrol sejenis seragam (input/tombol = 36; login = 40/44)
- [ ] Semua warna berasal dari token; teks memenuhi 4.5:1; border input ≥ 3:1
- [ ] Hanya satu tombol primary per area; Hapus selalu dikonfirmasi
- [ ] Setiap kontrol punya state hover, focus, pressed, disabled
- [ ] Tabel tidak terpotong; scroll muncul bila perlu; kolom tidak membungkus
- [ ] Empty state, error validasi, dan loading state ada
- [ ] Dialog: tombol tidak menyentuh tepi; Enter/Esc berfungsi
- [ ] Fungsi dan teks asli tidak berubah, kecuali yang tercantum di bagian 1
