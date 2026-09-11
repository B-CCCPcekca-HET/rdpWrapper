using System.Drawing;
using System.Windows.Forms;

namespace rdpWrapper {

  internal sealed class ThemedToolStripRenderer : ToolStripProfessionalRenderer {

    public ThemedToolStripRenderer() : base(new ThemedColorTable()) {
    }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e) {
      e.TextColor = Theme.Current.ForegroundColor;
      base.OnRenderItemText(e);
    }

    private sealed class ThemedColorTable : ProfessionalColorTable {
      public override Color MenuItemSelected => Theme.Current.ControlColor;
      public override Color MenuItemSelectedGradientBegin => Theme.Current.ControlColor;
      public override Color MenuItemSelectedGradientEnd => Theme.Current.ControlColor;
      public override Color MenuItemBorder => Theme.Current.ForegroundColor;
      public override Color MenuItemPressedGradientBegin => Theme.Current.ControlColor;
      public override Color MenuItemPressedGradientEnd => Theme.Current.ControlColor;
      public override Color ToolStripDropDownBackground => Theme.Current.BackgroundColor;
      public override Color ImageMarginGradientBegin => Theme.Current.BackgroundColor;
      public override Color ImageMarginGradientMiddle => Theme.Current.BackgroundColor;
      public override Color ImageMarginGradientEnd => Theme.Current.BackgroundColor;
      public override Color MenuBorder => Theme.Current.ForegroundColor;
      public override Color MenuStripGradientBegin => Theme.Current.BackgroundColor;
      public override Color MenuStripGradientEnd => Theme.Current.BackgroundColor;
      public override Color SeparatorDark => Theme.Current.ForegroundColor;
      public override Color SeparatorLight => Theme.Current.ForegroundColor;
    }
  }
}
