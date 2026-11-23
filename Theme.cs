using System.Drawing;

namespace PerpustakaanPelitaBangsa;

public static class Theme
{
    public static class Colors
    {
        public static readonly Color Primary900 = ColorTranslator.FromHtml("#151515");
        public static readonly Color Primary800 = ColorTranslator.FromHtml("#2947D8");
        public static readonly Color Primary700 = ColorTranslator.FromHtml("#3D5AFE");
        public static readonly Color Primary600 = ColorTranslator.FromHtml("#3048D8");
        public static readonly Color Primary100 = ColorTranslator.FromHtml("#DDE3FF");
        public static readonly Color Primary50 = ColorTranslator.FromHtml("#EEF1FF");
        public static readonly Color Neutral0 = Color.White;
        public static readonly Color Neutral50 = ColorTranslator.FromHtml("#FFFBF0");
        public static readonly Color Neutral100 = ColorTranslator.FromHtml("#FFF6D1");
        public static readonly Color Neutral200 = ColorTranslator.FromHtml("#E8E1D2");
        public static readonly Color BorderStrong = ColorTranslator.FromHtml("#151515");
        public static readonly Color Neutral500 = ColorTranslator.FromHtml("#66645E");
        public static readonly Color Neutral600 = ColorTranslator.FromHtml("#4F4D48");
        public static readonly Color Neutral900 = ColorTranslator.FromHtml("#151515");
        public static readonly Color SuccessText = ColorTranslator.FromHtml("#151515");
        public static readonly Color SuccessFill = ColorTranslator.FromHtml("#55E5D0");
        public static readonly Color WarningText = ColorTranslator.FromHtml("#151515");
        public static readonly Color WarningFill = ColorTranslator.FromHtml("#FFE45C");
        public static readonly Color DangerText = ColorTranslator.FromHtml("#151515");
        public static readonly Color DangerFill = ColorTranslator.FromHtml("#FF6196");
        public static readonly Color Lavender = ColorTranslator.FromHtml("#C7B5FF");
        public static readonly Color Pink = ColorTranslator.FromHtml("#FF6196");
        public static readonly Color Teal = ColorTranslator.FromHtml("#55E5D0");
        public static readonly Color Yellow = ColorTranslator.FromHtml("#FFE45C");
        public static readonly Color Cream = ColorTranslator.FromHtml("#FFFBF0");
    }

    public static class Spacing
    {
        public const int S1 = 4, S2 = 8, S3 = 12, S4 = 16, S5 = 24, S6 = 32, S7 = 48;
    }

    public static class Radius
    {
        public const int Small = 8, Medium = 12, Large = 16, XLarge = 20;
    }

    public static class Sizes
    {
        public const int Sidebar = 264, Header = 80, Input = 40, LoginInput = 44;
        public const int Button = 40, LoginButton = 48, TableRow = 46, TableHeader = 42;
        public const int MinWindowWidth = 1024, MinWindowHeight = 640;
    }

    public static class Fonts
    {
        public static Font Caption => new("Segoe UI", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font Body => new("Segoe UI", 14f, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font Label => new("Segoe UI", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font H3 => new("Segoe UI", 20f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font H2 => new("Segoe UI", 26f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font H1 => new("Segoe UI", 34f, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font Icon => new("Segoe Fluent Icons", 20f, GraphicsUnit.Pixel);
    }
}
