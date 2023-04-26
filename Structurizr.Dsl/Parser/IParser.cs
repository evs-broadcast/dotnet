using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public interface IParser
  {
    bool Accept(string line, ParsingContext context);
    ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger);
  }
}