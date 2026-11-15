using PerpustakaanPelitaBangsa.Models;

namespace PerpustakaanPelitaBangsa.Forms;

/// <summary>
/// Dialog modal untuk Tambah dan Edit Buku.
/// </summary>
public class DialogTambahBuku : Form
{
    private readonly TextBox _txtKode = new();
    private readonly TextBox _txtJudul = new();
    private readonly TextBox _txtPenulis = new();
    private readonly TextBox _txtLokasi = new();
    private readonly ComboBox _cmbKategori = new();
    private readonly TextBox _txtTahun = new();
    private readonly Button _btnSimpan = new();
    private readonly Button _btnBatal = new();
    private readonly ErrorProvider _errors = new();

    public Buku? HasilBuku { get; private set; }
    private readonly bool _isEdit;

    public DialogTambahBuku(Buku? existing = null)
    {
        _isEdit = existing != null;
        InitUI(existing);
    }

    private void InitUI(Buku? existing)
    {
        Text = _isEdit ? "Edit Informasi Buku" : "Tambah Koleksi Buku Baru";
        ClientSize = new Size(520, 720);
        MinimumSize = new Size(520, 720);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        AutoScroll = true;
        AcceptButton = _btnSimpan;
        CancelButton = _btnBatal;
        BackColor = Theme.Colors.Neutral50;
        Font = Theme.Fonts.Body;

        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(28),
            AutoScroll = true
        };
        Controls.Add(panel);

        int y = 16;
        var lblTitle = new Label
        {
            Text = _isEdit ? "Perbarui Data Koleksi" : "Form Tambah Buku Baru",
            Font = Theme.Fonts.H3,
            ForeColor = Theme.Colors.Neutral900,
            Location = new Point(24, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        void AddField(string labelText, Control ctrl, bool readOnly = false)
        {
            var lbl = new Label
            {
                Text = labelText,
                Font = Theme.Fonts.Label,
                ForeColor = Theme.Colors.Neutral600,
                Location = new Point(24, y),
                AutoSize = true
            };
            panel.Controls.Add(lbl);
            y += Theme.Spacing.S4;

            ctrl.Location = new Point(24, y);
            ctrl.Size = new Size(Math.Max(260, panel.ClientSize.Width - 48), Theme.Sizes.Input);
            if (ctrl is TextBox tb)
            {
                tb.ReadOnly = readOnly;
                tb.BorderStyle = BorderStyle.FixedSingle;
            }
            panel.Controls.Add(ctrl);
            y += Theme.Spacing.S6;
        }

        _cmbKategori.DropDownStyle = ComboBoxStyle.DropDownList;
        _cmbKategori.Items.AddRange(new object[] { "Novel", "Pendidikan", "Sains", "Psikologi", "Sejarah", "Teknologi", "Umum" });
        _cmbKategori.SelectedIndex = 0;

        AddField("Kode Buku (Contoh: BK-010)", _txtKode, readOnly: _isEdit);
        AddField("Judul Buku", _txtJudul);
        AddField("Nama Penulis", _txtPenulis);
        AddField("Lokasi Rak (Contoh: Rak B-12, Lantai 1)", _txtLokasi);
        AddField("Kategori", _cmbKategori);
        AddField("Tahun Terbit", _txtTahun);

        if (existing != null)
        {
            _txtKode.Text = existing.KodeBuku;
            _txtJudul.Text = existing.Judul;
            _txtPenulis.Text = existing.Penulis;
            _txtLokasi.Text = existing.LokasiRak;
            _txtTahun.Text = existing.Tahun;
            int idx = _cmbKategori.Items.IndexOf(existing.Kategori);
            _cmbKategori.SelectedIndex = idx >= 0 ? idx : 0;
        }

        y += 10;
        _btnSimpan.Text = _isEdit ? "Simpan Perubahan" : "Simpan Buku";
        _btnSimpan.Location = new Point(Math.Max(24, panel.ClientSize.Width - 220), y);
        _btnSimpan.Size = new Size(130, 36);
        _btnSimpan.BackColor = Color.FromArgb(29, 59, 139);
        _btnSimpan.ForeColor = Color.White;
        _btnSimpan.FlatStyle = FlatStyle.Flat;
        _btnSimpan.FlatAppearance.BorderSize = 0;
        _btnSimpan.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        _btnSimpan.Click += BtnSimpan_Click;
        panel.Controls.Add(_btnSimpan);

        _btnBatal.Text = "Batal";
        _btnBatal.Location = new Point(Math.Max(24, panel.ClientSize.Width - 80), y);
        _btnBatal.Size = new Size(80, 36);
        _btnBatal.BackColor = Color.FromArgb(226, 232, 240);
        _btnBatal.ForeColor = Color.FromArgb(51, 65, 85);
        _btnBatal.FlatStyle = FlatStyle.Flat;
        _btnBatal.FlatAppearance.BorderSize = 0;
        _btnBatal.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
        panel.Controls.Add(_btnBatal);
        panel.Resize += (s, e) =>
        {
            int width = Math.Max(260, panel.ClientSize.Width - 48);
            foreach (Control c in new Control[] { _txtKode, _txtJudul, _txtPenulis, _txtLokasi, _cmbKategori, _txtTahun })
                c.Width = width;
            _btnSimpan.Left = Math.Max(24, panel.ClientSize.Width - 220);
            _btnBatal.Left = Math.Max(24, panel.ClientSize.Width - 80);
        };
    }

    private void BtnSimpan_Click(object? sender, EventArgs e)
    {
        string kode = _txtKode.Text.Trim();
        string judul = _txtJudul.Text.Trim();
        string penulis = _txtPenulis.Text.Trim();
        string lokasi = _txtLokasi.Text.Trim();
        string kategori = _cmbKategori.SelectedItem?.ToString() ?? "Umum";
        string tahun = _txtTahun.Text.Trim();

        _errors.Clear();
        var required = new (Control Control, string Value, string Message)[]
        {
            (_txtKode, kode, "Kode buku wajib diisi."),
            (_txtJudul, judul, "Judul buku wajib diisi."),
            (_txtPenulis, penulis, "Nama penulis wajib diisi."),
            (_txtLokasi, lokasi, "Lokasi rak wajib diisi.")
        };
        foreach (var item in required)
        {
            if (string.IsNullOrWhiteSpace(item.Value))
            {
                _errors.SetError(item.Control, item.Message);
                item.Control.Focus();
                return;
            }
        }

        if (!string.IsNullOrWhiteSpace(tahun) && (!int.TryParse(tahun, out int year) || year is < 1900 or > 2100))
        {
            _errors.SetError(_txtTahun, "Tahun harus berupa 4 digit antara 1900 dan 2100.");
            _txtTahun.Focus();
            return;
        }

        if (string.IsNullOrWhiteSpace(tahun)) tahun = DateTime.Now.Year.ToString();
        HasilBuku = new Buku(kode, judul, penulis, lokasi, true, kategori, tahun);
        DialogResult = DialogResult.OK;
        Close();
    }
}
