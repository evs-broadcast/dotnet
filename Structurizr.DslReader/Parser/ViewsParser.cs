using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class ViewsParser : IParser
  {
    private const string VIEWS = "views";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{VIEWS} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      contextualWorkspace.Context.Set(contextualWorkspace.Workspace.Views);
      return ValueTask.FromResult(contextualWorkspace);
    }
  }

  //public sealed class ElementParser : IParser
  //{
  //  private const string VIEWS = "element";
  //  public bool Accept(string line, ParsingContext context)
  //  {
  //    return line.StartsWith($"{VIEWS} ", StringComparison.InvariantCultureIgnoreCase);
  //  }

  //  public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
  //  {
  //    var tokens = line.Split(' ');
  //    //contextualWorkspace.Workspace.Views.Configuration.Styles.Add(new RelationshipStyle {Tag= })
  //  }
  //}
}