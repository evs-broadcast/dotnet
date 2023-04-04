namespace Structurizr.DslReader.Parser
{
    public sealed class SoftwareSystemParser : IParser
  {
    private const string SOFTWARE_SYSTEM = "SoftwareSystem";

    public bool Accept(string line, ParsingContext context)
    {
      return line.Split(" ").FirstOrDefault(s => string.Compare(s, SOFTWARE_SYSTEM, true) == 0) != null;
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      var tokens = line.Split(" ");

      SoftwareSystem softwareSystem;
      if (string.Compare(tokens.GetValueAtOrDefault(0), SOFTWARE_SYSTEM, true) == 0) // softwareSystem = {name} {description}
      {
        softwareSystem = contextualWorkspace.Workspace.Model.AddSoftwareSystem(tokens.GetValueAtOrDefault(2), tokens.GetValueAtOrDefault(3));
      }
      else
      {
        if (tokens.GetValueAtOrDefault(1) == "=") //{id} = SoftwareSystem {name}
        {
          softwareSystem = contextualWorkspace.Workspace.Model.AddSoftwareSystem(tokens.GetValueAtOrDefault(0), Location.Unspecified, tokens.GetValueAtOrDefault(3), tokens.GetValueAtOrDefault(4));
        }
        else
          throw new Exception($"Unable to parse {SOFTWARE_SYSTEM}");
      }

      if (tokens.Last() == "{")
        contextualWorkspace.Context.Set(softwareSystem);

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}