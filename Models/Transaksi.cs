namespace PerpustakaanPelitaBangsa.Models;

/// <summary>
/// Model data transaksi sirkulasi peminjaman. Menerapkan Enkapsulasi.
/// </summary>
public class Transaksi
{
    private string _idTransaksi = string.Empty;
    private string _kodeBuku = string.Empty;
    private string _judulBuku = string.Empty;
    private string _namaPeminjam = string.Empty;
    private string _tanggalPinjam = string.Empty;
    private string _tanggalKembali = "-";
    private string _statusPeminjaman = "Dipinjam";

    public string IdTransaksi
    {
        get => _idTransaksi;
        set => _idTransaksi = value?.Trim() ?? string.Empty;
    }

    public string KodeBuku
    {
        get => _kodeBuku;
        set => _kodeBuku = value?.Trim() ?? string.Empty;
    }

    public string JudulBuku
    {
        get => _judulBuku;
        set => _judulBuku = value?.Trim() ?? string.Empty;
    }

    public string NamaPeminjam
    {
        get => _namaPeminjam;
        set => _namaPeminjam = value?.Trim() ?? string.Empty;
    }

    public string TanggalPinjam
    {
        get => _tanggalPinjam;
        set => _tanggalPinjam = value?.Trim() ?? string.Empty;
    }

    public string TanggalKembali
    {
        get => _tanggalKembali;
        set => _tanggalKembali = value?.Trim() ?? "-";
    }

    public string StatusPeminjaman
    {
        get => _statusPeminjaman;
        set => _statusPeminjaman = value?.Trim() ?? "Dipinjam";
    }

    public Transaksi(string idTransaksi, string kodeBuku, string judulBuku, string namaPeminjam,
                     string tanggalPinjam, string tanggalKembali, string statusPeminjaman)
    {
        IdTransaksi = idTransaksi;
        KodeBuku = kodeBuku;
        JudulBuku = judulBuku;
        NamaPeminjam = namaPeminjam;
        TanggalPinjam = tanggalPinjam;
        TanggalKembali = tanggalKembali;
        StatusPeminjaman = statusPeminjaman;
    }

    public string ToFileFormat()
    {
        return $"{IdTransaksi}|{KodeBuku}|{JudulBuku}|{NamaPeminjam}|{TanggalPinjam}|{TanggalKembali}|{StatusPeminjaman}";
    }

    public static Transaksi? FromFileFormat(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;
        string[] parts = line.Split('|');
        if (parts.Length < 7) return null;

        return new Transaksi(
            parts[0].Trim(),
            parts[1].Trim(),
            parts[2].Trim(),
            parts[3].Trim(),
            parts[4].Trim(),
            parts[5].Trim(),
            parts[6].Trim()
        );
    }
}
