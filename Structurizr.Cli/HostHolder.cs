using System.Diagnostics;

public class HostHolder
{
  private CancellationTokenSource _cancellationTokenSource;

  public HostHolder()
  {
    _cancellationTokenSource = new CancellationTokenSource();
  }

  public CancellationToken CancellationToken => _cancellationTokenSource.Token;

  public void Stop()
  {
    _cancellationTokenSource.Cancel();
  }

  public void Exit()
  {
    Process.GetCurrentProcess().Kill();
  }
}