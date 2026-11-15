-- Database schema: SMA Pelita Bangsa library
-- For local XAMPP/MariaDB development. Run this script in phpMyAdmin or mysql CLI.
CREATE DATABASE IF NOT EXISTS pbo_perpustakaan
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE pbo_perpustakaan;

CREATE TABLE IF NOT EXISTS buku (
    kode_buku VARCHAR(20) PRIMARY KEY,
    judul VARCHAR(255) NOT NULL,
    penulis VARCHAR(150) NOT NULL,
    lokasi_rak VARCHAR(100) NOT NULL,
    tersedia TINYINT(1) NOT NULL DEFAULT 1,
    kategori VARCHAR(50) DEFAULT 'Umum',
    tahun VARCHAR(10) DEFAULT '',
    foto_path VARCHAR(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS transaksi (
    id_transaksi VARCHAR(20) PRIMARY KEY,
    kode_buku VARCHAR(20) NOT NULL,
    judul_buku VARCHAR(255) NOT NULL,
    nama_peminjam VARCHAR(150) NOT NULL,
    tanggal_pinjam VARCHAR(20) NOT NULL,
    tanggal_kembali VARCHAR(20) DEFAULT '-',
    status_peminjaman VARCHAR(100) NOT NULL,
    INDEX idx_transaksi_kode_buku (kode_buku),
    CONSTRAINT fk_transaksi_buku FOREIGN KEY (kode_buku)
        REFERENCES buku(kode_buku) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

CREATE TABLE IF NOT EXISTS pengguna (
    username VARCHAR(64) PRIMARY KEY,
    password_hash VARCHAR(256) NOT NULL,
    salt VARCHAR(64) NOT NULL,
    role ENUM('admin','siswa') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- First application startup seeds admin/admin and siswa/siswa into an empty
-- pengguna table using PBKDF2-SHA256 hashes and per-account random salts.
-- Never insert or commit plaintext passwords into this SQL file.
