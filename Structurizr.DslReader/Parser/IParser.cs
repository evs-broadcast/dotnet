using Structurizr;

namespace Structurizr.DslReader.Parser
{
  public interface IParser
  {
    bool Accept(string line);
    ValueTask<Workspace?> ParseAsync(string line, Workspace? workspace, DirectoryInfo directoryInfo);
  }
}