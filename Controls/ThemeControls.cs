using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace PerpustakaanPelitaBangsa.Controls;

public enum ButtonVariant { Primary, Secondary, Danger, Ghost }

public class RoundedPanel : Panel
{
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CornerRadius { get; set; } = Theme.Radius.Large;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color BorderColor { get; set; } = Theme.Colors.Neutral200;

    public RoundedPanel()
    {
        BackColor = Theme.Colors.Neutral0;
        Resize += (_, _) => UpdateRegion();
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = CreatePath(ClientRectangle, CornerRadius);
        using var pen = new Pen(BorderColor);
        e.Graphics.DrawPath(pen, path);
        base.OnPaint(e);
    }

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        using var path = CreatePath(ClientRectangle, CornerRadius);
        Region = new Region(path);
        Invalidate();
    }

    internal static GraphicsPath CreatePath(Rectangle bounds, int radius)
    {
        int d = Math.Min(radius * 2, Math.Min(bounds.Width, bounds.Height));
        var path = new GraphicsPath();
        path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Top, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.Left, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }
}

public class RoundedButton : Button
{
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ButtonVariant Variant { get; set; } = ButtonVariant.Primary;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CornerRadius { get; set; } = Theme.Radius.Medium;
    private bool _hover;

    public RoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        Font = Theme.Fonts.Label;
        Height = Theme.Sizes.Button;
        Cursor = Cursors.Hand;
        MouseEnter += (_, _) => { _hover = true; Invalidate(); };
        MouseLeave += (_, _) => { _hover = false; Invalidate(); };
        Resize += (_, _) => UpdateRegion();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var bounds = ClientRectangle;
        bounds.Width--;
        bounds.Height--;
        using var path = RoundedPanel.CreatePath(bounds, CornerRadius);
        using var fill = new SolidBrush(BackColorForState());
        using var border = new Pen(BorderColorForState());
        e.Graphics.FillPath(fill, path);
        e.Graphics.DrawPath(border, path);
        Color textColor = Variant switch
        {
            ButtonVariant.Danger => Theme.Colors.DangerText,
            ButtonVariant.Secondary or ButtonVariant.Ghost => Theme.Colors.Neutral900,
            _ => Color.White
        };
        TextRenderer.DrawText(e.Graphics, Text, Font, bounds, Enabled ? textColor : Theme.Colors.Neutral500, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }
    private Color BackColorForState() => Enabled ? (_hover ? Theme.Colors.Primary800 : Variant switch
    {
        ButtonVariant.Secondary => Theme.Colors.Neutral100,
        ButtonVariant.Danger => Theme.Colors.Neutral0,
        ButtonVariant.Ghost => Theme.Colors.Neutral0,
        _ => Theme.Colors.Primary700
    }) : Theme.Colors.Neutral200;
    private Color BorderColorForState() => Variant == ButtonVariant.Danger ? Theme.Colors.DangerText : Variant == ButtonVariant.Ghost ? Theme.Colors.Neutral200 : BackColorForState();
    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        using var path = RoundedPanel.CreatePath(ClientRectangle, CornerRadius);
        Region = new Region(path);
    }
}

public class RoundedTextBox : UserControl
{
    private readonly TextBox _input = new() { BorderStyle = BorderStyle.None, Dock = DockStyle.Fill };
    public TextBox Input => _input;
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PlaceholderText { get => _input.PlaceholderText; set => _input.PlaceholderText = value; }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool HasError { get; set; }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new string Text { get => _input.Text; set => _input.Text = value; }
    public new event KeyEventHandler? KeyDown
    {
        add => _input.KeyDown += value;
        remove => _input.KeyDown -= value;
    }

