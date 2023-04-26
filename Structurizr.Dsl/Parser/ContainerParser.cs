using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class ContainerParser : IParser
  {
    private const string CONTAINER = "container";
    public bool Accept(string line, ParsingContext context)
    {
      return line.Contains($"{CONTAINER} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = Tokenizer.Tokenize(line);
      if (contextualWorkspace.Context.SoftwareSystem == null)
        throw new Exception($"SoftwareSystem not set for line [{line}]");

      if (tokens.GetValueAtOrDefault(1) == "=") //{id} = Container {name} {description} {technology} {tags}
      {
        var container = contextualWorkspace.Context.SoftwareSystem.AddContainer(tokens.GetValueAtOrDefault(0), tokens.GetValueAtOrDefault(3), tokens.GetValueAtOrDefault(4), tokens.GetValueAtOrDefault(5));
        for (var i = 6; tokens.GetValueAtOrDefault(i) != null; i++)
        {
          container.AddTags(tokens.GetValueAtOrDefault(i)?.Split(','));
        }

        if (tokens.Last() == "{")
          contextualWorkspace.Context.Set(container);
        ContainerValidator.Validate(container, contextualWorkspace,lineNumber,directoryInfo);
      }
      else
      {
        throw new NotImplementedException();
      }

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}