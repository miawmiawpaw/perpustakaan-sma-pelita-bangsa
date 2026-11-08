using System.Drawing.Drawing2D;
using PerpustakaanPelitaBangsa.Helpers;

namespace PerpustakaanPelitaBangsa.Controls;

/// <summary>
/// Kontrol kustom visualisasi grafik batang tren sirkulasi peminjaman bulanan.
/// </summary>
public class DasborBarChart : Control
{
    private readonly string[] _bulan = { "Mei", "Jun", "Jul", "Agu", "Sep", "Okt" };
    private readonly int[] _values = { 248, 295, 210, 364, 328, 396 };
    private const int MaxVal = 420;

    public DasborBarChart()
    {
        DoubleBuffered = true;
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        int w = Width;
        int h = Height - 28;
        int n = _bulan.Length;
        int barW = Math.Min(38, Math.Max(20, (w / n) - 24));

        // Garis Grid Horizontal
        using (var penGrid = new Pen(Color.FromArgb(241, 245, 249), 1))
        {
            g.DrawLine(penGrid, 20, h, w - 20, h);
            g.DrawLine(penGrid, 20, h / 2, w - 20, h / 2);
        }

        using var fontVal = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        using var fontMonth = new Font("Segoe UI", 8f, FontStyle.Regular);
        using var brushMuted = new SolidBrush(Color.FromArgb(100, 116, 139));
        using var brushDark = new SolidBrush(Color.FromArgb(15, 23, 42));

        for (int i = 0; i < n; i++)
        {
            int slotX = 20 + i * ((w - 40) / n);
            int bx = slotX + (((w - 40) / n - barW) / 2);
            int barH = (int)(((double)_values[i] / MaxVal) * (h - 26));
            int by = h - barH;

            // Bar bulat
            var barRect = new Rectangle(bx, by, barW, barH);
            bool isCurrent = (i == n - 1);

            Color barColor = isCurrent ? Color.FromArgb(29, 59, 139) : Color.FromArgb(191, 219, 254);
            using (var brushBar = new SolidBrush(barColor))
            {
                using var path = UiHelper.CreateRoundedRectanglePath(barRect, 4);
                g.FillPath(brushBar, path);
            }

            // Nilai angka di atas bar
            string sv = _values[i].ToString();
            var sizeVal = g.MeasureString(sv, fontVal);
            g.DrawString(sv, fontVal, isCurrent ? brushDark : brushMuted, bx + (barW - sizeVal.Width) / 2, by - 16);

            // Label Bulan di bawah bar
            string bm = _bulan[i];
            var sizeBm = g.MeasureString(bm, fontMonth);
            g.DrawString(bm, fontMonth, brushMuted, bx + (barW - sizeBm.Width) / 2, h + 8);
        }
    }
}