    public RoundedTextBox()
    {
        Height = Theme.Sizes.Input;
        Padding = new Padding(Theme.Spacing.S2, Theme.Spacing.S2, Theme.Spacing.S2, Theme.Spacing.S2);
        BackColor = Theme.Colors.Neutral0;
        _input.Font = Theme.Fonts.Body;
        _input.ForeColor = Theme.Colors.Neutral900;
        _input.BackColor = Theme.Colors.Neutral0;
        Controls.Add(_input);
        _input.Enter += (_, _) => Invalidate();
        _input.Leave += (_, _) => Invalidate();
        Resize += (_, _) => UpdateRegion();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var path = RoundedPanel.CreatePath(new Rectangle(0, 0, Width - 1, Height - 1), Theme.Radius.Medium);
        using var pen = new Pen(HasError ? Theme.Colors.DangerText : _input.Focused ? Theme.Colors.Primary600 : Theme.Colors.BorderStrong, _input.Focused || HasError ? 2 : 1);
        e.Graphics.DrawPath(pen, path);
        base.OnPaint(e);
    }

    private void UpdateRegion()
    {
        if (Width <= 0 || Height <= 0) return;
        using var path = RoundedPanel.CreatePath(ClientRectangle, Theme.Radius.Medium);
        Region = new Region(path);
        Invalidate();
    }
}

public class StatusBadge : Label
{
    public StatusBadge()
    {
        AutoSize = false;
        Height = 24;
        Padding = new Padding(Theme.Spacing.S2, 0, Theme.Spacing.S2, 0);
        Font = Theme.Fonts.Caption;
        TextAlign = ContentAlignment.MiddleCenter;
    }

    public void SetStatus(string status)
    {
        Text = status;
        (BackColor, ForeColor) = status.Contains("Terlambat", StringComparison.OrdinalIgnoreCase)
            ? (Theme.Colors.DangerFill, Theme.Colors.DangerText)
            : status.Contains("Dipinjam", StringComparison.OrdinalIgnoreCase)
                ? (Theme.Colors.WarningFill, Theme.Colors.WarningText)
                : (Theme.Colors.SuccessFill, Theme.Colors.SuccessText);
    }
}

public class SidebarButton : RoundedButton
{
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool Active { get; set; }
    public SidebarButton() { Variant = ButtonVariant.Ghost; TextAlign = ContentAlignment.MiddleLeft; Width = 216; }
    protected override void OnPaint(PaintEventArgs e)
    {
        if (Active) BackColor = Theme.Colors.Primary50;
        ForeColor = Active ? Theme.Colors.Primary700 : Theme.Colors.Neutral900;
        base.OnPaint(e);
    }
}

public class StatCard : RoundedPanel
{
    public StatCard(string title, string value)
    {
        Padding = new Padding(Theme.Spacing.S5);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, Theme.Spacing.S6));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(new Label { Text = title, Dock = DockStyle.Fill, Font = Theme.Fonts.Caption, ForeColor = Theme.Colors.Neutral600 }, 0, 0);
        layout.Controls.Add(new Label { Text = value, Dock = DockStyle.Fill, Font = Theme.Fonts.H1, ForeColor = Theme.Colors.Primary700 }, 0, 1);
        Controls.Add(layout);
    }
}

public class EmptyState : RoundedPanel
{
    public EmptyState(string title, string helper)
    {
        var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = false, Padding = new Padding(Theme.Spacing.S5) };
        flow.Controls.Add(new Label { Text = title, AutoSize = true, Font = Theme.Fonts.Label, ForeColor = Theme.Colors.Neutral900 });
        flow.Controls.Add(new Label { Text = helper, AutoSize = true, Font = Theme.Fonts.Caption, ForeColor = Theme.Colors.Neutral500 });
        Controls.Add(flow);
    }
}

public class Toast : RoundedPanel
{
    public Toast(string message)
    {
        Height = Theme.Spacing.S7;
        Padding = new Padding(Theme.Spacing.S4);
        Controls.Add(new Label { Text = message, Dock = DockStyle.Fill, Font = Theme.Fonts.Body, ForeColor = Theme.Colors.Neutral900 });
    }
}
