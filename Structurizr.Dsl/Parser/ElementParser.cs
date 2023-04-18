using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class ElementParser : IParser
  {
    private const string ELEMENT = "element";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{ELEMENT} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = Tokenizer.Tokenize(line);
      ElementStyle elementStyle = FindOrCreate(contextualWorkspace.Workspace.Views.Configuration.Styles, tokens[1]);
      contextualWorkspace.Context.Set(elementStyle);

      return ValueTask.FromResult(contextualWorkspace);
    }

    private static ElementStyle FindOrCreate(Styles styles, string tag)
    {
      var elementStyle = styles.Elements.FirstOrDefault(elementStyle => elementStyle.Tag == tag);
      if(elementStyle == null)
      {
        elementStyle = new ElementStyle(tag);
        styles.Add(elementStyle);
      }
      return elementStyle;
    }
  }
}