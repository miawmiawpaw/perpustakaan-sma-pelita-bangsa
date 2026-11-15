using System.Data;
using System.Globalization;
using System.Drawing.Drawing2D;
using PerpustakaanPelitaBangsa.Models;
using PerpustakaanPelitaBangsa.Services;

namespace PerpustakaanPelitaBangsa.Forms;

public class MainForm : Form
{
    // =========================================================================
    // WARNA & TEMA FLAT SLATE MODERN (SAMA PERSIS DENGAN JAVA SWING)
    // =========================================================================
    public static readonly Color C_NAVY_DARK = Color.FromArgb(27, 54, 93);     // #1B365D - Hero Login
    public static readonly Color C_PRIMARY = Color.FromArgb(29, 59, 139);      // #1D3B8B - Primary Blue
    public static readonly Color C_BLUE_ACCENT = Color.FromArgb(37, 99, 235);  // #2563EB
    public static readonly Color C_BLUE_LIGHT = Color.FromArgb(239, 246, 255); // #EFF6FF
    public static readonly Color C_BG_APP = Color.FromArgb(248, 250, 252);     // #F8FAFC
    public static readonly Color C_BORDER = Color.FromArgb(226, 232, 240);     // #E2E8F0
    public static readonly Color C_TEXT_DARK = Color.FromArgb(15, 23, 42);     // #0F172A
    public static readonly Color C_TEXT_MUTED = Color.FromArgb(100, 116, 139); // #64748B
    public static readonly Color C_GREEN_BG = Color.FromArgb(209, 250, 229);   // #D1FAE5
    public static readonly Color C_GREEN_FG = Color.FromArgb(6, 95, 70);       // #065F46
    public static readonly Color C_RED_BG = Color.FromArgb(254, 226, 226);     // #FEE2E2
    public static readonly Color C_RED_FG = Color.FromArgb(153, 27, 27);       // #991B1B
    public static readonly Color C_AMBER_BG = Color.FromArgb(254, 243, 199);   // #FEF3C7
    public static readonly Color C_AMBER_FG = Color.FromArgb(146, 64, 14);     // #92400E

    private readonly PerpustakaanService _service = new();
    private Pengguna? _currentUser;

    // Komponen Container Utama
    private readonly Panel _pnlRoot = new() { Dock = DockStyle.Fill };
    private Panel _pnlLogin = null!;
    private Panel _pnlAppShell = null!;
    private Panel _pnlContentArea = null!;

    // Header User Info
    private Label _lblUserAvatar = null!;
    private Label _lblUserName = null!;
    private Label _lblUserRole = null!;
    private Label _lblGreetingTitle = null!;
    private Label _lblGreetingSub = null!;

    // Sidebar Items
    private readonly Dictionary<string, (Button Btn, Panel Pnl)> _pages = new();
    private Button? _activeSidebarBtn;

    // View: Dasbor
    private Label _lblStatTotalBuku = null!;
    private Label _lblStatBukuDipinjam = null!;
    private Label _lblStatBukuTerlambat = null!;
    private Label _lblStatTotalDenda = null!;
    private DataGridView _gridPinjamTerbaru = null!;
    private Panel _pnlBukuPopulerList = null!;

    // View: Data Buku
    private DataGridView _gridDataBuku = null!;
    private TextBox _txtSearchBuku = null!;
    private ComboBox _cmbKategoriBuku = null!;
    private ComboBox _cmbStatusBuku = null!;
    private Button _btnTambahBukuTop = null!;
    private Panel _pnlActionBuku = null!;
    private FlowLayoutPanel _pnlActionSirkulasi = null!;
    private Label _lblStripTotal = null!;
    private Label _lblStripTersedia = null!;
    private Label _lblStripDipinjam = null!;

    // View: Pencarian
    private TextBox _txtCariBesar = null!;
    private ComboBox _cmbKategoriCari = null!;
    private DataGridView _gridHasilCari = null!;
    private Panel _pnlDetailBuku = null!;
    private Label _lblDetailJudul = null!;
    private Label _lblDetailPenulis = null!;
    private Label _lblDetailKode = null!;
    private Label _lblDetailKategori = null!;
    private Label _lblDetailRak = null!;
    private Label _lblDetailStatusBadge = null!;
    private Button _btnDetailAksiPinjam = null!;
    private Buku? _selectedBukuDetail;

    // View: Peminjaman
    private DataGridView _gridSirkulasi = null!;
    private string _filterSirkulasi = "SEMUA";

    // View: Laporan
    private Label _lblLaporanTotalBuku = null!;
    private Label _lblLaporanTotalTrx = null!;
    private Label _lblLaporanTotalDenda = null!;
    private DataGridView _gridLaporanDenda = null!;

    public MainForm()
    {
        DoubleBuffered = true;
        Text = "Sistem Perpustakaan Digital SMA Pelita Bangsa";
        Size = new Size(1280, 800);
        MinimumSize = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
        BackColor = C_BG_APP;

        Controls.Add(_pnlRoot);

        BuildLoginPanel();
        BuildAppShell();
        TampilkanLogin();
    }

    // =========================================================================
    // 1. PANEL LOGIN
    // =========================================================================
    private void BuildLoginPanel()
    {
        _pnlLogin = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP };

