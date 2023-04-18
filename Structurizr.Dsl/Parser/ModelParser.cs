using Microsoft.Extensions.Logging;
namespace Structurizr.DslReader.Parser
{
  public sealed class ModelParser : IParser
  {
    private const string MODEL = "model";

    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{MODEL} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      contextualWorkspace.Context.Set(contextualWorkspace.Workspace.Model);
      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}