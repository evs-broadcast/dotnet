using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class RelationshipViewParser : IParser
  {
    private const string RELATIONSHIP = "relationship";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{RELATIONSHIP} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = Tokenizer.Tokenize(line);
      var relationshipStyle = FindOrCreate(contextualWorkspace.Workspace.Views.Configuration.Styles, tokens[1]);
      contextualWorkspace.Context.Set(relationshipStyle);
      return ValueTask.FromResult(contextualWorkspace);
    }

    private static RelationshipStyle FindOrCreate(Styles styles, string tag)
    {
      var relationshipStyle = styles.Relationships.FirstOrDefault(relationshipStyle => relationshipStyle.Tag == tag);
      if (relationshipStyle == null)
      {
        relationshipStyle = new RelationshipStyle(tag);
        styles.Add(relationshipStyle);
      }
      return relationshipStyle;
    }
  }
}