using System.Globalization;
using PerpustakaanPelitaBangsa.Models;

namespace PerpustakaanPelitaBangsa.Services;

/// <summary>
/// Service pengelola data persistensi flat-file buku.txt dan transaksi.txt serta logika bisnis perpustakaan.
/// </summary>
public class PerpustakaanService
{
    public const string FileBukuName = "buku.txt";
    public const string FileTransaksiName = "transaksi.txt";
    public const int BatasHariPinjam = 7;
    public const int DendaPerHari = 1000;
    public const int KuotaPinjamMaksimal = 3;

    public List<Buku> DaftarBuku { get; private set; } = new();
    public List<Transaksi> DaftarTransaksi { get; private set; } = new();
    public bool DatabaseAktif { get; private set; }
    public string StatusKoneksiDatabase { get; private set; } = string.Empty;

    private readonly string _fileBukuPath;
    private readonly string _fileTransaksiPath;
    public PerpustakaanService()
    {
        _fileBukuPath = ResolveFilePath(FileBukuName);
        _fileTransaksiPath = ResolveFilePath(FileTransaksiName);
        InisialisasiDataAwalJikaPerlu();
        MuatData();
    }

    private static string ResolveFilePath(string fileName)
    {
        // 1. Direct in working dir
        if (File.Exists(fileName)) return Path.GetFullPath(fileName);

        // 2. In base directory of application
        string inAppDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        if (File.Exists(inAppDir)) return inAppDir;

        // 3. In JAVA folder
        string inJava = Path.Combine("JAVA", fileName);
        if (File.Exists(inJava)) return Path.GetFullPath(inJava);

        // 4. Default to current directory
        return Path.GetFullPath(fileName);
    }

    public void InisialisasiDataAwalJikaPerlu()
    {
        if (!File.Exists(_fileBukuPath))
        {
            var defaultBuku = new List<Buku>
            {
                new("BK-001", "Laskar Pelangi", "Andrea Hirata", "Rak B-12, Lantai 1", true, "Novel", "2005"),
                new("BK-002", "Bumi Manusia", "Pramoedya Ananta Toer", "Rak B-14, Lantai 1", false, "Novel", "1980"),
                new("BK-003", "Negeri 5 Menara", "Ahmad Fuadi", "Rak B-12, Lantai 1", true, "Novel", "2009"),
                new("BK-004", "Filosofi Teras", "Henry Manampiring", "Rak C-03, Lantai 1", true, "Psikologi", "2018"),
                new("BK-005", "Hujan", "Tere Liye", "Rak B-15, Lantai 1", false, "Novel", "2016"),
                new("BK-006", "Pulang", "Tere Liye", "Rak B-15, Lantai 1", true, "Novel", "2015"),
                new("BK-007", "Algoritma dan Pemrograman Java", "Rinaldi Munir", "Rak B-01, Lantai 1", true, "Pendidikan", "2020"),
                new("BK-008", "Fisika Dasar SMA Kelas XII", "Marthen Kanginan", "Rak C-02, Lantai 1", true, "Sains", "2019"),
                new("BK-009", "Sejarah Indonesia Modern", "M.C. Ricklefs", "Rak D-01, Lantai 1", true, "Sejarah", "2008")
            };
            SimpanDaftarBuku(defaultBuku, _fileBukuPath);
        }

        if (!File.Exists(_fileTransaksiPath))
        {
            var defaultTrx = new List<Transaksi>
            {
                new("TRX-001", "BK-002", "Bumi Manusia", "Dimas Saputra", "2026-10-12", "-", "Dipinjam"),
                new("TRX-002", "BK-005", "Hujan", "Rizky Pratama", "2026-10-21", "-", "Dipinjam"),
                new("TRX-003", "BK-001", "Laskar Pelangi", "Nadia Putri", "2026-10-10", "2026-10-17", "Dikembalikan"),
                new("TRX-004", "BK-003", "Negeri 5 Menara", "Ahmad Fadlan", "2026-10-11", "2026-10-16", "Dikembalikan"),
                new("TRX-005", "BK-006", "Pulang", "Siti Nurhaliza", "2026-10-05", "2026-10-15", "Dikembalikan (Denda: Rp 3.000)")
            };
            SimpanDaftarTransaksi(defaultTrx, _fileTransaksiPath);
        }
    }

