namespace Structurizr.DslReader.Parser
{
  public sealed class ComponentParser : IParser
  {
    private const string COMPONENT = "component";
    public bool Accept(string line, ParsingContext context)
    {
      return line.Contains($"{COMPONENT} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      var tokens = line.Split(' ');
      if (contextualWorkspace.Context.Container == null)
        throw new Exception($"Container not set for line [{line}]");

      if (tokens.GetValueAtOrDefault(1) == "=") //{id} = component  {name} {description} {technology} {tags}
      {
        var container = contextualWorkspace.Context.Container.AddComponent(tokens.GetValueAtOrDefault(0), tokens.GetValueAtOrDefault(3),type: null, tokens.GetValueAtOrDefault(4), tokens.GetValueAtOrDefault(5));
        for (var i = 6; tokens.GetValueAtOrDefault(i) != null; i++)
        {
          container.AddTags(tokens.GetValueAtOrDefault(i).Split(','));
          contextualWorkspace.Context.Set(container);
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