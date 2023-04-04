namespace Structurizr.DslReader.Parser
{
  public interface IParser
  {
    bool Accept(string line, ParsingContext context);
    ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo);
  }
}