using PerpustakaanPelitaBangsa.Controls;

namespace PerpustakaanPelitaBangsa.Forms;

public sealed class UiGallery : Form
{
    public UiGallery()
    {
        Text = "UI Gallery";
        MinimumSize = new Size(Theme.Sizes.MinWindowWidth, Theme.Sizes.MinWindowHeight);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Theme.Colors.Neutral50;
        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(Theme.Spacing.S5),
            AutoScroll = true,
            WrapContents = false,
            FlowDirection = FlowDirection.TopDown
        };
        Controls.Add(flow);

        var title = new Label { Text = "Shared UI controls", AutoSize = true, Font = Theme.Fonts.H2, ForeColor = Theme.Colors.Neutral900 };
        flow.Controls.Add(title);
        flow.Controls.Add(new RoundedButton { Text = "Primary", Width = 180, Variant = ButtonVariant.Primary });
        flow.Controls.Add(new RoundedButton { Text = "Secondary", Width = 180, Variant = ButtonVariant.Secondary, ForeColor = Theme.Colors.Neutral900 });
        flow.Controls.Add(new RoundedButton { Text = "Danger", Width = 180, Variant = ButtonVariant.Danger, ForeColor = Theme.Colors.DangerText });
        flow.Controls.Add(new RoundedTextBox { Width = 360, PlaceholderText = "Masukkan teks" });
        var badges = new FlowLayoutPanel { AutoSize = true, WrapContents = false };
        var disabled = new RoundedButton { Text = "Disabled", Width = 180, Enabled = false };
        flow.Controls.Add(disabled);
        foreach (var status in new[] { "Tersedia", "Dipinjam", "Terlambat", "Dikembalikan" })
        {
            var badge = new StatusBadge { Width = 120 };
            badge.SetStatus(status);
            badges.Controls.Add(badge);
        }
        flow.Controls.Add(badges);
        flow.Controls.Add(new StatCard("TOTAL KOLEKSI", "9") { Width = 360, Height = 112 });
        flow.Controls.Add(new EmptyState("Belum ada data", "Data akan tampil setelah transaksi dibuat.") { Width = 360, Height = 96 });
        flow.Controls.Add(new Toast("Buku berhasil disimpan") { Width = 360 });
    }
}
