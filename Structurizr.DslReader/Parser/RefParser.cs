namespace Structurizr.DslReader.Parser
{
  public sealed class RefParser : IParser
  {
    private const string REF = "!ref";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{REF} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      var tokens = line.Split(' ');

      if(contextualWorkspace.Context.Model != null)
      {
        var softwaresystem = contextualWorkspace.Context.Model.GetSoftwareSystemWithId(tokens.GetValueAtOrDefault(1));
        if (softwaresystem == null)
          throw new Exception($"Unknow Software system [{tokens.GetValueAtOrDefault(1)}]");
        contextualWorkspace.Context.Set(softwaresystem);
      }
      else
      {
        throw new NotImplementedException();
      }

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}