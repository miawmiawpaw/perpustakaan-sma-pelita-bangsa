namespace PerpustakaanPelitaBangsa.Models;

/// <summary>
/// Model data buku perpustakaan. Menerapkan Enkapsulasi.
/// </summary>
public class Buku
{
    private string _kodeBuku = string.Empty;
    private string _judul = string.Empty;
    private string _penulis = string.Empty;
    private string _lokasiRak = string.Empty;
    private bool _tersedia;
    private string _kategori = string.Empty;
    private string _tahun = string.Empty;
    private string _fotoPath = string.Empty;

    public string KodeBuku
    {
        get => _kodeBuku;
        set => _kodeBuku = value?.Trim() ?? string.Empty;
    }

    public string Judul
    {
        get => _judul;
        set => _judul = value?.Trim() ?? string.Empty;
    }

    public string Penulis
    {
        get => _penulis;
        set => _penulis = value?.Trim() ?? string.Empty;
    }

    public string LokasiRak
    {
        get => _lokasiRak;
        set => _lokasiRak = value?.Trim() ?? string.Empty;
    }

    public bool Tersedia
    {
        get => _tersedia;
        set => _tersedia = value;
    }

    public string Kategori
    {
        get => string.IsNullOrEmpty(_kategori) ? InferKategori(_judul) : _kategori;
        set => _kategori = value?.Trim() ?? string.Empty;
    }

    public string Tahun
    {
        get => string.IsNullOrEmpty(_tahun) ? InferTahun(_kodeBuku) : _tahun;
        set => _tahun = value?.Trim() ?? string.Empty;
    }
    public string FotoPath
    {
        get => _fotoPath;
        set => _fotoPath = value?.Trim() ?? string.Empty;
    }

    public Buku(string kodeBuku, string judul, string penulis, string lokasiRak, bool tersedia)
        : this(kodeBuku, judul, penulis, lokasiRak, tersedia, InferKategori(judul), InferTahun(kodeBuku), "")
    {
    }

    public Buku(string kodeBuku, string judul, string penulis, string lokasiRak, bool tersedia, string? kategori, string? tahun, string? fotoPath = null)
    {
        KodeBuku = kodeBuku;
        Judul = judul;
        Penulis = penulis;
        LokasiRak = lokasiRak;
        Tersedia = tersedia;
        Kategori = string.IsNullOrWhiteSpace(kategori) ? InferKategori(judul) : kategori;
        Tahun = string.IsNullOrWhiteSpace(tahun) ? InferTahun(kodeBuku) : tahun;
        FotoPath = fotoPath ?? string.Empty;
    }

    public static string InferKategori(string judul)
    {
        if (string.IsNullOrWhiteSpace(judul)) return "Umum";
        string s = judul.ToLowerInvariant();
        if (s.Contains("java") || s.Contains("algoritma") || s.Contains("dasar") || s.Contains("pemrograman"))
            return "Pendidikan";
        if (s.Contains("fisika") || s.Contains("matematika") || s.Contains("kimia") || s.Contains("sains"))
            return "Sains";
        if (s.Contains("sejarah"))
            return "Sejarah";
        if (s.Contains("filosofi") || s.Contains("psikologi"))
            return "Psikologi";
        return "Novel";
    }

    public static string InferTahun(string kode)
    {
        return kode?.ToUpperInvariant() switch
        {
            "BK-001" => "2005",
            "BK-002" => "1980",
            "BK-003" => "2009",
            "BK-004" => "2018",
            "BK-005" => "2016",
            "BK-006" => "2015",
            "BK-007" => "2020",
            "BK-008" => "2019",
            "BK-009" => "2008",
            _ => "2021"
        };
    }

    public string ToFileFormat()
    {
        string statusStr = Tersedia ? "Tersedia" : "Dipinjam";
        return string.Join("|", KodeBuku, Judul, Penulis, LokasiRak, statusStr, Kategori, Tahun, FotoPath);
    }

    public static Buku? FromFileFormat(string line)
    {
        if (string.IsNullOrWhiteSpace(line)) return null;
        string[] parts = line.Split('|');
        if (parts.Length < 5) return null;
        bool tersedia = parts[4].Trim().Equals("Tersedia", StringComparison.OrdinalIgnoreCase);
        string kat = parts.Length >= 6 ? parts[5].Trim() : InferKategori(parts[1].Trim());
        string thn = parts.Length >= 7 ? parts[6].Trim() : InferTahun(parts[0].Trim());
        string foto = parts.Length >= 8 ? parts[7].Trim() : string.Empty;
        return new Buku(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), parts[3].Trim(), tersedia, kat, thn, foto);
    }
}
