using System.Drawing.Drawing2D;
using PerpustakaanPelitaBangsa.Models;

namespace PerpustakaanPelitaBangsa.Helpers;

/// <summary>
/// Utility untuk rendering grafis UI: sampul buku dinamis, palet warna, dan chart visual.
/// </summary>
public static class UiHelper
{
    public static Color GetBookColor(string? judul)
    {
        if (string.IsNullOrWhiteSpace(judul)) return Color.FromArgb(29, 59, 139);
        string s = judul.ToLowerInvariant();

        if (s.Contains("laskar")) return Color.FromArgb(31, 95, 97);      // Teal
        if (s.Contains("bumi")) return Color.FromArgb(168, 90, 59);       // Terracotta
        if (s.Contains("menara")) return Color.FromArgb(181, 141, 61);    // Ochre
        if (s.Contains("filosofi")) return Color.FromArgb(36, 58, 94);    // Slate Navy
        if (s.Contains("hujan")) return Color.FromArgb(43, 88, 89);       // Dark Teal
        if (s.Contains("pulang")) return Color.FromArgb(74, 35, 40);      // Plum
        if (s.Contains("java") || s.Contains("algoritma")) return Color.FromArgb(30, 64, 175);
        if (s.Contains("fisika")) return Color.FromArgb(14, 116, 144);
        if (s.Contains("sejarah")) return Color.FromArgb(120, 53, 15);

        int hash = Math.Abs(judul.GetHashCode());
        Color[] palet =
        {
            Color.FromArgb(30, 58, 138),
            Color.FromArgb(15, 118, 110),
            Color.FromArgb(180, 83, 9),
            Color.FromArgb(109, 40, 217),
            Color.FromArgb(71, 85, 105)
        };
        return palet[hash % palet.Length];
    }

    public static Bitmap CreateBookCover(Buku? b, int w, int h)
    {
        var bmp = new Bitmap(w, h);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

        Color baseColor = GetBookColor(b?.Judul);
        Color darkerColor = ControlPaint.Dark(baseColor, 0.2f);

        // Background Gradient
        using (var brush = new LinearGradientBrush(new Rectangle(0, 0, w, h), baseColor, darkerColor, 45f))
        {
            using var path = CreateRoundedRectanglePath(new Rectangle(0, 0, w, h), 8);
            g.FillPath(brush, path);
        }

        // Spine highlight
        using (var spineBrush = new SolidBrush(Color.FromArgb(50, Color.White)))
        {
            g.FillRectangle(spineBrush, 6, 0, 5, h);
        }

        // Bookmark ribbon gold
        using (var ribbonBrush = new SolidBrush(Color.FromArgb(245, 158, 11)))
        {
            g.FillRectangle(ribbonBrush, 14, h - 22, 34, 4);
        }

        // Author Name
        string author = b?.Penulis?.ToUpperInvariant() ?? "PENULIS";
        if (author.Length > 16) author = string.Concat(author.AsSpan(0, 14), "..");
        using (var fAuthor = new Font("Segoe UI", 7.5f, FontStyle.Bold))
        using (var brAuthor = new SolidBrush(Color.FromArgb(224, 231, 255)))
        {
            g.DrawString(author, fAuthor, brAuthor, 16, 18);
        }

        // Book Title (multi-line)
        string title = b?.Judul ?? "Judul Buku";
        string[] words = title.Split(' ');
        using (var fTitle = new Font("Segoe UI", 10.5f, FontStyle.Bold))
        using (var brTitle = new SolidBrush(Color.White))
        {
            float y = 44;
            int count = Math.Min(3, words.Length);
            for (int i = 0; i < count; i++)
            {
                g.DrawString(words[i], fTitle, brTitle, 16, y);
                y += 18;
            }
        }

        return bmp;
    }

    public static Bitmap CreateThumbnail(Buku? b, int w = 36, int h = 48)
    {
        var bmp = new Bitmap(w, h);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Color baseColor = GetBookColor(b?.Judul);
        using (var brush = new SolidBrush(baseColor))
        {
            using var path = CreateRoundedRectanglePath(new Rectangle(0, 0, w, h), 4);
            g.FillPath(brush, path);
        }

        // Spine
        using (var spine = new SolidBrush(Color.FromArgb(50, Color.White)))
        {
            g.FillRectangle(spine, 3, 0, 3, h);
        }

        // Ribbon
        using (var rib = new SolidBrush(Color.FromArgb(245, 158, 11)))
        {
            g.FillRectangle(rib, 6, h - 8, w - 12, 2);
        }

        return bmp;
    }

    public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
    {
        var path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}
