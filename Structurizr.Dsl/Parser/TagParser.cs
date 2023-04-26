using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class TagParser : IParser
  {
    private const string TAGS= "tags";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{TAGS} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      if (contextualWorkspace.Context.Element == null)
        throw new Exception($"No Element defined for line {line}");

      var tokens = Tokenizer.Tokenize(line);
      for(var i=1;i<tokens.Length; i++)
      {
        contextualWorkspace.Context.Element.AddTags(tokens[i].Trim('"'));
      }

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}