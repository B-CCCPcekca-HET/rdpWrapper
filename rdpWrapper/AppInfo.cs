using System.Reflection;

namespace rdpWrapper {

  internal static class AppInfo {

    public static readonly string ApplicationName;
    public static readonly string ApplicationTitle;
    public static readonly string CurrentVersion;
    public static readonly string CurrentFileLocation;

    static AppInfo() {
      var assembly = Assembly.GetExecutingAssembly();
      ApplicationName = assembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product ?? "RDPWrapper";
      ApplicationTitle = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? ApplicationName;
      CurrentVersion = assembly.GetName().Version?.ToString(3) ?? "0.0.0";
      CurrentFileLocation = assembly.Location;
    }
  }
}
