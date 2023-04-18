using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class EmptyLineParser : IParser
  {
    public bool Accept(string line, ParsingContext context)
    {
      return string.IsNullOrWhiteSpace(line);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace workspace, DirectoryInfo directoryInfo, ILogger logger) =>  ValueTask.FromResult(workspace);
  }
}