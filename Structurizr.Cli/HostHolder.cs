using Microsoft.Extensions.Hosting;
using System.Diagnostics;

public class HostHolder
{
  private IHost? _host;

  public void RegisterHost(IHost host) => _host = host;
  public async Task Stop()
  {
    if(_host != null )
      await _host.StopAsync();
  }

  public void Exit()
  {
    Process.GetCurrentProcess().Kill();
  }
}