    public void MuatData()
    {
        // Ensure login accounts exist before connection-status capture.
        try { DatabaseHelper.EnsureUsersTable(); }
        catch { /* Login will fail closed if DB is unavailable. */ }
        var (suksesDb, pesanDb) = DatabaseHelper.TestKoneksi();
        DatabaseAktif = suksesDb;
        StatusKoneksiDatabase = pesanDb;

        if (DatabaseAktif)
        {
            try
            {
                DaftarBuku = DatabaseHelper.AmbilSemuaBuku();
                DaftarTransaksi = DatabaseHelper.AmbilSemuaTransaksi();
                // Sinkronkan ke flat file txt sebagai backup
                SimpanSemuaKeFile();
                return;
            }
            catch
            {
                DatabaseAktif = false;
            }
        }

        // Fallback: muat dari flat-file txt jika MySQL mati/gagal
        DaftarBuku.Clear();
        if (File.Exists(_fileBukuPath))
        {
            foreach (string line in File.ReadAllLines(_fileBukuPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var b = Buku.FromFileFormat(line);
                if (b != null) DaftarBuku.Add(b);
            }
        }

        DaftarTransaksi.Clear();
        if (File.Exists(_fileTransaksiPath))
        {
            foreach (string line in File.ReadAllLines(_fileTransaksiPath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var t = Transaksi.FromFileFormat(line);
                if (t != null) DaftarTransaksi.Add(t);
            }
        }
    }

    public void SimpanSemua()
    {
        SimpanSemuaKeFile();
        if (DatabaseAktif)
        {
            try
            {
                foreach (var b in DaftarBuku) DatabaseHelper.SimpanBuku(b);
                foreach (var t in DaftarTransaksi) DatabaseHelper.SimpanTransaksi(t);
            }
            catch
            {
                DatabaseAktif = false;
            }
        }
    }

    private void SimpanSemuaKeFile()
    {
        SimpanDaftarBuku(DaftarBuku, _fileBukuPath);
        SimpanDaftarTransaksi(DaftarTransaksi, _fileTransaksiPath);
    }

    private static void SimpanDaftarBuku(List<Buku> list, string path)
    {
        var lines = list.Select(b => b.ToFileFormat());
        File.WriteAllLines(path, lines);
    }

    private static void SimpanDaftarTransaksi(List<Transaksi> list, string path)
    {
        var lines = list.Select(t => t.ToFileFormat());
        File.WriteAllLines(path, lines);
    }

    public (bool Sukses, string Pesan) TambahBuku(Buku baru)
    {
        if (string.IsNullOrWhiteSpace(baru.KodeBuku))
            return (false, "Kode buku tidak boleh kosong.");
        if (string.IsNullOrWhiteSpace(baru.Judul))
            return (false, "Judul buku tidak boleh kosong.");

        if (DaftarBuku.Any(b => b.KodeBuku.Equals(baru.KodeBuku, StringComparison.OrdinalIgnoreCase)))
            return (false, $"Buku dengan kode '{baru.KodeBuku}' sudah terdaftar.");

        DaftarBuku.Add(baru);
        SimpanSemua();
        return (true, $"Buku '{baru.Judul}' berhasil ditambahkan.");
    }

    public (bool Sukses, string Pesan) UbahBuku(string kodeBuku, string judul, string penulis, string rak, string kategori, string tahun, string fotoPath = "")
    {
        var buku = DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kodeBuku, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return (false, "Buku tidak ditemukan.");
        buku.Judul = judul;
        buku.Penulis = penulis;
        buku.LokasiRak = rak;
        buku.Kategori = kategori;
        buku.Tahun = tahun;
        buku.FotoPath = fotoPath;
        SimpanSemua();
        return (true, "Data buku berhasil diperbarui.");
    }

    public (bool Sukses, string Pesan) HapusBukuAman(string kodeBuku)
    {
        var buku = DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kodeBuku, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return (false, "Buku tidak ditemukan.");

        // Validasi proteksi bisnis: Buku yang sedang dipinjam dilarang dihapus
        if (!buku.Tersedia)
            return (false, $"Buku '{buku.Judul}' sedang dipinjam. Kembalikan buku terlebih dahulu sebelum dihapus!");

        DaftarBuku.Remove(buku);
        SimpanSemua();
        return (true, $"Buku '{buku.Judul}' berhasil dihapus.");
    }

    public (bool Sukses, string Pesan) PinjamBuku(string kodeBuku, string namaPeminjam)
    {
        if (string.IsNullOrWhiteSpace(namaPeminjam))
            return (false, "Nama peminjam wajib diisi.");

        var buku = DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kodeBuku, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return (false, "Buku tidak ditemukan.");
        if (!buku.Tersedia) return (false, $"Buku '{buku.Judul}' sedang dipinjam siswa lain.");

        // Validasi kuota pinjam siswa (maks 3 buku aktif)
        int pinjamanAktif = HitungPinjamanAktifSiswa(namaPeminjam);
        if (pinjamanAktif >= KuotaPinjamMaksimal)
            return (false, $"Siswa '{namaPeminjam}' telah mencapai batas maksimal kuota ({KuotaPinjamMaksimal} buku aktif).");

        // Validasi duplikasi peminjaman buku yang sama
        bool sedangPinjamBukuSama = DaftarTransaksi.Any(t =>
            t.NamaPeminjam.Equals(namaPeminjam, StringComparison.OrdinalIgnoreCase) &&
            t.KodeBuku.Equals(kodeBuku, StringComparison.OrdinalIgnoreCase) &&
            t.StatusPeminjaman.StartsWith("Dipinjam", StringComparison.OrdinalIgnoreCase));

        if (sedangPinjamBukuSama)
            return (false, $"Siswa '{namaPeminjam}' sudah meminjam buku ini dan belum mengembalikannya.");

        // Buat ID transaksi baru berurutan
        string idTrx = $"TRX-{DaftarTransaksi.Count + 1:D3}";
        string tanggalPinjam = DateTime.Now.ToString("yyyy-MM-dd");

        buku.Tersedia = false;
        var trx = new Transaksi(idTrx, buku.KodeBuku, buku.Judul, namaPeminjam, tanggalPinjam, "-", "Dipinjam");
        DaftarTransaksi.Add(trx);

        SimpanSemua();
        return (true, $"Buku '{buku.Judul}' berhasil dipinjam atas nama '{namaPeminjam}'. ID: {idTrx}");
    }

    public (bool Sukses, string Pesan, long Denda) KembalikanBuku(string kodeBuku)
    {
        var buku = DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kodeBuku, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return (false, "Buku tidak ditemukan.", 0);
        if (buku.Tersedia) return (false, $"Buku '{buku.Judul}' sudah berstatus Tersedia di rak.", 0);

        var trx = DaftarTransaksi.LastOrDefault(t =>
            t.KodeBuku.Equals(kodeBuku, StringComparison.OrdinalIgnoreCase) &&
            t.StatusPeminjaman.StartsWith("Dipinjam", StringComparison.OrdinalIgnoreCase));

        if (trx == null) return (false, "Catatan transaksi peminjaman aktif tidak ditemukan.", 0);

        DateTime today = DateTime.Today;
        string tglKembali = today.ToString("yyyy-MM-dd");
        trx.TanggalKembali = tglKembali;

        // Hitung denda keterlambatan
        long denda = 0;
        if (DateTime.TryParse(trx.TanggalPinjam, out DateTime tglPinjam))
        {
            DateTime tenggat = tglPinjam.AddDays(BatasHariPinjam);
            if (today > tenggat)
            {
                int hariTelat = (int)(today - tenggat).TotalDays;
                denda = (long)hariTelat * DendaPerHari;
            }
        }

        if (denda > 0)
        {
            trx.StatusPeminjaman = $"Dikembalikan (Denda: Rp {denda:N0})";
        }
        else
        {
            trx.StatusPeminjaman = "Dikembalikan";
        }

        buku.Tersedia = true;
        SimpanSemua();

        string pesan = denda > 0
            ? $"Buku '{buku.Judul}' berhasil dikembalikan. Terlambat dikenakan denda Rp {denda:N0}."
            : $"Buku '{buku.Judul}' berhasil dikembalikan tepat waktu tanpa denda.";

        return (true, pesan, denda);
    }

    public int HitungPinjamanAktifSiswa(string namaSiswa)
    {
        return DaftarTransaksi.Count(t =>
            t.NamaPeminjam.Equals(namaSiswa, StringComparison.OrdinalIgnoreCase) &&
            t.StatusPeminjaman.StartsWith("Dipinjam", StringComparison.OrdinalIgnoreCase));
    }

    public (int TotalBuku, int Tersedia, int Dipinjam, int Terlambat, long TotalDenda) AmbilStatistik()
    {
        int total = DaftarBuku.Count;
        int dipinjam = DaftarBuku.Count(b => !b.Tersedia);
        int tersedia = total - dipinjam;
        int terlambat = 0;
        long totalDenda = 0;

        DateTime today = DateTime.Today;
        foreach (var t in DaftarTransaksi)
        {
            if (t.StatusPeminjaman.StartsWith("Dipinjam", StringComparison.OrdinalIgnoreCase))
            {
                if (DateTime.TryParse(t.TanggalPinjam, out DateTime tp))
                {
                    if (today > tp.AddDays(BatasHariPinjam))
                        terlambat++;
                }
            }
            else if (t.StatusPeminjaman.Contains("Denda: Rp"))
            {
                try
                {
                    int idx = t.StatusPeminjaman.IndexOf("Rp");
                    string num = t.StatusPeminjaman.Substring(idx + 2).Replace(".", "").Replace(")", "").Trim();
                    if (long.TryParse(num, NumberStyles.Any, CultureInfo.InvariantCulture, out long d))
                        totalDenda += d;
                }
                catch { /* ignore */ }
            }
        }

        return (total, tersedia, dipinjam, terlambat, totalDenda);
    }
}
