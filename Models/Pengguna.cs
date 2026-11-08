namespace PerpustakaanPelitaBangsa.Models;

/// <summary>
/// Kelas abstrak basis pengguna perpustakaan (Abstraksi).
/// </summary>
public abstract class Pengguna
{
    private string _username = string.Empty;
    protected List<Buku> _daftarBuku;
    protected List<Transaksi> _daftarTransaksi;

    public string Username
    {
        get => _username;
        set => _username = value?.Trim() ?? string.Empty;
    }

    public List<Buku> DaftarBuku => _daftarBuku;
    public List<Transaksi> DaftarTransaksi => _daftarTransaksi;

    protected Pengguna(string username, List<Buku> daftarBuku, List<Transaksi> daftarTransaksi)
    {
        Username = username;
        _daftarBuku = daftarBuku;
        _daftarTransaksi = daftarTransaksi;
    }

    /// <summary>
    /// Kontrak method abstrak untuk menampilkan antarmuka/menu peran (Polimorfisme).
    /// </summary>
    public abstract void TampilMenu();
}

/// <summary>
/// Peran Siswa (Pewarisan dari Pengguna).
/// </summary>
public class Siswa : Pengguna
{
    public Siswa(string username, List<Buku> daftarBuku, List<Transaksi> daftarTransaksi)
        : base(username, daftarBuku, daftarTransaksi)
    {
    }

    public override void TampilMenu()
    {
        // Polimorfisme: Implementasi peran Siswa
    }
}

/// <summary>
/// Peran Pustakawan (Pewarisan dari Pengguna).
/// </summary>
public class Pustakawan : Pengguna
{
    public Pustakawan(string username, List<Buku> daftarBuku, List<Transaksi> daftarTransaksi)
        : base(username, daftarBuku, daftarTransaksi)
    {
    }

    public override void TampilMenu()
    {
        // Polimorfisme: Implementasi peran Pustakawan
    }
}
