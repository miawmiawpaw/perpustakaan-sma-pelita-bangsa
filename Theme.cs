using System.Drawing;

namespace PerpustakaanPelitaBangsa;

public static class Theme
{
    public static class Colors
    {
        public static readonly Color Primary900 = ColorTranslator.FromHtml("#0F2A5C");
        public static readonly Color Primary700 = ColorTranslator.FromHtml("#1E3A8A");
        public static readonly Color Primary800 = ColorTranslator.FromHtml("#1E40AF");
        public static readonly Color Primary600 = ColorTranslator.FromHtml("#1D4ED8");
        public static readonly Color Primary100 = ColorTranslator.FromHtml("#E0E7FF");
        public static readonly Color Primary50 = ColorTranslator.FromHtml("#EFF6FF");
        public static readonly Color Neutral0 = Color.White;
        public static readonly Color Neutral50 = ColorTranslator.FromHtml("#F8FAFC");
        public static readonly Color Neutral100 = ColorTranslator.FromHtml("#F1F5F9");
        public static readonly Color Neutral200 = ColorTranslator.FromHtml("#E2E8F0");
        public static readonly Color BorderStrong = ColorTranslator.FromHtml("#7C8BA1");
        public static readonly Color Neutral500 = ColorTranslator.FromHtml("#64748B");
        public static readonly Color Neutral600 = ColorTranslator.FromHtml("#475569");
        public static readonly Color Neutral900 = ColorTranslator.FromHtml("#0F172A");
        public static readonly Color SuccessText = ColorTranslator.FromHtml("#047857");
        public static readonly Color SuccessFill = ColorTranslator.FromHtml("#D1FAE5");
        public static readonly Color WarningText = ColorTranslator.FromHtml("#B45309");
        public static readonly Color WarningFill = ColorTranslator.FromHtml("#FEF3C7");
        public static readonly Color DangerText = ColorTranslator.FromHtml("#B91C1C");
        public static readonly Color DangerFill = ColorTranslator.FromHtml("#FEE2E2");
    }

    public static class Spacing
    {
        public const int S1 = 4, S2 = 8, S3 = 12, S4 = 16, S5 = 24, S6 = 32, S7 = 48;
    }

    public static class Radius
    {
        public const int Small = 4, Medium = 8, Large = 12, XLarge = 16;
    }

    public static class Sizes
    {
        public const int Sidebar = 240, Header = 72, Input = 36, LoginInput = 40;
        public const int Button = 36, LoginButton = 44, TableRow = 44, TableHeader = 40;
        public const int MinWindowWidth = 1024, MinWindowHeight = 640;
    }

    public static class Fonts
    {
        public static Font Caption => new("Segoe UI", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font Body => new("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font Label => new("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font H3 => new("Segoe UI", 20f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font H2 => new("Segoe UI", 24f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font H1 => new("Segoe UI", 32f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font Icon => new("Segoe Fluent Icons", 20f, GraphicsUnit.Pixel);
    }
}
