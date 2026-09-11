using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;

namespace rdpWrapper {

  internal sealed class Theme {

    public string Id { get; set; }
    public Color BackgroundColor { get; set; }
    public Color ForegroundColor { get; set; }
    public Color ControlColor { get; set; }
    public Color InfoColor { get; set; }
    public Color MessageColor { get; set; }
    public Color WarnColor { get; set; }

    public static readonly Theme Light = new() {
      Id = "light",
      BackgroundColor = Color.White,
      ForegroundColor = Color.Black,
      ControlColor = SystemColors.Control,
      InfoColor = Color.SteelBlue,
      MessageColor = Color.DarkGreen,
      WarnColor = Color.Firebrick,
    };

    public static readonly Theme Dark = new() {
      Id = "dark",
      BackgroundColor = Color.FromArgb(32, 32, 32),
      ForegroundColor = Color.Gainsboro,
      ControlColor = Color.FromArgb(45, 45, 48),
      InfoColor = Color.DeepSkyBlue,
      MessageColor = Color.LightGreen,
      WarnColor = Color.OrangeRed,
    };

    public static bool IsAutoThemeEnabled;
    public static Theme Current = Light;

    public static bool SystemUsesDarkMode() {
      try {
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
        return key?.GetValue("AppsUseLightTheme") is int v && v == 0;
      }
      catch {
        return false;
      }
    }

    public void Apply(Control root) {
      ApplyRecursive(root);
    }

    private void ApplyRecursive(Control control) {
      if (control is not MenuStrip and not ToolStrip) {
        control.BackColor = control is Form or GroupBox or Panel ? BackgroundColor : ControlColor;
        control.ForeColor = ForegroundColor;
      }
      foreach (Control child in control.Controls)
        ApplyRecursive(child);
    }
  }
}
