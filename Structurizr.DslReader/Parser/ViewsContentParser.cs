namespace Structurizr.DslReader.Parser
{
  public sealed class ViewsContentParser : IParser
  {
    public bool Accept(string line, ParsingContext context)
    {
      return context.Views != null;
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}