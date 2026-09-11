using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace rdpWrapper {

  internal static class StringExtensions {
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);
    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);
  }

  internal static class WinApiHelper {

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int RegisterWindowMessage(string message);

    public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME_RDPWRAPPER");
  }

  internal static class OSHelper {

    public static bool IsWindowsServer {
      get {
        try {
          using var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\ProductOptions");
          var productType = key?.GetValue("ProductType") as string;
          return productType != null && !productType.Equals("WinNT", StringComparison.OrdinalIgnoreCase);
        }
        catch {
          return false;
        }
      }
    }
  }

  internal static class WinStationHelper {

    private const string RegRdpKey = @"SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp";

    public static bool IsListenerWorking() {
      try {
        var port = 3389;
        using (var key = Registry.LocalMachine.OpenSubKey(RegRdpKey)) {
          if (key?.GetValue("PortNumber") is int p) port = p;
        }
        return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any(l => l.Port == port);
      }
      catch {
        return false;
      }
    }
  }
}
