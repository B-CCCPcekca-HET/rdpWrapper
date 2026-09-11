using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace rdpWrapper {

  internal class PersistentSettings {

    private const string RegistryPath = @"Software\RDPWrapper";
    private readonly Dictionary<string, string> values = new(StringComparer.OrdinalIgnoreCase);
    private string filePath;

    public bool IsPortable { get; set; }

    public void Load() {
      values.Clear();
      var dir = Path.GetDirectoryName(AppInfo.CurrentFileLocation);
      filePath = Path.Combine(string.IsNullOrEmpty(dir) ? "." : dir, "rdpWrapper.settings");

      if (File.Exists(filePath)) {
        IsPortable = true;
        try {
          foreach (var line in File.ReadAllLines(filePath)) {
            var idx = line.IndexOf('=');
            if (idx > 0) values[line.Substring(0, idx)] = line.Substring(idx + 1);
          }
        }
        catch {
          // ignore corrupt settings file
        }
      }
      else {
        IsPortable = false;
        try {
          using var key = Registry.CurrentUser.OpenSubKey(RegistryPath);
          if (key != null) {
            foreach (var name in key.GetValueNames())
              values[name] = key.GetValue(name)?.ToString() ?? string.Empty;
          }
        }
        catch {
          // ignore inaccessible registry
        }
      }
    }

    public T GetValue<T>(string key, T defaultValue) {
      if (!values.TryGetValue(key, out var raw))
        return defaultValue;
      try {
        if (typeof(T) == typeof(string)) return (T)(object)raw;
        if (typeof(T).IsEnum) return (T)Enum.Parse(typeof(T), raw, true);
        return (T)Convert.ChangeType(raw, typeof(T));
      }
      catch {
        return defaultValue;
      }
    }

    public void SetValue(string key, string value) {
      values[key] = value;
      try {
        if (IsPortable) {
          var lines = new List<string>();
          foreach (var kv in values)
            lines.Add($"{kv.Key}={kv.Value}");
          File.WriteAllLines(filePath, lines);
        }
        else {
          using var key2 = Registry.CurrentUser.CreateSubKey(RegistryPath);
          key2?.SetValue(key, value);
        }
      }
      catch {
        // best-effort persistence; ignore failures (read-only location, missing permissions)
      }
    }
  }

  internal class UserOption {

    public event Action Changed;

    public bool Value { get; private set; }

    public UserOption(string key, bool defaultValue, ToolStripMenuItem menuItem, PersistentSettings settings) {
      Value = settings.GetValue(key, defaultValue);
      menuItem.CheckOnClick = true;
      menuItem.Checked = Value;
      menuItem.CheckedChanged += (_, _) => {
        Value = menuItem.Checked;
        settings.SetValue(key, Value.ToString());
        Changed?.Invoke();
      };
    }
  }
}
