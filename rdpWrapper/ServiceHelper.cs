using System;
using System.ServiceProcess;

namespace rdpWrapper {

  internal class ServiceHelper {

    private readonly Logger logger;

    public ServiceHelper(Logger logger) {
      this.logger = logger;
    }

    public ServiceControllerStatus? GetState(string serviceName) {
      try {
        using var sc = new ServiceController(serviceName);
        return sc.Status;
      }
      catch {
        return null;
      }
    }

    public void Start(string serviceName, TimeSpan timeout) {
      using var sc = new ServiceController(serviceName);
      sc.Refresh();
      if (sc.Status == ServiceControllerStatus.Running) return;
      logger.Log($"Starting service '{serviceName}'...", Logger.StateKind.Info, false);
      sc.Start();
      sc.WaitForStatus(ServiceControllerStatus.Running, timeout);
    }

    public void Stop(string serviceName, TimeSpan timeout) {
      using var sc = new ServiceController(serviceName);
      sc.Refresh();
      if (sc.Status == ServiceControllerStatus.Stopped) return;
      logger.Log($"Stopping service '{serviceName}'...", Logger.StateKind.Info, false);
      sc.Stop();
      sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
    }
  }
}
