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
    //Environment.Exit(-1);
    throw new Exception("let's exit with -1 error code");
  }
}