        var split = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1
        };
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        _pnlLogin.Controls.Add(split);

        // --- SISI KIRI: HERO PANEL NAVY ---
        var hero = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = C_NAVY_DARK,
            Padding = new Padding(48, 50, 48, 40)
        };
        split.Controls.Add(hero, 0, 0);

        var lblHeroBrand = new Label
        {
            Text = "📚  Perpustakaan SMA Pelita Bangsa",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(48, 48)
        };
        hero.Controls.Add(lblHeroBrand);

        var lblHeroPill = new Label
        {
            Text = "  SISTEM DIGITALISASI PUSTAKA MODERN  ",
            Font = new Font("Segoe UI", 8f, FontStyle.Bold),
            ForeColor = Color.FromArgb(147, 197, 253),
            BackColor = Color.FromArgb(30, 64, 175),
            AutoSize = true,
            Location = new Point(48, 92),
            Padding = new Padding(6, 4, 6, 4)
        };
        hero.Controls.Add(lblHeroPill);

        var lblHeroTitle = new Label
        {
            Text = "Kelola Koleksi &\nSirkulasi Buku Lebih Cepat",
            Font = new Font("Segoe UI", 24f, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Location = new Point(48, 140)
        };
        hero.Controls.Add(lblHeroTitle);

        var lblHeroDesc = new Label
        {
            Text = "Aplikasi manajemen perpustakaan terpadu sekolah.\n" +
                   "Mendukung katalog otomatis, pelacakan peminjaman, proteksi kuota,\n" +
                   "serta perhitungan denda keterlambatan secara otomatis.",
            Font = new Font("Segoe UI", 10.5f, FontStyle.Regular),
            ForeColor = Color.FromArgb(203, 213, 225),
            AutoSize = true,
            Location = new Point(48, 230)
        };
        hero.Controls.Add(lblHeroDesc);

        // Box Hint Akun Pengujian
        // Demo accounts seed into the MySQL pengguna table on first run.
        var boxHint = new Panel
        {
            Location = new Point(48, 360),
            Size = new Size(480, 150),
            BackColor = Color.FromArgb(15, 35, 70),
            Padding = new Padding(16)
        };
        hero.Controls.Add(boxHint);

        var lblHintTitle = new Label
        {
            Text = "🔑  AKUN DEMO DATABASE",
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(245, 158, 11),
            Dock = DockStyle.Top,
            Height = 26
        };
        boxHint.Controls.Add(lblHintTitle);

        var lblHintDesc = new Label
        {
            Text = "Akun demo dibuat otomatis saat tabel pengguna masih kosong.\nAdmin dan Siswa login memakai kredensial MySQL.",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            ForeColor = Color.White,
            Dock = DockStyle.Fill
        };
        boxHint.Controls.Add(lblHintDesc);

        // --- SISI KANAN: FORM LOGIN ---
        var formContainer = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(72, 48, 72, 40)
        };
        split.Controls.Add(formContainer, 1, 0);

        int y = 42;
        var lblFormTitle = new Label
        {
            Text = "Masuk ke Akun Anda",
            Font = new Font("Segoe UI", 20f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Location = new Point(72, y),
            AutoSize = true
        };
        formContainer.Controls.Add(lblFormTitle);
        y += 38;
        var lblFormSub = new Label
        {
            Text = "Pilih peran pengguna dan masukkan kredensial untuk melanjutkan.",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            ForeColor = C_TEXT_MUTED,
            Location = new Point(72, y),
            AutoSize = true
        };
        formContainer.Controls.Add(lblFormSub);

        y += 44;

        // Toggle Peran
        var btnRolePustakawan = new Button
        {
            Text = "👑  Pustakawan",
            Location = new Point(72, y),
            Size = new Size(188, 40),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            BackColor = C_PRIMARY,
            ForeColor = Color.White
        };
        btnRolePustakawan.FlatAppearance.BorderSize = 0;

        var btnRoleSiswa = new Button
        {
            Text = "🎓  Siswa",
            Location = new Point(268, y),
            Size = new Size(188, 40),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            BackColor = Color.FromArgb(241, 245, 249),
            ForeColor = C_TEXT_DARK
        };
        btnRoleSiswa.FlatAppearance.BorderSize = 0;

        string roleSelected = "admin";

        btnRolePustakawan.Click += (s, e) =>
        {
            roleSelected = "admin";
            btnRolePustakawan.BackColor = C_PRIMARY;
            btnRolePustakawan.ForeColor = Color.White;
            btnRoleSiswa.BackColor = Color.FromArgb(241, 245, 249);
            btnRoleSiswa.ForeColor = C_TEXT_DARK;
        };

        btnRoleSiswa.Click += (s, e) =>
        {
            roleSelected = "siswa";
            btnRoleSiswa.BackColor = C_PRIMARY;
            btnRoleSiswa.ForeColor = Color.White;
            btnRolePustakawan.BackColor = Color.FromArgb(241, 245, 249);
            btnRolePustakawan.ForeColor = C_TEXT_DARK;
        };

        formContainer.Controls.Add(btnRolePustakawan);
        formContainer.Controls.Add(btnRoleSiswa);
        y += 56;

        // Input Username
        var lblU = new Label
        {
            Text = "Nama Pengguna (Username)",
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 85, 105),
            Location = new Point(72, y),
            AutoSize = true
        };
        formContainer.Controls.Add(lblU);
        y += 24;
        var txtUsername = new TextBox
        {
            Location = new Point(72, y),
            Size = new Size(384, 34),
            Font = new Font("Segoe UI", 11f),
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(248, 250, 252),
            Padding = new Padding(10, 7, 10, 7)
        };
        formContainer.Controls.Add(txtUsername);
        y += 46;
        // Input Password
        var lblP = new Label
        {
            Text = "Kata Sandi (Password)",
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 85, 105),
            Location = new Point(72, y),
            AutoSize = true
        };
        formContainer.Controls.Add(lblP);
        y += 24;

        var txtPassword = new TextBox
        {
            Location = new Point(72, y),
            Size = new Size(384, 34),
            Font = new Font("Segoe UI", 11f),
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(248, 250, 252),
            Padding = new Padding(10, 7, 10, 7),
            UseSystemPasswordChar = true
        };
        formContainer.Controls.Add(txtPassword);
        y += 48;

        // Tombol Masuk Utama
        var btnLogin = new Button
        {
            Text = "Masuk ke Sistem  ➔",
            Location = new Point(72, y),
            Size = new Size(384, 46),
            BackColor = C_PRIMARY,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold)
        };
        btnLogin.FlatAppearance.BorderSize = 0;
        ApplyRounded(btnRolePustakawan, 10);
        ApplyRounded(btnRoleSiswa, 10);
        ApplyRounded(txtUsername, 10);
        ApplyRounded(txtPassword, 10);
        ApplyRounded(btnLogin, 10);
        formContainer.Controls.Add(btnLogin);
        y += 56;

        var lblInfoAkun = new Label
        {
            Text = "Akun dikelola melalui database oleh admin sistem.",
            Location = new Point(72, y),
            Size = new Size(384, 42),
            ForeColor = C_TEXT_MUTED,
            Font = new Font("Segoe UI", 8.5f)
        };
        formContainer.Controls.Add(lblInfoAkun);

        // Handler Login
        void DoLogin()
        {
            string u = txtUsername.Text.Trim();
            string p = txtPassword.Text.Trim();
            if (string.IsNullOrWhiteSpace(u) || string.IsNullOrWhiteSpace(p))
            {
                MessageBox.Show("Nama pengguna dan kata sandi tidak boleh kosong.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var auth = DatabaseHelper.Autentikasi(u, p, roleSelected);
            if (!auth.Sukses)
            {
                MessageBox.Show(auth.Pesan, "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _currentUser = auth.Role == "admin"
                ? new Pustakawan(u, _service.DaftarBuku, _service.DaftarTransaksi)
                : new Siswa(u, _service.DaftarBuku, _service.DaftarTransaksi);

            SetupSetelahLogin();
            TampilkanAplikasi();
        }


        btnLogin.Click += (s, e) => DoLogin();
        txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoLogin(); };
    }

    // =========================================================================
    // 2. SHELL UTAMA APLIKASI (SIDEBAR + TOPBAR + CONTENT)
    // =========================================================================
    private void BuildAppShell()
    {
        _pnlAppShell = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP };

        // --- SIDEBAR KIRI (Width: 240px) ---
        var sidebar = new Panel
        {
            Dock = DockStyle.Left,
            Width = 240,
            BackColor = Color.White
        };
        sidebar.Paint += (s, e) =>
        {
            e.Graphics.DrawLine(new Pen(C_BORDER, 1), sidebar.Width - 1, 0, sidebar.Width - 1, sidebar.Height);
        };
        _pnlAppShell.Controls.Add(sidebar);

        // Brand Logo Atas
        var pnlBrand = new Panel { Dock = DockStyle.Top, Height = 80, Padding = new Padding(16, 18, 16, 12) };
        sidebar.Controls.Add(pnlBrand);

        var lblBrand1 = new Label
        {
            Text = "📖  Perpustakaan",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = C_PRIMARY,
            Location = new Point(14, 16),
            AutoSize = true
        };
        var lblBrand2 = new Label
        {
            Text = "SMA Pelita Bangsa",
            Font = new Font("Segoe UI", 9f, FontStyle.Regular),
            ForeColor = C_TEXT_MUTED,
            Location = new Point(16, 40),
            AutoSize = true
        };
        pnlBrand.Controls.Add(lblBrand1);
        pnlBrand.Controls.Add(lblBrand2);

        // Menu Navigasi Container
        var pnlNav = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(10, 10, 10, 10),
            AutoScroll = true
        };
        sidebar.Controls.Add(pnlNav);

        var lblMenuHeader = new Label
        {
            Text = "RUANG KERJA",
            Font = new Font("Segoe UI", 8f, FontStyle.Bold),
            ForeColor = Color.FromArgb(148, 163, 184),
            Margin = new Padding(10, 10, 0, 8),
            AutoSize = true
        };
        pnlNav.Controls.Add(lblMenuHeader);

        // Tombol Logout di Bawah
        var pnlLogout = new Panel { Dock = DockStyle.Bottom, Height = 70, Padding = new Padding(12) };
        sidebar.Controls.Add(pnlLogout);

        var btnLogout = new Button
        {
            Text = "🚪  Keluar Akun",
            Dock = DockStyle.Fill,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(254, 242, 242),
            ForeColor = Color.FromArgb(220, 38, 38),
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnLogout.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
        btnLogout.Click += (s, e) =>
        {
            var confirm = MessageBox.Show("Apakah Anda yakin ingin keluar dari sistem?", "Konfirmasi Keluar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _currentUser = null;
                TampilkanLogin();
            }
        };
        pnlLogout.Controls.Add(btnLogout);

        // --- AREA KANAN: TOPBAR + CONTENT ---
        var pnlRight = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP };
        _pnlAppShell.Controls.Add(pnlRight);
        pnlRight.BringToFront();

        // Topbar
        var topbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 72,
            BackColor = Color.White,
            Padding = new Padding(24, 12, 24, 12)
        };
        topbar.Paint += (s, e) =>
        {
            e.Graphics.DrawLine(new Pen(C_BORDER, 1), 0, topbar.Height - 1, topbar.Width, topbar.Height - 1);
        };
        pnlRight.Controls.Add(topbar);

        _lblGreetingTitle = new Label
        {
            Text = "Selamat pagi 👋",
            Font = new Font("Segoe UI", 13f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Location = new Point(24, 14),
            AutoSize = true
        };
        _lblGreetingSub = new Label
        {
            Text = "Pantau pergerakan koleksi dan sirkulasi hari ini.",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
            ForeColor = C_TEXT_MUTED,
            Location = new Point(24, 40),
            AutoSize = true
        };
        topbar.Controls.Add(_lblGreetingTitle);
        topbar.Controls.Add(_lblGreetingSub);

        // Profil User Kanan Atas
        var pnlProfile = new Panel
        {
            Dock = DockStyle.Right,
            Width = 260
        };
        topbar.Controls.Add(pnlProfile);

        _lblUserAvatar = new Label
        {
            Size = new Size(38, 38),
            Location = new Point(10, 6),
            BackColor = C_PRIMARY,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter,
            Text = "RA"
        };
        pnlProfile.Controls.Add(_lblUserAvatar);

        _lblUserName = new Label
        {
            Text = "Rina Anggraini",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Location = new Point(56, 6),
            AutoSize = true
        };
        _lblUserRole = new Label
        {
            Text = "Pustakawan",
            Font = new Font("Segoe UI", 8f, FontStyle.Regular),
            ForeColor = C_TEXT_MUTED,
            Location = new Point(56, 26),
            AutoSize = true
        };
        pnlProfile.Controls.Add(_lblUserName);
        pnlProfile.Controls.Add(_lblUserRole);

        // Area Konten Dinamis
        _pnlContentArea = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = C_BG_APP,
            Padding = new Padding(24)
        };
        pnlRight.Controls.Add(_pnlContentArea);
        _pnlContentArea.BringToFront();

        // Buat View Halaman
        var vDasbor = BuatViewDasbor();
        var vCari = BuatViewPencarian();
        var vDataBuku = BuatViewDataBuku();
        var vSirkulasi = BuatViewSirkulasi();
        var vAnggota = BuatViewAnggota();
        var vLaporan = BuatViewLaporan();

        void AddPage(string key, string label, string icon, Panel view)
        {
            view.Dock = DockStyle.Fill;
            view.Visible = false;
            _pnlContentArea.Controls.Add(view);

            var btn = new Button
            {
                Text = $"  {icon}  {label}",
                Width = 216,
                Height = 42,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = C_TEXT_DARK,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 0, 2)
            };
            btn.FlatAppearance.BorderSize = 0;

            btn.Click += (s, e) => BeralihMenu(key);
            pnlNav.Controls.Add(btn);

            _pages[key] = (btn, view);
        }

        AddPage("DASBOR", "Dasbor", "📊", vDasbor);
        AddPage("PENCARIAN", "Pencarian Buku", "🔍", vCari);
        AddPage("DATABUKU", "Data Buku", "📚", vDataBuku);
        AddPage("PEMINJAMAN", "Peminjaman", "🔄", vSirkulasi);
        AddPage("ANGGOTA", "Anggota", "👥", vAnggota);
        AddPage("LAPORAN", "Laporan", "📑", vLaporan);
    }

    private void BeralihMenu(string key)
    {
        if (_currentUser is Siswa && (key == "ANGGOTA" || key == "LAPORAN")) return;
        if (!_pages.TryGetValue(key, out var target)) return;

        foreach (var p in _pages.Values)
        {
            p.Pnl.Visible = false;
            p.Btn.BackColor = Color.White;
            p.Btn.ForeColor = C_TEXT_DARK;
        }

        target.Pnl.Visible = true;
        target.Btn.BackColor = C_BLUE_LIGHT;
        target.Btn.ForeColor = C_PRIMARY;
        _activeSidebarBtn = target.Btn;

        RefreshSemuaData();
    }

    // =========================================================================
    // 3. KONTROL HAK AKSES & SETUP SETELAH LOGIN
    // =========================================================================
    private void SetupSetelahLogin()
    {
        bool isAdmin = _currentUser is Pustakawan;

        if (isAdmin)
        {
            _lblUserAvatar.Text = "RA";
            _lblUserAvatar.BackColor = C_PRIMARY;
            _lblUserName.Text = "Rina Anggraini";
            _lblUserRole.Text = "Pustakawan Pelita Bangsa";
            _lblGreetingTitle.Text = "Selamat pagi, Bu Rina 👋";
            string statusDb = _service.DatabaseAktif ? "🟢 MySQL XAMPP Aktif" : "📁 Penyimpanan Flat-File (.txt)";
            _lblGreetingSub.Text = $"Kelola koleksi & sirkulasi hari ini. [Penyimpanan: {statusDb}]";
        }
        else
        {
            string uname = _currentUser?.Username ?? "Siswa";
            _lblUserAvatar.Text = uname.Length >= 2 ? uname[..2].ToUpper() : "SW";
            _lblUserAvatar.BackColor = Color.FromArgb(16, 185, 129);
            _lblUserName.Text = uname.ToUpper();
            _lblUserRole.Text = "Siswa Pelita Bangsa";
            _lblGreetingTitle.Text = $"Selamat datang, {uname} 👋";
            string statusDbSiswa = _service.DatabaseAktif ? "🟢 MySQL XAMPP Aktif" : "📁 Flat-File (.txt)";
            _lblGreetingSub.Text = $"Jelajahi koleksi & pantau sirkulasi peminjaman. [Penyimpanan: {statusDbSiswa}]";
        }

        _btnTambahBukuTop.Visible = isAdmin;
        _pnlActionBuku.Visible = isAdmin;
        _pnlActionSirkulasi.Visible = isAdmin;
        _btnDetailAksiPinjam.Visible = isAdmin;
        _pages["ANGGOTA"].Btn.Visible = isAdmin;
        _pages["LAPORAN"].Btn.Visible = isAdmin;

        BeralihMenu("DASBOR");
    }

    private void TampilkanLogin()
    {
        _pnlRoot.Controls.Clear();
        _pnlRoot.Controls.Add(_pnlLogin);
    }

    private void TampilkanAplikasi()
    {
        _pnlRoot.Controls.Clear();
        _pnlRoot.Controls.Add(_pnlAppShell);
    }

    // =========================================================================
    // 4. VIEW: DASBOR
    // =========================================================================
    private Panel BuatViewDasbor()
    {
        var panel = new Panel { AutoScroll = true, BackColor = C_BG_APP };

        // Strip Metrik 4 Kartu
        var pnlMetrics = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 110,
            ColumnCount = 4,
            RowCount = 1
        };
        for (int i = 0; i < 4; i++) pnlMetrics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
        panel.Controls.Add(pnlMetrics);

        Panel BuatKartuStat(string title, string icon, Color accent, out Label lblVal)
        {
            var c = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(6),
                Padding = new Padding(16, 14, 16, 14)
            };
            c.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, c.Width - 1, c.Height - 1);

            var lT = new Label
            {
                Text = $"{icon}  {title}",
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = C_TEXT_MUTED,
                Dock = DockStyle.Top,
                Height = 22
            };
            lblVal = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = accent,
                Dock = DockStyle.Fill
            };
            c.Controls.Add(lblVal);
            c.Controls.Add(lT);
            return c;
        }

        pnlMetrics.Controls.Add(BuatKartuStat("TOTAL KOLEKSI", "📚", C_PRIMARY, out _lblStatTotalBuku), 0, 0);
        pnlMetrics.Controls.Add(BuatKartuStat("BUKU DIPINJAM", "🔄", Color.FromArgb(37, 99, 235), out _lblStatBukuDipinjam), 1, 0);
        pnlMetrics.Controls.Add(BuatKartuStat("TERLAMBAT DIKEMBALIKAN", "⚠️", Color.FromArgb(220, 38, 38), out _lblStatBukuTerlambat), 2, 0);
        pnlMetrics.Controls.Add(BuatKartuStat("DENDA TERKUMPUL", "💰", Color.FromArgb(16, 185, 129), out _lblStatTotalDenda), 3, 0);

        // Body: 2 Kolom (Kiri: Sirkulasi Terbaru, Kanan: Buku Terpopuler & Akses Cepat)
        var pnlBody = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 520,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(0, 16, 0, 0)
        };
        pnlBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65f));
        pnlBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35f));
        panel.Controls.Add(pnlBody);
        pnlBody.BringToFront();

        // Kolom Kiri: Tabel Sirkulasi Terbaru
        var cardRecent = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(6),
            Padding = new Padding(16)
        };
        cardRecent.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, cardRecent.Width - 1, cardRecent.Height - 1);
        pnlBody.Controls.Add(cardRecent, 0, 0);

        var lblRecentTitle = new Label
        {
            Text = "🔄  Sirkulasi Transaksi Terbaru",
            Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Dock = DockStyle.Top,
            Height = 36
        };
        cardRecent.Controls.Add(lblRecentTitle);

        _gridPinjamTerbaru = BuatGridStandar();
        _gridPinjamTerbaru.Columns.Add("Peminjam", "Nama Peminjam");
        _gridPinjamTerbaru.Columns.Add("Judul", "Judul Buku");
        _gridPinjamTerbaru.Columns.Add("TglPinjam", "Tgl Pinjam");
        _gridPinjamTerbaru.Columns.Add("Status", "Status");
        cardRecent.Controls.Add(_gridPinjamTerbaru);
        _gridPinjamTerbaru.BringToFront();

        // Kolom Kanan: Buku Populer & Tindakan Cepat
        var cardRight = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(6),
            Padding = new Padding(16)
        };
        cardRight.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, cardRight.Width - 1, cardRight.Height - 1);
        pnlBody.Controls.Add(cardRight, 1, 0);

        var lblPopTitle = new Label
        {
            Text = "⭐  Koleksi Terpopuler",
            Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Dock = DockStyle.Top,
            Height = 36
        };
        cardRight.Controls.Add(lblPopTitle);

        _pnlBukuPopulerList = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
        cardRight.Controls.Add(_pnlBukuPopulerList);
        _pnlBukuPopulerList.BringToFront();

        return panel;
    }

    // =========================================================================
    // 5. VIEW: DATA BUKU (CRUD & PENGELOLAAN LENGKAP)
    // =========================================================================
    private Panel BuatViewDataBuku()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP };

        // Strip Ringkasan
        var pnlStrip = new Panel
        {
            Dock = DockStyle.Top,
            Height = 44,
            BackColor = Color.White,
            Padding = new Padding(16, 10, 16, 10)
        };
        pnlStrip.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER), 0, pnlStrip.Height - 1, pnlStrip.Width, pnlStrip.Height - 1);
        panel.Controls.Add(pnlStrip);

        _lblStripTotal = new Label { Text = "Total: 0 buku", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = C_PRIMARY, Location = new Point(16, 12), AutoSize = true };
        _lblStripTersedia = new Label { Text = "Tersedia: 0", Font = new Font("Segoe UI", 9f), ForeColor = C_GREEN_FG, Location = new Point(160, 12), AutoSize = true };
        _lblStripDipinjam = new Label { Text = "Dipinjam: 0", Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(220, 38, 38), Location = new Point(280, 12), AutoSize = true };
        pnlStrip.Controls.Add(_lblStripTotal);
        pnlStrip.Controls.Add(_lblStripTersedia);
        pnlStrip.Controls.Add(_lblStripDipinjam);

        // Toolbar Atas (Filter & Tombol Tambah)
        var pnlToolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(0, 12, 0, 12)
        };
        panel.Controls.Add(pnlToolbar);
        pnlToolbar.BringToFront();

        _txtSearchBuku = new TextBox
        {
            Location = new Point(0, 14),
            Size = new Size(240, 30),
            Font = new Font("Segoe UI", 9.5f),
            PlaceholderText = "🔍 Cari judul atau penulis..."
        };
        _txtSearchBuku.TextChanged += (s, e) => FilterDataBuku();
        pnlToolbar.Controls.Add(_txtSearchBuku);
        ApplyRounded(_txtSearchBuku, 8);

        _cmbKategoriBuku = new ComboBox
        {
            Location = new Point(250, 14),
            Size = new Size(140, 30),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _cmbKategoriBuku.Items.AddRange(new object[] { "Semua Kategori", "Novel", "Pendidikan", "Sains", "Psikologi", "Sejarah", "Teknologi" });
        _cmbKategoriBuku.SelectedIndex = 0;
        _cmbKategoriBuku.SelectedIndexChanged += (s, e) => FilterDataBuku();
        pnlToolbar.Controls.Add(_cmbKategoriBuku);
        ApplyRounded(_cmbKategoriBuku, 8);

        _cmbStatusBuku = new ComboBox
        {
            Location = new Point(400, 14),
            Size = new Size(130, 30),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _cmbStatusBuku.Items.AddRange(new object[] { "Semua Status", "Tersedia", "Dipinjam" });
        _cmbStatusBuku.SelectedIndex = 0;
        _cmbStatusBuku.SelectedIndexChanged += (s, e) => FilterDataBuku();
        pnlToolbar.Controls.Add(_cmbStatusBuku);
        ApplyRounded(_cmbStatusBuku, 8);

        _btnTambahBukuTop = new Button
        {
            Text = "+ Tambah Buku Baru",
            Size = new Size(160, 34),
            Location = new Point(540, 12),
            BackColor = C_PRIMARY,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        _btnTambahBukuTop.FlatAppearance.BorderSize = 0;
        _btnTambahBukuTop.Click += BtnTambahBuku_Click;
        pnlToolbar.Controls.Add(_btnTambahBukuTop);

        // Toolbar Bawah: Tombol Aksi Item
        _pnlActionBuku = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 56,
            Padding = new Padding(0, 10, 0, 10)
        };
        panel.Controls.Add(_pnlActionBuku);

        var btnEdit = new Button
        {
            Text = "✏️  Edit Buku",
            Size = new Size(120, 36),
            Location = new Point(0, 10),
            BackColor = Color.White,
            ForeColor = C_TEXT_DARK,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        btnEdit.FlatAppearance.BorderColor = C_BORDER;
        btnEdit.Click += BtnEditBuku_Click;
        _pnlActionBuku.Controls.Add(btnEdit);

        var btnHapus = new Button
        {
            Text = "🗑️  Hapus Aman",
            Size = new Size(130, 36),
            Location = new Point(130, 10),
            BackColor = Color.FromArgb(254, 242, 242),
            ForeColor = Color.FromArgb(220, 38, 38),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        btnHapus.FlatAppearance.BorderColor = Color.FromArgb(254, 202, 202);
        btnHapus.Click += BtnHapusBuku_Click;
        _pnlActionBuku.Controls.Add(btnHapus);

        var btnPinjam = new Button
        {
            Text = "🔄  Pinjamkan Buku",
            Size = new Size(140, 36),
            Location = new Point(270, 10),
            BackColor = C_BLUE_LIGHT,
            ForeColor = C_PRIMARY,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        btnPinjam.FlatAppearance.BorderColor = Color.FromArgb(191, 219, 254);
        btnPinjam.Click += BtnPinjamBukuToolbar_Click;
        _pnlActionBuku.Controls.Add(btnPinjam);

        var btnKembalikan = new Button
        {
            Text = "✓  Kembalikan Buku",
            Size = new Size(140, 36),
            Location = new Point(420, 10),
            BackColor = C_GREEN_BG,
            ForeColor = C_GREEN_FG,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        btnKembalikan.FlatAppearance.BorderColor = Color.FromArgb(167, 243, 208);
        btnKembalikan.Click += BtnKembalikanBukuToolbar_Click;
        _pnlActionBuku.Controls.Add(btnKembalikan);

        // Tabel Data Buku
        _gridDataBuku = BuatGridStandar();
        _gridDataBuku.Columns.Add("Kode", "Kode");
        _gridDataBuku.Columns.Add("Judul", "Judul Buku");
        _gridDataBuku.Columns.Add("Penulis", "Penulis");
        _gridDataBuku.Columns.Add("Kategori", "Kategori");
        _gridDataBuku.Columns.Add("Tahun", "Tahun");
        _gridDataBuku.Columns.Add("Status", "Status");
        _gridDataBuku.Columns.Add("Rak", "Lokasi Rak");

        panel.Controls.Add(_gridDataBuku);
        _gridDataBuku.BringToFront();

        return panel;
    }

    private void FilterDataBuku()
    {
        string kw = _txtSearchBuku.Text.Trim().ToLowerInvariant();
        string kat = _cmbKategoriBuku.SelectedItem?.ToString() ?? "Semua Kategori";
        string st = _cmbStatusBuku.SelectedItem?.ToString() ?? "Semua Status";

        _gridDataBuku.Rows.Clear();

        foreach (var b in _service.DaftarBuku)
        {
            if (!string.IsNullOrEmpty(kw))
            {
                if (!b.Judul.ToLowerInvariant().Contains(kw) &&
                    !b.Penulis.ToLowerInvariant().Contains(kw) &&
                    !b.KodeBuku.ToLowerInvariant().Contains(kw))
                    continue;
            }

            if (kat != "Semua Kategori" && !b.Kategori.Equals(kat, StringComparison.OrdinalIgnoreCase))
                continue;

            if (st == "Tersedia" && !b.Tersedia) continue;
            if (st == "Dipinjam" && b.Tersedia) continue;

            int rowIdx = _gridDataBuku.Rows.Add(
                b.KodeBuku,
                b.Judul,
                b.Penulis,
                b.Kategori,
                b.Tahun,
                b.Tersedia ? "Tersedia" : "Dipinjam",
                b.LokasiRak
            );

            var row = _gridDataBuku.Rows[rowIdx];
            if (!b.Tersedia)
            {
                row.Cells["Status"].Style.ForeColor = Color.FromArgb(185, 28, 28);
                row.Cells["Status"].Style.BackColor = C_RED_BG;
            }
            else
            {
                row.Cells["Status"].Style.ForeColor = C_GREEN_FG;
                row.Cells["Status"].Style.BackColor = C_GREEN_BG;
            }
        }
    }

    private void BtnTambahBuku_Click(object? sender, EventArgs e)
    {
        using var dlg = new DialogTambahBuku();
        if (dlg.ShowDialog(this) == DialogResult.OK && dlg.HasilBuku != null)
        {
            var res = _service.TambahBuku(dlg.HasilBuku);
            MessageBox.Show(res.Pesan, res.Sukses ? "Berhasil" : "Gagal",
                MessageBoxButtons.OK, res.Sukses ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            RefreshSemuaData();
        }
    }

    private void BtnEditBuku_Click(object? sender, EventArgs e)
    {
        if (_gridDataBuku.SelectedRows.Count == 0)
        {
            MessageBox.Show("Pilih baris buku yang ingin diedit.", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string kode = _gridDataBuku.SelectedRows[0].Cells["Kode"].Value?.ToString() ?? "";
        var buku = _service.DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kode, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return;

        using var dlg = new DialogTambahBuku(buku);
        if (dlg.ShowDialog(this) == DialogResult.OK && dlg.HasilBuku != null)
        {
            var h = dlg.HasilBuku;
            var res = _service.UbahBuku(buku.KodeBuku, h.Judul, h.Penulis, h.LokasiRak, h.Kategori, h.Tahun);
            MessageBox.Show(res.Pesan, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshSemuaData();
        }
    }

    private void BtnHapusBuku_Click(object? sender, EventArgs e)
    {
        if (_gridDataBuku.SelectedRows.Count == 0)
        {
            MessageBox.Show("Pilih baris buku yang ingin dihapus.", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string kode = _gridDataBuku.SelectedRows[0].Cells["Kode"].Value?.ToString() ?? "";
        var buku = _service.DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kode, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return;

        // Validasi hapus aman: jika buku dipinjam, tolak langsung!
        if (!buku.Tersedia)
        {
            MessageBox.Show($"Buku '{buku.Judul}' saat ini sedang DIPINJAM siswa.\n" +
                            "Buku yang sedang dipinjam dilarang dihapus dari katalog!",
                            "Proteksi Hapus Aman", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show($"Yakin ingin menghapus buku '{buku.Judul}' ({buku.KodeBuku})?",
            "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
        {
            var res = _service.HapusBukuAman(buku.KodeBuku);
            MessageBox.Show(res.Pesan, res.Sukses ? "Sukses" : "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshSemuaData();
        }
    }

    private void BtnPinjamBukuToolbar_Click(object? sender, EventArgs e)
    {
        string? preKode = null;
        if (_gridDataBuku.SelectedRows.Count > 0)
        {
            preKode = _gridDataBuku.SelectedRows[0].Cells["Kode"].Value?.ToString();
        }

        string? preSiswa = (_currentUser is Siswa) ? _currentUser.Username : null;
        using var dlg = new DialogPinjamBuku(_service, preKode, preSiswa);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            RefreshSemuaData();
        }
    }

    private void BtnKembalikanBukuToolbar_Click(object? sender, EventArgs e)
    {
        if (_gridDataBuku.SelectedRows.Count == 0)
        {
            MessageBox.Show("Pilih baris buku yang ingin dikembalikan.", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string kode = _gridDataBuku.SelectedRows[0].Cells["Kode"].Value?.ToString() ?? "";
        var buku = _service.DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kode, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return;

        if (buku.Tersedia)
        {
            MessageBox.Show($"Buku '{buku.Judul}' sudah berstatus Tersedia di rak.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var res = _service.KembalikanBuku(buku.KodeBuku);
        MessageBox.Show(res.Pesan, res.Sukses ? "Pengembalian Selesai" : "Gagal",
            MessageBoxButtons.OK, res.Denda > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

        RefreshSemuaData();
    }

    // =========================================================================
    // 6. VIEW: PENCARIAN BUKU (SPLIT CARI & DETAIL PREVIEW)
    // =========================================================================
    private Panel BuatViewPencarian()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP };

        // Search Header
        var pnlSearchHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 70,
            BackColor = Color.White,
            Padding = new Padding(16, 14, 16, 14)
        };
        pnlSearchHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER), 0, pnlSearchHeader.Height - 1, pnlSearchHeader.Width, pnlSearchHeader.Height - 1);
        panel.Controls.Add(pnlSearchHeader);

        _txtCariBesar = new TextBox
        {
            Location = new Point(16, 16),
            Size = new Size(450, 34),
            Font = new Font("Segoe UI", 11f),
            PlaceholderText = "🔍 Ketik judul buku, pengarang, topik, atau kode..."
        };
        _txtCariBesar.TextChanged += (s, e) => LakukanPencarian();
        pnlSearchHeader.Controls.Add(_txtCariBesar);

        _cmbKategoriCari = new ComboBox
        {
            Location = new Point(480, 16),
            Size = new Size(160, 34),
            Font = new Font("Segoe UI", 10f),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        _cmbKategoriCari.Items.AddRange(new object[] { "Semua Kategori", "Novel", "Pendidikan", "Sains", "Psikologi", "Sejarah", "Teknologi" });
        _cmbKategoriCari.SelectedIndex = 0;
        _cmbKategoriCari.SelectedIndexChanged += (s, e) => LakukanPencarian();
        pnlSearchHeader.Controls.Add(_cmbKategoriCari);

        // Body: Split 2 Kolom (Hasil Tabel & Panel Detail Buku)
        var split = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 1,
            Padding = new Padding(0, 14, 0, 0)
        };
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62f));
        split.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38f));
        panel.Controls.Add(split);
        split.BringToFront();

        // Kolom Kiri: Tabel Hasil
        var pnlGridCari = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(4)
        };
        pnlGridCari.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, pnlGridCari.Width - 1, pnlGridCari.Height - 1);
        split.Controls.Add(pnlGridCari, 0, 0);

        _gridHasilCari = BuatGridStandar();
        _gridHasilCari.Columns.Add("Kode", "Kode");
        _gridHasilCari.Columns.Add("Judul", "Judul Buku");
        _gridHasilCari.Columns.Add("Penulis", "Penulis");
        _gridHasilCari.Columns.Add("Kategori", "Kategori");
        _gridHasilCari.Columns.Add("Status", "Status");
        _gridHasilCari.SelectionChanged += GridHasilCari_SelectionChanged;
        pnlGridCari.Controls.Add(_gridHasilCari);

        // Kolom Kanan: Card Detail Buku
        _pnlDetailBuku = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(4),
            Padding = new Padding(24)
        };
        _pnlDetailBuku.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, _pnlDetailBuku.Width - 1, _pnlDetailBuku.Height - 1);
        split.Controls.Add(_pnlDetailBuku, 1, 0);

        int dy = 20;
        var lblDetHead = new Label
        {
            Text = "📖  INFORMASI DETAIL BUKU",
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = C_TEXT_MUTED,
            Location = new Point(24, dy),
            AutoSize = true
        };
        _pnlDetailBuku.Controls.Add(lblDetHead);
        dy += 34;

        _lblDetailJudul = new Label
        {
            Text = "Pilih buku dari daftar",
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Location = new Point(24, dy),
            Size = new Size(340, 56)
        };
        _pnlDetailBuku.Controls.Add(_lblDetailJudul);
        dy += 60;

        _lblDetailPenulis = new Label
        {
            Text = "-",
            Font = new Font("Segoe UI", 10f, FontStyle.Italic),
            ForeColor = C_TEXT_MUTED,
            Location = new Point(24, dy),
            AutoSize = true
        };
        _pnlDetailBuku.Controls.Add(_lblDetailPenulis);
        dy += 34;

        void AddRowInfo(string title, out Label val)
        {
            var lT = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Location = new Point(24, dy),
                AutoSize = true
            };
            _pnlDetailBuku.Controls.Add(lT);

            val = new Label
            {
                Text = "-",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = C_TEXT_DARK,
                Location = new Point(140, dy),
                AutoSize = true
            };
            _pnlDetailBuku.Controls.Add(val);
            dy += 28;
        }

        AddRowInfo("Kode Buku", out _lblDetailKode);
        AddRowInfo("Kategori", out _lblDetailKategori);
        AddRowInfo("Lokasi Rak", out _lblDetailRak);

        dy += 6;
        var lSt = new Label
        {
            Text = "Status Buku",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 85, 105),
            Location = new Point(24, dy),
            AutoSize = true
        };
        _pnlDetailBuku.Controls.Add(lSt);

        _lblDetailStatusBadge = new Label
        {
            Text = "Tersedia",
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            ForeColor = C_GREEN_FG,
            BackColor = C_GREEN_BG,
            Location = new Point(140, dy),
            Padding = new Padding(6, 3, 6, 3),
            AutoSize = true
        };
        _pnlDetailBuku.Controls.Add(_lblDetailStatusBadge);
        dy += 50;

        _btnDetailAksiPinjam = new Button
        {
            Text = "Pinjam Buku Ini",
            Location = new Point(24, dy),
            Size = new Size(220, 40),
            BackColor = C_PRIMARY,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        _btnDetailAksiPinjam.FlatAppearance.BorderSize = 0;
        _btnDetailAksiPinjam.Click += (s, e) =>
        {
            if (_selectedBukuDetail == null) return;
            string? preSiswa = (_currentUser is Siswa) ? _currentUser.Username : null;
            using var dlg = new DialogPinjamBuku(_service, _selectedBukuDetail.KodeBuku, preSiswa);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshSemuaData();
            }
        };
        _pnlDetailBuku.Controls.Add(_btnDetailAksiPinjam);

        return panel;
    }

    private void LakukanPencarian()
    {
        string kw = _txtCariBesar.Text.Trim().ToLowerInvariant();
        string kat = _cmbKategoriCari.SelectedItem?.ToString() ?? "Semua Kategori";

        _gridHasilCari.Rows.Clear();

        foreach (var b in _service.DaftarBuku)
        {
            if (!string.IsNullOrEmpty(kw))
            {
                if (!b.Judul.ToLowerInvariant().Contains(kw) &&
                    !b.Penulis.ToLowerInvariant().Contains(kw) &&
                    !b.KodeBuku.ToLowerInvariant().Contains(kw))
                    continue;
            }

            if (kat != "Semua Kategori" && !b.Kategori.Equals(kat, StringComparison.OrdinalIgnoreCase))
                continue;

            int rowIdx = _gridHasilCari.Rows.Add(
                b.KodeBuku,
                b.Judul,
                b.Penulis,
                b.Kategori,
                b.Tersedia ? "Tersedia" : "Dipinjam"
            );

            var row = _gridHasilCari.Rows[rowIdx];
            if (!b.Tersedia)
            {
                row.Cells["Status"].Style.ForeColor = Color.FromArgb(185, 28, 28);
                row.Cells["Status"].Style.BackColor = C_RED_BG;
            }
            else
            {
                row.Cells["Status"].Style.ForeColor = C_GREEN_FG;
                row.Cells["Status"].Style.BackColor = C_GREEN_BG;
            }
        }

        if (_gridHasilCari.Rows.Count > 0)
        {
            _gridHasilCari.Rows[0].Selected = true;
        }
    }

    private void GridHasilCari_SelectionChanged(object? sender, EventArgs e)
    {
        if (_gridHasilCari.SelectedRows.Count == 0) return;
        string kode = _gridHasilCari.SelectedRows[0].Cells["Kode"].Value?.ToString() ?? "";
        var buku = _service.DaftarBuku.FirstOrDefault(b => b.KodeBuku.Equals(kode, StringComparison.OrdinalIgnoreCase));
        if (buku == null) return;

        _selectedBukuDetail = buku;
        _lblDetailJudul.Text = buku.Judul;
        _lblDetailPenulis.Text = $"Karya: {buku.Penulis} ({buku.Tahun})";
        _lblDetailKode.Text = buku.KodeBuku;
        _lblDetailKategori.Text = buku.Kategori;
        _lblDetailRak.Text = buku.LokasiRak;

        if (buku.Tersedia)
        {
            _lblDetailStatusBadge.Text = "Tersedia di Rak";
            _lblDetailStatusBadge.ForeColor = C_GREEN_FG;
            _lblDetailStatusBadge.BackColor = C_GREEN_BG;
            _btnDetailAksiPinjam.Enabled = true;
            _btnDetailAksiPinjam.Text = "Pinjam Buku Ini";
            _btnDetailAksiPinjam.BackColor = C_PRIMARY;
        }
        else
        {
            _lblDetailStatusBadge.Text = "Sedang Dipinjam";
            _lblDetailStatusBadge.ForeColor = Color.FromArgb(185, 28, 28);
            _lblDetailStatusBadge.BackColor = C_RED_BG;
            _btnDetailAksiPinjam.Enabled = false;
            _btnDetailAksiPinjam.Text = "Buku Sedang Dipinjam";
            _btnDetailAksiPinjam.BackColor = Color.FromArgb(203, 213, 225);
        }
    }

    // =========================================================================
    // 7. VIEW: PEMINJAMAN & SIRKULASI
    // =========================================================================
    private Panel BuatViewSirkulasi()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP };

        // Filter Bar
        var pnlFilter = new Panel
        {
            Dock = DockStyle.Top,
            Height = 56,
            BackColor = Color.White,
            Padding = new Padding(16, 10, 16, 10)
        };
        pnlFilter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(C_BORDER), 0, pnlFilter.Height - 1, pnlFilter.Width, pnlFilter.Height - 1);
        panel.Controls.Add(pnlFilter);

        var pnlButtonsFilter = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 520,
            FlowDirection = FlowDirection.LeftToRight
        };
        pnlFilter.Controls.Add(pnlButtonsFilter);

        void AddFilterBtn(string code, string text)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(115, 34),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                BackColor = (_filterSirkulasi == code) ? C_PRIMARY : Color.White,
                ForeColor = (_filterSirkulasi == code) ? Color.White : C_TEXT_DARK,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = C_BORDER;
            btn.Click += (s, e) =>
            {
                _filterSirkulasi = code;
                foreach (Control c in pnlButtonsFilter.Controls)
                {
                    if (c is Button b)
                    {
                        b.BackColor = Color.White;
                        b.ForeColor = C_TEXT_DARK;
                    }
                }
                btn.BackColor = C_PRIMARY;
                btn.ForeColor = Color.White;
                RefreshTabelSirkulasi();
            };
            pnlButtonsFilter.Controls.Add(btn);
        }

        AddFilterBtn("SEMUA", "Semua Data");
        AddFilterBtn("DIPINJAM", "Aktif Dipinjam");
        AddFilterBtn("TERLAMBAT", "Terlambat");
        AddFilterBtn("KEMBALI", "Dikembalikan");

        // Tombol Pinjam & Kembali Cepat
        _pnlActionSirkulasi = new FlowLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 320,
            FlowDirection = FlowDirection.RightToLeft
        };
        pnlFilter.Controls.Add(_pnlActionSirkulasi);

        var btnKembaliSirkulasi = new Button
        {
            Text = "✓ Proses Kembali",
            Size = new Size(140, 34),
            BackColor = C_GREEN_BG,
            ForeColor = C_GREEN_FG,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnKembaliSirkulasi.FlatAppearance.BorderColor = Color.FromArgb(167, 243, 208);
        btnKembaliSirkulasi.Click += (s, e) =>
        {
            if (_gridSirkulasi.SelectedRows.Count == 0)
            {
                MessageBox.Show("Pilih baris transaksi yang ingin dikembalikan.", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string kode = _gridSirkulasi.SelectedRows[0].Cells["Kode"].Value?.ToString() ?? "";
            var res = _service.KembalikanBuku(kode);
            MessageBox.Show(res.Pesan, res.Sukses ? "Pengembalian Berhasil" : "Peringatan",
                MessageBoxButtons.OK, res.Denda > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
            RefreshSemuaData();
        };

        var btnPinjamSirkulasi = new Button
        {
            Text = "+ Pinjam Baru",
            Size = new Size(130, 34),
            BackColor = C_PRIMARY,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnPinjamSirkulasi.FlatAppearance.BorderSize = 0;
        btnPinjamSirkulasi.Click += (s, e) =>
        {
            string? preSiswa = (_currentUser is Siswa) ? _currentUser.Username : null;
            using var dlg = new DialogPinjamBuku(_service, null, preSiswa);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                RefreshSemuaData();
            }
        };

        _pnlActionSirkulasi.Controls.Add(btnKembaliSirkulasi);
        _pnlActionSirkulasi.Controls.Add(btnPinjamSirkulasi);

        // Tabel Riwayat Sirkulasi
        _gridSirkulasi = BuatGridStandar();
        _gridSirkulasi.Columns.Add("ID", "ID Trx");
        _gridSirkulasi.Columns.Add("Kode", "Kode Buku");
        _gridSirkulasi.Columns.Add("Judul", "Judul Buku");
        _gridSirkulasi.Columns.Add("Peminjam", "Nama Peminjam");
        _gridSirkulasi.Columns.Add("TglPinjam", "Tgl Pinjam");
        _gridSirkulasi.Columns.Add("JatuhTempo", "Jatuh Tempo (H+7)");
        _gridSirkulasi.Columns.Add("TglKembali", "Tgl Kembali");
        _gridSirkulasi.Columns.Add("Status", "Status Sirkulasi");

        panel.Controls.Add(_gridSirkulasi);
        _gridSirkulasi.BringToFront();

        return panel;
    }

    private void RefreshTabelSirkulasi()
    {
        _gridSirkulasi.Rows.Clear();
        DateTime today = DateTime.Today;

        var list = _service.DaftarTransaksi.AsEnumerable();
        if (_currentUser is Siswa s)
        {
            // Siswa hanya melihat riwayat pinjaman miliknya sendiri
            list = list.Where(t => t.NamaPeminjam.Equals(s.Username, StringComparison.OrdinalIgnoreCase));
        }

        foreach (var t in list.Reverse())
        {
            bool isDipinjam = t.StatusPeminjaman.StartsWith("Dipinjam", StringComparison.OrdinalIgnoreCase);
            bool isTerlambat = false;
            string jatuhTempoStr = "-";

            if (DateTime.TryParse(t.TanggalPinjam, out DateTime tp))
            {
                DateTime jt = tp.AddDays(PerpustakaanService.BatasHariPinjam);
                jatuhTempoStr = jt.ToString("yyyy-MM-dd");
                if (isDipinjam && today > jt)
                {
                    isTerlambat = true;
                }
            }

            if (_filterSirkulasi == "DIPINJAM" && !isDipinjam) continue;
            if (_filterSirkulasi == "TERLAMBAT" && !isTerlambat) continue;
            if (_filterSirkulasi == "KEMBALI" && isDipinjam) continue;

            int rowIdx = _gridSirkulasi.Rows.Add(
                t.IdTransaksi,
                t.KodeBuku,
                t.JudulBuku,
                t.NamaPeminjam,
                t.TanggalPinjam,
                jatuhTempoStr,
                t.TanggalKembali,
                isTerlambat ? $"{t.StatusPeminjaman} (Terlambat!)" : t.StatusPeminjaman
            );

            var row = _gridSirkulasi.Rows[rowIdx];
            if (isTerlambat)
            {
                row.Cells["Status"].Style.ForeColor = Color.FromArgb(185, 28, 28);
                row.Cells["Status"].Style.BackColor = C_RED_BG;
            }
            else if (isDipinjam)
            {
                row.Cells["Status"].Style.ForeColor = Color.FromArgb(30, 64, 175);
                row.Cells["Status"].Style.BackColor = C_BLUE_LIGHT;
            }
            else
            {
                row.Cells["Status"].Style.ForeColor = C_GREEN_FG;
                row.Cells["Status"].Style.BackColor = C_GREEN_BG;
            }
        }
    }

    // =========================================================================
    // 8. VIEW: ANGGOTA (DIREKTORI SISWA)
    // =========================================================================
    private Panel BuatViewAnggota()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP, Padding = new Padding(0) };

        var lblHead = new Label
        {
            Text = "👥  Direktori Anggota & Status Kuota Peminjaman (Maks 3 Buku)",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Dock = DockStyle.Top,
            Height = 40
        };
        panel.Controls.Add(lblHead);

        var grid = BuatGridStandar();
        grid.Columns.Add("NIS", "NIS");
        grid.Columns.Add("Nama", "Nama Lengkap");
        grid.Columns.Add("Kelas", "Kelas");
        grid.Columns.Add("PinjamAktif", "Pinjaman Aktif");
        grid.Columns.Add("SisaKuota", "Sisa Kuota");
        grid.Columns.Add("Status", "Status Anggota");

        // Seed data anggota SMA Pelita Bangsa
        string[][] siswaData = {
            new[] { "20241001", "Dimas Saputra", "XII MIPA 1" },
            new[] { "20241002", "Rizky Pratama", "XII IPS 2" },
            new[] { "20241003", "Ahmad Fadlan", "XI MIPA 3" },
            new[] { "20241004", "Nadia Putri", "X-A" },
            new[] { "20241005", "Siti Nurhaliza", "XI IPS 1" },
            new[] { "20241006", "Budi Santoso", "XII MIPA 2" }
        };

        foreach (var s in siswaData)
        {
            int aktif = _service.HitungPinjamanAktifSiswa(s[1]);
            int sisa = Math.Max(0, PerpustakaanService.KuotaPinjamMaksimal - aktif);
            grid.Rows.Add(s[0], s[1], s[2], $"{aktif} buku", $"{sisa} buku", "Aktif");
        }

        panel.Controls.Add(grid);
        grid.BringToFront();

        return panel;
    }

    // =========================================================================
    // 9. VIEW: LAPORAN (REKAP SIRKULASI & DENDA)
    // =========================================================================
    private Panel BuatViewLaporan()
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = C_BG_APP, AutoScroll = true };

        var pnlHeaderCards = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 110,
            ColumnCount = 3,
            RowCount = 1
        };
        for (int i = 0; i < 3; i++) pnlHeaderCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
        panel.Controls.Add(pnlHeaderCards);

        Panel BuatLapCard(string title, string icon, Color cVal, out Label lblVal)
        {
            var p = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(6),
                Padding = new Padding(16, 14, 16, 14)
            };
            p.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, p.Width - 1, p.Height - 1);

            var lT = new Label
            {
                Text = $"{icon}  {title}",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = C_TEXT_MUTED,
                Dock = DockStyle.Top,
                Height = 24
            };
            lblVal = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 20f, FontStyle.Bold),
                ForeColor = cVal,
                Dock = DockStyle.Fill
            };
            p.Controls.Add(lblVal);
            p.Controls.Add(lT);
            return p;
        }

        pnlHeaderCards.Controls.Add(BuatLapCard("TOTAL BUKU TERDAFTAR", "📚", C_PRIMARY, out _lblLaporanTotalBuku), 0, 0);
        pnlHeaderCards.Controls.Add(BuatLapCard("TOTAL SIRKULASI SELESAI", "✓", Color.FromArgb(16, 185, 129), out _lblLaporanTotalTrx), 1, 0);
        pnlHeaderCards.Controls.Add(BuatLapCard("TOTAL DENDA KETERLAMBATAN", "💰", Color.FromArgb(220, 38, 38), out _lblLaporanTotalDenda), 2, 0);

        var pnlTableContainer = new Panel
        {
            Dock = DockStyle.Top,
            Height = 440,
            BackColor = Color.White,
            Margin = new Padding(6),
            Padding = new Padding(16)
        };
        pnlTableContainer.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, pnlTableContainer.Width - 1, pnlTableContainer.Height - 1);
        panel.Controls.Add(pnlTableContainer);
        pnlTableContainer.BringToFront();

        var lblTblTitle = new Label
        {
            Text = "📑  Catatan Sirkulasi Pengembalian dengan Denda Keterlambatan",
            Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
            ForeColor = C_TEXT_DARK,
            Dock = DockStyle.Top,
            Height = 36
        };
        pnlTableContainer.Controls.Add(lblTblTitle);

        _gridLaporanDenda = BuatGridStandar();
        _gridLaporanDenda.Columns.Add("ID", "ID Transaksi");
        _gridLaporanDenda.Columns.Add("Judul", "Buku");
        _gridLaporanDenda.Columns.Add("Peminjam", "Peminjam");
        _gridLaporanDenda.Columns.Add("TglPinjam", "Tgl Pinjam");
        _gridLaporanDenda.Columns.Add("TglKembali", "Tgl Kembali");
        _gridLaporanDenda.Columns.Add("Keterangan", "Keterangan Denda");
        pnlTableContainer.Controls.Add(_gridLaporanDenda);
        _gridLaporanDenda.BringToFront();

        return panel;
    }

    // =========================================================================
    // 10. REFRESH DATA & METRIK GLOBAL
    // =========================================================================
    private void RefreshSemuaData()
    {
        _service.MuatData();
        var (total, tersedia, dipinjam, terlambat, denda) = _service.AmbilStatistik();

        // 1. Refresh Kartu Metrik
        _lblStatTotalBuku.Text = $"{total:N0}";
        _lblStatBukuDipinjam.Text = $"{dipinjam:N0}";
        _lblStatBukuTerlambat.Text = $"{terlambat:N0}";
        _lblStatTotalDenda.Text = $"Rp {denda:N0}";

        _lblStripTotal.Text = $"Total: {total} buku";
        _lblStripTersedia.Text = $"Tersedia: {tersedia} eks";
        _lblStripDipinjam.Text = $"Dipinjam: {dipinjam} eks";

        _lblLaporanTotalBuku.Text = $"{total} buku";
        _lblLaporanTotalTrx.Text = $"{_service.DaftarTransaksi.Count} transaksi";
        _lblLaporanTotalDenda.Text = $"Rp {denda:N0}";

        // 2. Refresh Dasbor Tabel Pinjam Terbaru
        _gridPinjamTerbaru.Rows.Clear();
        foreach (var t in _service.DaftarTransaksi.TakeLast(6).Reverse())
        {
            _gridPinjamTerbaru.Rows.Add(t.NamaPeminjam, t.JudulBuku, t.TanggalPinjam, t.StatusPeminjaman);
        }

        // 3. Refresh Buku Populer
        _pnlBukuPopulerList.Controls.Clear();
        var popList = new[]
        {
            ("01", "Laskar Pelangi", "Andrea Hirata", "48 kali dipinjam"),
            ("02", "Bumi Manusia", "Pramoedya Ananta Toer", "42 kali dipinjam"),
            ("03", "Negeri 5 Menara", "Ahmad Fuadi", "36 kali dipinjam"),
            ("04", "Filosofi Teras", "Henry Manampiring", "29 kali dipinjam")
        };

        int py = 6;
        foreach (var item in popList)
        {
            var p = new Panel
            {
                Location = new Point(0, py),
                Size = new Size(_pnlBukuPopulerList.Width - 10, 64),
                BackColor = Color.FromArgb(248, 250, 252)
            };
            p.Paint += (s, e) => e.Graphics.DrawRectangle(new Pen(C_BORDER), 0, 0, p.Width - 1, p.Height - 1);

            var lRank = new Label
            {
                Text = item.Item1,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = C_PRIMARY,
                Location = new Point(12, 18),
                AutoSize = true
            };
            var lJ = new Label
            {
                Text = item.Item2,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = C_TEXT_DARK,
                Location = new Point(48, 12),
                AutoSize = true
            };
            var lP = new Label
            {
                Text = $"{item.Item3} • {item.Item4}",
                Font = new Font("Segoe UI", 8f),
                ForeColor = C_TEXT_MUTED,
                Location = new Point(48, 34),
                AutoSize = true
            };

            p.Controls.Add(lRank);
            p.Controls.Add(lJ);
            p.Controls.Add(lP);
            _pnlBukuPopulerList.Controls.Add(p);
            py += 72;
        }

        // 4. Filter Data Buku & Sirkulasi
        FilterDataBuku();
        LakukanPencarian();
        RefreshTabelSirkulasi();

        // 5. Laporan Denda
        _gridLaporanDenda.Rows.Clear();
        foreach (var t in _service.DaftarTransaksi.Where(x => x.StatusPeminjaman.Contains("Denda: Rp")))
        {
            _gridLaporanDenda.Rows.Add(t.IdTransaksi, t.JudulBuku, t.NamaPeminjam, t.TanggalPinjam, t.TanggalKembali, t.StatusPeminjaman);
        }
    }

    private static void ApplyRounded(Control control, int radius)
    {
        void UpdateRegion(object? sender, EventArgs e)
        {
            using var path = new GraphicsPath();
            var bounds = control.ClientRectangle;
            if (bounds.Width <= 0 || bounds.Height <= 0) return;
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(bounds.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(bounds.Width - radius, bounds.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, bounds.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }
        control.Resize += UpdateRegion;
        UpdateRegion(control, EventArgs.Empty);
    }

    // Helper Pembuat DataGridView Standar dengan Style Flat Modern
    private static DataGridView BuatGridStandar()
    {
        var grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            GridColor = Color.FromArgb(241, 245, 249),
            RowHeadersVisible = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowTemplate = { Height = 40 }
        };

        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = C_PRIMARY,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 10, 0),
            SelectionBackColor = C_PRIMARY,
            SelectionForeColor = Color.White
        };
        grid.ColumnHeadersHeight = 38;
        grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 250, 252) };
        grid.EnableHeadersVisualStyles = false;

        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Color.FromArgb(15, 23, 42),
            Font = new Font("Segoe UI", 9f),
            SelectionBackColor = Color.FromArgb(239, 246, 255),
            SelectionForeColor = Color.FromArgb(29, 59, 139),
            Padding = new Padding(8, 0, 8, 0)
        };

        return grid;
    }
}
