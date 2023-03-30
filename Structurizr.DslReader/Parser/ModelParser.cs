using Structurizr;

namespace Structurizr.DslReader.Parser
{
  public sealed class ModelParser : IParser
  {
    private const string MODEL = "model";

    public bool Accept(string line)
    {
      return line.StartsWith(MODEL, StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<Workspace?> ParseAsync(string line, Workspace? workspace, DirectoryInfo directoryInfo)
    {
      return ValueTask.FromResult(workspace);
    }
  }
}