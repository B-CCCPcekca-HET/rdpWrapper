using System;
using System.IO;

namespace rdpWrapper {

  internal class Logger {

    public enum StateKind {
      Info,
      Warning,
      Error,
      Success,
    }

    public event Action<string, StateKind, bool> OnNewLogEvent;

    public virtual void Log(string message, StateKind state = StateKind.Info, bool newLine = true) {
      OnNewLogEvent?.Invoke(message, state, newLine);
    }
  }

  internal class FileLogger : Logger, IDisposable {

    private readonly string logFilePath;
    private readonly object sync = new();

    public FileLogger() {
      var dir = Path.GetDirectoryName(AppInfo.CurrentFileLocation);
      logFilePath = Path.Combine(string.IsNullOrEmpty(dir) ? "." : dir, "rdpWrapper.log");
    }

    public override void Log(string message, StateKind state = StateKind.Info, bool newLine = true) {
      base.Log(message, state, newLine);
      try {
        lock (sync) {
          File.AppendAllText(logFilePath, (newLine ? Environment.NewLine + $"{DateTime.Now:T} - " : "") + message);
        }
      }
      catch {
        // best-effort file logging; ignore failures (e.g. read-only folder)
      }
    }

    public void Dispose() {
    }
  }
}
