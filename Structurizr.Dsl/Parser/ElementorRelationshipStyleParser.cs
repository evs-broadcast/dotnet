using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public abstract class ElementorRelationshipStyleParser : IParser
  {
    protected ElementorRelationshipStyleParser(string key)
    {
      _key = key;
    }
    protected readonly string _key;

    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{_key} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = Tokenizer.Tokenize(line);
      var elementStyle = contextualWorkspace.Context.ElementStyle;
      if (elementStyle != null)
        SetValue(elementStyle, tokens[1]);
      else if (contextualWorkspace.Context.RelationshipStyle != null)
        SetValue(contextualWorkspace.Context.RelationshipStyle, tokens[1]);
      else
        throw new Exception($"No ElementStyle nor RelationshipStyle defined for line:{line}");
      return ValueTask.FromResult(contextualWorkspace);
    }

    protected abstract void SetValue(ElementStyle elementStyle, string value);
    protected abstract void SetValue(RelationshipStyle relationshipStyle, string value);
  }
}