USE pbo_perpustakaan;
GO

INSERT INTO dbo.buku (kode_buku, judul, penulis, lokasi_rak, tersedia, kategori, tahun, foto_path)
SELECT v.kode_buku, v.judul, v.penulis, v.lokasi_rak, v.tersedia, v.kategori, v.tahun, v.foto_path
FROM (VALUES
    ('BK-001', 'Laskar Pelangi', 'Andrea Hirata', 'Rak B-12, Lantai 1', CAST(1 AS bit), 'Novel', '2005', 'book-covers/BK-001.jpg'),
    ('BK-002', 'Bumi Manusia', 'Pramoedya Ananta Toer', 'Rak B-14, Lantai 1', CAST(0 AS bit), 'Novel', '1980', 'book-covers/BK-002.jpg'),
    ('BK-003', 'Negeri 5 Menara', 'Ahmad Fuadi', 'Rak B-12, Lantai 1', CAST(1 AS bit), 'Novel', '2009', 'book-covers/BK-003.jpg'),
    ('BK-004', 'Filosofi Teras', 'Henry Manampiring', 'Rak C-03, Lantai 1', CAST(1 AS bit), 'Psikologi', '2018', 'book-covers/BK-004.jpg'),
    ('BK-005', 'Hujan', 'Tere Liye', 'Rak B-15, Lantai 1', CAST(0 AS bit), 'Novel', '2016', 'book-covers/BK-005.jpg'),
    ('BK-006', 'Pulang', 'Tere Liye', 'Rak B-15, Lantai 1', CAST(1 AS bit), 'Novel', '2015', 'book-covers/BK-006.jpg'),
    ('BK-007', 'Algoritma dan Pemrograman Java', 'Rinaldi Munir', 'Rak B-01, Lantai 1', CAST(1 AS bit), 'Pendidikan', '2020', NULL),
    ('BK-008', 'Fisika Dasar SMA Kelas XII', 'Marthen Kanginan', 'Rak C-02, Lantai 1', CAST(1 AS bit), 'Sains', '2019', NULL),
    ('BK-009', 'Sejarah Indonesia Modern', 'M.C. Ricklefs', 'Rak D-01, Lantai 1', CAST(1 AS bit), 'Sejarah', '2008', 'book-covers/BK-009.jpg')
) AS v(kode_buku, judul, penulis, lokasi_rak, tersedia, kategori, tahun, foto_path)
WHERE NOT EXISTS (SELECT 1 FROM dbo.buku b WHERE b.kode_buku = v.kode_buku);

UPDATE b SET foto_path = v.foto_path
FROM dbo.buku b
JOIN (VALUES
    ('BK-001', 'book-covers/BK-001.jpg'),
    ('BK-002', 'book-covers/BK-002.jpg'),
    ('BK-003', 'book-covers/BK-003.jpg'),
    ('BK-004', 'book-covers/BK-004.jpg'),
    ('BK-005', 'book-covers/BK-005.jpg'),
    ('BK-006', 'book-covers/BK-006.jpg'),
    ('BK-009', 'book-covers/BK-009.jpg')
) AS v(kode_buku, foto_path) ON b.kode_buku = v.kode_buku;
GO

INSERT INTO dbo.transaksi (id_transaksi, kode_buku, judul_buku, nama_peminjam, tanggal_pinjam, tanggal_kembali, status_peminjaman)
SELECT v.id_transaksi, v.kode_buku, v.judul_buku, v.nama_peminjam, v.tanggal_pinjam, v.tanggal_kembali, v.status_peminjaman
FROM (VALUES
    ('TRX-001', 'BK-002', 'Bumi Manusia', 'Dimas Saputra', '2026-10-12', '-', 'Dipinjam'),
    ('TRX-002', 'BK-005', 'Hujan', 'Rizky Pratama', '2026-10-21', '-', 'Dipinjam'),
    ('TRX-003', 'BK-001', 'Laskar Pelangi', 'Nadia Putri', '2026-10-10', '2026-10-17', 'Dikembalikan'),
    ('TRX-004', 'BK-003', 'Negeri 5 Menara', 'Ahmad Fadlan', '2026-10-11', '2026-10-16', 'Dikembalikan'),
    ('TRX-005', 'BK-006', 'Pulang', 'Siti Nurhaliza', '2026-10-05', '2026-10-15', 'Dikembalikan (Denda: Rp 3.000)')
) AS v(id_transaksi, kode_buku, judul_buku, nama_peminjam, tanggal_pinjam, tanggal_kembali, status_peminjaman)
WHERE NOT EXISTS (SELECT 1 FROM dbo.transaksi t WHERE t.id_transaksi = v.id_transaksi);
GO

SELECT COUNT(*) AS total_buku FROM dbo.buku;
SELECT COUNT(*) AS total_transaksi FROM dbo.transaksi;
GO
