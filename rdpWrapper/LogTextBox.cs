using System;
using System.Drawing;
using System.Windows.Forms;

namespace rdpWrapper {

  internal sealed class LogTextBox : RichTextBox {

    public LogTextBox() {
      ReadOnly = true;
      BorderStyle = BorderStyle.None;
      HideSelection = false;
    }

    public void AppendLine(string text, Color color, bool newLine = true) {
      if (newLine && TextLength > 0)
        AppendText(Environment.NewLine);
      SelectionStart = TextLength;
      SelectionLength = 0;
      SelectionColor = color;
      AppendText(text);
    }
  }
}
