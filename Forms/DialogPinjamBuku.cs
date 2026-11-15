using PerpustakaanPelitaBangsa.Models;
using PerpustakaanPelitaBangsa.Services;

namespace PerpustakaanPelitaBangsa.Forms;

/// <summary>
/// Dialog modal untuk Transaksi Peminjaman Buku.
/// </summary>
public class DialogPinjamBuku : Form
{
    private readonly ComboBox _cmbBuku = new();
    private readonly TextBox _txtPeminjam = new();
    private readonly Label _lblInfoBuku = new();
    private readonly Label _lblInfoAturan = new();
    private readonly Button _btnPinjam = new();
    private readonly Button _btnBatal = new();

    private readonly PerpustakaanService _service;
    private readonly List<Buku> _bukuTersedia;

    public string? SelectedKodeBuku { get; private set; }
    public string? NamaPeminjam { get; private set; }

    public DialogPinjamBuku(PerpustakaanService service, string? preselectedKode = null, string? preselectedSiswa = null)
    {
        _service = service;
        _bukuTersedia = service.DaftarBuku.Where(b => b.Tersedia).ToList();
        InitUI(preselectedKode, preselectedSiswa);
    }

    private void InitUI(string? preselectedKode, string? preselectedSiswa)
    {
        Text = "Formulir Peminjaman Buku";
        ClientSize = new Size(520, 460);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        AcceptButton = _btnPinjam;
        CancelButton = _btnBatal;
        BackColor = Theme.Colors.Neutral50;
        Font = Theme.Fonts.Body;

        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(24)
        };
        Controls.Add(panel);

