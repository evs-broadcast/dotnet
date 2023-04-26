using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class ViewsContentParser : IParser
  {
    public bool Accept(string line, ParsingContext context)
    {
      return context.Views != null;
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}