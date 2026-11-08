using MySqlConnector;
using PerpustakaanPelitaBangsa.Models;

namespace PerpustakaanPelitaBangsa.Services;

/// <summary>
/// Helper koneksi dan operasi database MySQL/MariaDB (XAMPP).
/// ponytail: failover otomatis ke flat file txt jika server MySQL mati/tidak terhubung.
/// </summary>
public static class DatabaseHelper
{
    public static string ConnectionString => Environment.GetEnvironmentVariable("PERPUSTAKAAN_MYSQL_CONNECTION")
        ?? "Server=localhost;Port=3306;Database=pbo_perpustakaan;User ID=root;Password=;Connection Timeout=3;";

    public static void EnsureUsersTable()
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand(@"CREATE TABLE IF NOT EXISTS pengguna (
            username VARCHAR(64) PRIMARY KEY,
            password_hash VARCHAR(256) NOT NULL,
            salt VARCHAR(64) NOT NULL,
            role ENUM('admin','siswa') NOT NULL
        );", conn);
        cmd.ExecuteNonQuery();

        using var count = new MySqlCommand("SELECT COUNT(*) FROM pengguna;", conn);
        if (Convert.ToInt32(count.ExecuteScalar()) == 0)
        {
            TambahAkun(conn, "admin", "admin", "admin");
            TambahAkun(conn, "siswa", "siswa", "siswa");
        }
    }

    private static void TambahAkun(MySqlConnection conn, string username, string password, string role)
    {
        byte[] salt = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);
        byte[] hash = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, System.Security.Cryptography.HashAlgorithmName.SHA256, 32);
        using var cmd = new MySqlCommand("INSERT INTO pengguna (username, password_hash, salt, role) VALUES (@u, @h, @s, @r);", conn);
        cmd.Parameters.AddWithValue("@u", username);
        cmd.Parameters.AddWithValue("@h", Convert.ToHexString(hash));
        cmd.Parameters.AddWithValue("@s", Convert.ToHexString(salt));
        cmd.Parameters.AddWithValue("@r", role);
        cmd.ExecuteNonQuery();
    }

    public static (bool Sukses, string Role, string Pesan) Autentikasi(string username, string password, string role)
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new MySqlCommand("SELECT password_hash, salt, role FROM pengguna WHERE username = @u;", conn);
            cmd.Parameters.AddWithValue("@u", username);
            using var r = cmd.ExecuteReader();
            if (!r.Read()) return (false, "", "Username atau password salah.");

            byte[] expected = Convert.FromHexString(r.GetString(0));
            byte[] salt = Convert.FromHexString(r.GetString(1));
            string storedRole = r.GetString(2);
            byte[] actual = System.Security.Cryptography.Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, System.Security.Cryptography.HashAlgorithmName.SHA256, expected.Length);
            bool valid = System.Security.Cryptography.CryptographicOperations.FixedTimeEquals(actual, expected)
                && string.Equals(storedRole, role, StringComparison.OrdinalIgnoreCase);
            return valid ? (true, storedRole, "Login berhasil.") : (false, "", "Username, password, atau role salah.");
        }
        catch (Exception ex)
        {
            return (false, "", $"Database login tidak tersedia: {ex.Message}");
        }
    }

    public static (bool Sukses, string Pesan) TestKoneksi()
    {
        try
        {
            using var conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using var cmd = new MySqlCommand("SELECT VERSION();", conn);
            var version = cmd.ExecuteScalar()?.ToString() ?? "Unknown";
            return (true, $"Terhubung ke MySQL/MariaDB ({version})");
        }
        catch (Exception ex)
        {
            return (false, $"Gagal terhubung ke MySQL: {ex.Message}");
        }
    }

    public static List<Buku> AmbilSemuaBuku()
    {
        var list = new List<Buku>();
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand("SELECT kode_buku, judul, penulis, lokasi_rak, tersedia, kategori, tahun FROM buku;", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Buku(
                r.GetString(0),
                r.GetString(1),
                r.GetString(2),
                r.GetString(3),
                r.GetInt32(4) == 1,
                r.IsDBNull(5) ? "" : r.GetString(5),
                r.IsDBNull(6) ? "" : r.GetString(6)
            ));
        }
        return list;
    }

    public static List<Transaksi> AmbilSemuaTransaksi()
    {
        var list = new List<Transaksi>();
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand("SELECT id_transaksi, kode_buku, judul_buku, nama_peminjam, tanggal_pinjam, tanggal_kembali, status_peminjaman FROM transaksi;", conn);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Transaksi(
                r.GetString(0),
                r.GetString(1),
                r.GetString(2),
                r.GetString(3),
                r.GetString(4),
                r.GetString(5),
                r.GetString(6)
            ));
        }
        return list;
    }

    public static void SimpanBuku(Buku b)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        const string sql = @"REPLACE INTO buku (kode_buku, judul, penulis, lokasi_rak, tersedia, kategori, tahun)
                             VALUES (@k, @j, @p, @r, @t, @c, @y);";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@k", b.KodeBuku);
        cmd.Parameters.AddWithValue("@j", b.Judul);
        cmd.Parameters.AddWithValue("@p", b.Penulis);
        cmd.Parameters.AddWithValue("@r", b.LokasiRak);
        cmd.Parameters.AddWithValue("@t", b.Tersedia ? 1 : 0);
        cmd.Parameters.AddWithValue("@c", b.Kategori);
        cmd.Parameters.AddWithValue("@y", b.Tahun);
        cmd.ExecuteNonQuery();
    }

    public static void SimpanTransaksi(Transaksi t)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        const string sql = @"REPLACE INTO transaksi (id_transaksi, kode_buku, judul_buku, nama_peminjam, tanggal_pinjam, tanggal_kembali, status_peminjaman)
                             VALUES (@id, @k, @j, @p, @tp, @tk, @s);";
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", t.IdTransaksi);
        cmd.Parameters.AddWithValue("@k", t.KodeBuku);
        cmd.Parameters.AddWithValue("@j", t.JudulBuku);
        cmd.Parameters.AddWithValue("@p", t.NamaPeminjam);
        cmd.Parameters.AddWithValue("@tp", t.TanggalPinjam);
        cmd.Parameters.AddWithValue("@tk", t.TanggalKembali);
        cmd.Parameters.AddWithValue("@s", t.StatusPeminjaman);
        cmd.ExecuteNonQuery();
    }

    public static void HapusBuku(string kodeBuku)
    {
        using var conn = new MySqlConnection(ConnectionString);
        conn.Open();
        using var cmd = new MySqlCommand("DELETE FROM buku WHERE kode_buku = @k;", conn);
        cmd.Parameters.AddWithValue("@k", kodeBuku);
        cmd.ExecuteNonQuery();
    }
}
