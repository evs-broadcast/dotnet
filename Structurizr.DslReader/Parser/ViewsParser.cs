namespace Structurizr.DslReader.Parser
{
  public sealed class ViewsParser : IParser
  {
    private const string VIEWS = "views";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{VIEWS} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      contextualWorkspace.Context.Set(contextualWorkspace.Workspace.Views);
      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}