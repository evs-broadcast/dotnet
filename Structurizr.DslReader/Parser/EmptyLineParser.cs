using Structurizr;

namespace Structurizr.DslReader.Parser
{
  public sealed class EmptyLineParser : IParser
  {
    public bool Accept(string line)
    {
      return string.IsNullOrWhiteSpace(line);
    }

    public ValueTask<Workspace?> ParseAsync(string line, Workspace? workspace, DirectoryInfo directoryInfo)
    {
      return ValueTask.FromResult(workspace);
    }
  }
}