        int y = 16;
        var lblTitle = new Label
        {
            Text = "Catat Peminjaman Sirkulasi",
            Font = new Font("Segoe UI", 13f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Location = new Point(24, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var lblBuku = new Label
        {
            Text = "Pilih Buku Koleksi (Tersedia)",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 85, 105),
            Location = new Point(24, y),
            AutoSize = true
        };
        panel.Controls.Add(lblBuku);
        y += 20;

        _cmbBuku.Location = new Point(24, y);
        _cmbBuku.Size = new Size(435, Theme.Sizes.Input);
        _cmbBuku.DropDownStyle = ComboBoxStyle.DropDownList;

        foreach (var b in _bukuTersedia)
        {
            _cmbBuku.Items.Add($"[{b.KodeBuku}] {b.Judul} - {b.Penulis}");
        }

        if (_bukuTersedia.Count > 0)
        {
            int defaultIdx = 0;
            if (!string.IsNullOrEmpty(preselectedKode))
            {
                int match = _bukuTersedia.FindIndex(b => b.KodeBuku.Equals(preselectedKode, StringComparison.OrdinalIgnoreCase));
                if (match >= 0) defaultIdx = match;
            }
            _cmbBuku.SelectedIndex = defaultIdx;
        }
        else
        {
            _cmbBuku.Items.Add("-- Tidak ada buku yang tersedia di rak --");
            _cmbBuku.SelectedIndex = 0;
            _cmbBuku.Enabled = false;
        }

        _cmbBuku.SelectedIndexChanged += (s, e) => UpdateInfoBuku();
        panel.Controls.Add(_cmbBuku);
        y += 36;

        _lblInfoBuku.Location = new Point(24, y);
        _lblInfoBuku.Size = new Size(435, 36);
        _lblInfoBuku.ForeColor = Color.FromArgb(100, 116, 139);
        _lblInfoBuku.Font = new Font("Segoe UI", 8.5f, FontStyle.Italic);
        panel.Controls.Add(_lblInfoBuku);
        UpdateInfoBuku();
        y += 42;

        var lblPeminjam = new Label
        {
            Text = "Nama Lengkap Siswa / Peminjam",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(71, 85, 105),
            Location = new Point(24, y),
            AutoSize = true
        };
        panel.Controls.Add(lblPeminjam);
        y += 20;

        _txtPeminjam.Location = new Point(24, y);
        _txtPeminjam.Size = new Size(435, Theme.Sizes.Input);
        _txtPeminjam.BorderStyle = BorderStyle.FixedSingle;
        _txtPeminjam.PlaceholderText = "Masukkan nama siswa/peminjam";
        if (!string.IsNullOrWhiteSpace(preselectedSiswa))
        {
            _txtPeminjam.Text = preselectedSiswa;
        }
        panel.Controls.Add(_txtPeminjam);
        y += 38;

        _lblInfoAturan.Location = new Point(24, y);
        _lblInfoAturan.Size = new Size(435, 60);
        _lblInfoAturan.ForeColor = Theme.Colors.WarningText;
        _lblInfoAturan.BackColor = Theme.Colors.WarningFill;
        _lblInfoAturan.Padding = new Padding(Theme.Spacing.S2);
        _lblInfoAturan.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
        _lblInfoAturan.AutoEllipsis = false;
        _lblInfoAturan.Text = "Ketentuan: Maksimal 3 buku aktif per siswa. Batas masa pinjam 7 hari. Keterlambatan dikenakan denda Rp 1.000 / hari.";
        panel.Controls.Add(_lblInfoAturan);
        y += 70;

        _btnPinjam.Text = "Proses Pinjam";
        _btnPinjam.Location = new Point(235, y);
        _btnPinjam.Size = new Size(130, 36);
        _btnPinjam.BackColor = Color.FromArgb(29, 59, 139);
        _btnPinjam.ForeColor = Color.White;
        _btnPinjam.FlatStyle = FlatStyle.Flat;
        _btnPinjam.FlatAppearance.BorderSize = 0;
        _btnPinjam.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        _btnPinjam.Enabled = _bukuTersedia.Count > 0;
        _btnPinjam.Click += BtnPinjam_Click;
        panel.Controls.Add(_btnPinjam);

        _btnBatal.Text = "Batal";
        _btnBatal.Location = new Point(375, y);
        _btnBatal.Size = new Size(84, 36);
        _btnBatal.BackColor = Color.FromArgb(226, 232, 240);
        _btnBatal.ForeColor = Color.FromArgb(51, 65, 85);
        _btnBatal.FlatStyle = FlatStyle.Flat;
        _btnBatal.FlatAppearance.BorderSize = 0;
        _btnBatal.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        panel.Controls.Add(_btnBatal);
        _txtPeminjam.Focus();
    }

    private void UpdateInfoBuku()
    {
        if (_cmbBuku.SelectedIndex >= 0 && _cmbBuku.SelectedIndex < _bukuTersedia.Count)
        {
            var b = _bukuTersedia[_cmbBuku.SelectedIndex];
            _lblInfoBuku.Text = $"Lokasi: {b.LokasiRak} | Kategori: {b.Kategori} | Tahun: {b.Tahun}";
        }
        else
        {
            _lblInfoBuku.Text = string.Empty;
        }
    }

    private void BtnPinjam_Click(object? sender, EventArgs e)
    {
        if (_cmbBuku.SelectedIndex < 0 || _cmbBuku.SelectedIndex >= _bukuTersedia.Count)
        {
            MessageBox.Show("Pilih buku yang ingin dipinjam.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        string peminjam = _txtPeminjam.Text.Trim();
        if (string.IsNullOrWhiteSpace(peminjam))
        {
            MessageBox.Show("Nama siswa peminjam tidak boleh kosong.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var b = _bukuTersedia[_cmbBuku.SelectedIndex];
        var res = _service.PinjamBuku(b.KodeBuku, peminjam);
        if (!res.Sukses)
        {
            MessageBox.Show(res.Pesan, "Gagal Meminjam", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SelectedKodeBuku = b.KodeBuku;
        NamaPeminjam = peminjam;
        MessageBox.Show(res.Pesan, "Peminjaman Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
        Close();
    }
}
