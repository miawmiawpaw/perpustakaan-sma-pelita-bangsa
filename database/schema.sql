-- SQL Server schema: SMA Pelita Bangsa library
IF DB_ID(N'pbo_perpustakaan') IS NULL
    CREATE DATABASE pbo_perpustakaan;
GO

USE pbo_perpustakaan;
GO

IF OBJECT_ID(N'dbo.buku', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.buku (
        kode_buku VARCHAR(20) NOT NULL PRIMARY KEY,
        judul VARCHAR(255) NOT NULL,
        penulis VARCHAR(150) NOT NULL,
        lokasi_rak VARCHAR(100) NOT NULL,
        tersedia BIT NOT NULL DEFAULT 1,
        kategori VARCHAR(50) NULL DEFAULT 'Umum',
        tahun VARCHAR(10) NULL DEFAULT '',
        foto_path VARCHAR(255) NULL
    );
END;
GO

IF OBJECT_ID(N'dbo.transaksi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.transaksi (
        id_transaksi VARCHAR(20) NOT NULL PRIMARY KEY,
        kode_buku VARCHAR(20) NOT NULL,
        judul_buku VARCHAR(255) NOT NULL,
        nama_peminjam VARCHAR(150) NOT NULL,
        tanggal_pinjam VARCHAR(20) NOT NULL,
        tanggal_kembali VARCHAR(20) NULL DEFAULT '-',
        status_peminjaman VARCHAR(100) NOT NULL,
        CONSTRAINT fk_transaksi_buku FOREIGN KEY (kode_buku)
            REFERENCES dbo.buku(kode_buku) ON DELETE CASCADE
    );
    CREATE INDEX idx_transaksi_kode_buku ON dbo.transaksi(kode_buku);
END;
GO

IF OBJECT_ID(N'dbo.pengguna', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.pengguna (
        username VARCHAR(64) NOT NULL PRIMARY KEY,
        password_hash VARCHAR(256) NOT NULL,
        salt VARCHAR(64) NOT NULL,
        role VARCHAR(10) NOT NULL,
        CONSTRAINT ck_pengguna_role CHECK (role IN ('admin', 'siswa'))
    );
END;
GO
