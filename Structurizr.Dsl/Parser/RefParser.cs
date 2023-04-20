using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class RefParser : IParser
  {
    private const string REF = "!ref";
    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith($"{REF} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = line.Split(' ');

      if(contextualWorkspace.Context.Model != null)
      {
        string? softwareSystemId = tokens.GetValueAtOrDefault(1);
        var softwaresystem = contextualWorkspace.Context.Model.GetSoftwareSystemWithId(softwareSystemId);
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