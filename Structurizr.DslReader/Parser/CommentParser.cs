namespace Structurizr.DslReader.Parser
{
  public sealed class CommentParser : IParser
  {
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith("#");
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}