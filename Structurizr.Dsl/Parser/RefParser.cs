using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class RefParser : IParser
  {
    private const string REF = "!ref";
    private const string EXTEND = "!extend";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{REF} ", StringComparison.InvariantCultureIgnoreCase) || line.StartsWith($"{EXTEND} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = Tokenizer.Tokenize(line);

      if(contextualWorkspace.Workspace.Model != null)
      {
        var id = tokens.GetValueAtOrDefault(1);
        var element = contextualWorkspace.Workspace.Model.GetElement(id);
        if (element != null)
        {
          if(element is SoftwareSystem softwareSystem)
          {
            contextualWorkspace.Context.Set(softwareSystem);
          }
          else if(element is Container container)
          {
            contextualWorkspace.Context.Set(container);
          }
          else if (element is Component component)
          {
            contextualWorkspace.Context.Set(component);
          }
          else throw new Exception($"Unable to {REF} or {EXTEND}: {line}");
        }
      }
      else
      {
        throw new NotImplementedException();
      }

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}