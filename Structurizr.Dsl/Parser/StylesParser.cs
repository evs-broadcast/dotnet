using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class StylesParser : IParser
  {
    private const string STYLES = "styles";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{STYLES} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}