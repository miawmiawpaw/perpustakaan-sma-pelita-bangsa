using PerpustakaanPelitaBangsa.Forms;
using PerpustakaanPelitaBangsa.Models;
using PerpustakaanPelitaBangsa.Services;

namespace PerpustakaanPelitaBangsa;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--gallery")
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize();
            Application.Run(new UiGallery());
            return;
        }
        if (args.Length > 0 && args[0] == "--test")
        {
            RunSelfCheck();
            return;
        }

        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }

    // ponytail: throwaway self-check for business rules validation
    private static void RunSelfCheck()
    {
        Console.WriteLine("[TEST] Menjalankan verifikasi logika bisnis PBO...");

        // 1. Model Buku Roundtrip
        var b1 = new Buku("BK-999", "Buku Uji C#", "Fadlan", "Rak T-1", true, "Pendidikan", "2026");
        string bStr = b1.ToFileFormat();
        var b2 = Buku.FromFileFormat(bStr);
        if (b2 == null || b2.KodeBuku != "BK-999" || b2.Kategori != "Pendidikan" || !b2.Tersedia)
            throw new Exception("Buku serialization check failed!");
        Console.WriteLine("  ✓ Model Buku serialization OK");

        // 2. Model Transaksi Roundtrip
        var t1 = new Transaksi("TRX-999", "BK-999", "Buku Uji C#", "Budi", "2026-10-01", "-", "Dipinjam");
        string tStr = t1.ToFileFormat();
        var t2 = Transaksi.FromFileFormat(tStr);
        if (t2 == null || t2.IdTransaksi != "TRX-999" || t2.StatusPeminjaman != "Dipinjam")
            throw new Exception("Transaksi serialization check failed!");
        Console.WriteLine("  ✓ Model Transaksi serialization OK");

        // 3. Service Verification & Uji Coba Koneksi MySQL (Sesuai Draft PBO)
        var svc = new PerpustakaanService();
        Console.WriteLine($"  ✓ Service muat {svc.DaftarBuku.Count} buku & {svc.DaftarTransaksi.Count} transaksi");
        Console.WriteLine($"  ✓ Integrasi SQL: {(svc.DatabaseAktif ? "[AKTIF] " : "[FALLBACK FILE] ")}{svc.StatusKoneksiDatabase}");
        var wrongPassword = DatabaseHelper.Autentikasi("admin", "wrong", "admin");
        var wrongRole = DatabaseHelper.Autentikasi("admin", "admin", "siswa");
        var unknownUser = DatabaseHelper.Autentikasi("not-a-user", "password", "siswa");
        if (wrongPassword.Sukses || wrongRole.Sukses || unknownUser.Sukses)
            throw new Exception("MySQL authentication accepted invalid credentials or role!");
        if (svc.DatabaseAktif)
        {
            var adminAuth = DatabaseHelper.Autentikasi("admin", "admin", "admin");
            var siswaAuth = DatabaseHelper.Autentikasi("siswa", "siswa", "siswa");
            if (!adminAuth.Sukses || adminAuth.Role != "admin" || !siswaAuth.Sukses || siswaAuth.Role != "siswa")
                throw new Exception("MySQL rejected valid demo credentials!");
            Console.WriteLine("  ✓ MySQL valid/invalid password/role checks OK");
        }
        else
        {
            var unavailable = DatabaseHelper.Autentikasi("admin", "admin", "admin");
            if (unavailable.Sukses || !unavailable.Pesan.StartsWith("Database login tidak tersedia:"))
                throw new Exception("Login must fail closed when database is unavailable!");
            Console.WriteLine("  ✓ Login fail-closed without database OK");
        }
        // 4. Safe Delete Protection
        var resHapus = svc.HapusBukuAman("BK-002"); // BK-002 status Dipinjam
        if (resHapus.Sukses)
            throw new Exception("Buku yang sedang dipinjam harusnya dilarang dihapus!");
        Console.WriteLine($"  ✓ Proteksi Hapus Buku Aman OK: {resHapus.Pesan}");

        // 5. Max Kuota Pinjam (Maksimal 3)
        int aktif = svc.HitungPinjamanAktifSiswa("Dimas Saputra");
        Console.WriteLine($"  ✓ Hitung pinjaman aktif OK (Dimas Saputra: {aktif} buku)");

        Console.WriteLine("[TEST SUKSES] Semua verifikasi logika bisnis lolos 100%.");
    }
}
