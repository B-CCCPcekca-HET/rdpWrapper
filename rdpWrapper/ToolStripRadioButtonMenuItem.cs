using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace rdpWrapper {

  internal sealed class ToolStripRadioButtonMenuItem : ToolStripMenuItem {

    public static bool DisplayAsCheckboxes { get; set; }

    public ToolStripRadioButtonMenuItem(string text, Image image, EventHandler onClick)
      : base(text, image, onClick) {
    }

    protected override void OnClick(EventArgs e) {
      foreach (var sibling in GetSiblings())
        if (sibling != this) sibling.Checked = false;
      Checked = true;
      base.OnClick(e);
    }

    private IEnumerable<ToolStripRadioButtonMenuItem> GetSiblings() {
      var items = OwnerItem is ToolStripDropDownItem parent ? parent.DropDownItems : Owner?.Items;
      if (items == null) yield break;
      foreach (ToolStripItem item in items)
        if (item is ToolStripRadioButtonMenuItem radio) yield return radio;
    }
  }
}
