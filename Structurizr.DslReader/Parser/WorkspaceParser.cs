namespace Structurizr.DslReader.Parser
{
  public sealed class WorkspaceParser : IParser
  {
    private const string WORKSPACE = "workspace";

    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith(WORKSPACE, StringComparison.InvariantCultureIgnoreCase);
    }

    public async ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      ArgumentNullException.ThrowIfNull(contextualWorkspace, nameof(contextualWorkspace));

      var tokens = line.Split(" ");

      if (string.Compare(tokens[1], "extends", true) == 0)
      {
        var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, tokens[2]));
        if (fileInfo.Exists)
        {
          contextualWorkspace = new ContextualWorkspace(await DslFileReader.ParseAsync(fileInfo,contextualWorkspace.Workspace));
        }
      }
      else
      {
        var name = tokens.GetValueAtOrDefault(1);

        if (contextualWorkspace.Workspace == null)
        {
          contextualWorkspace = new ContextualWorkspace(new Workspace(name, tokens.GetValueAtOrDefault(2)));
        }
        else if (contextualWorkspace.Workspace.Name == name)
        {
          //Let's append to the existing workspace
          return contextualWorkspace;
        }
        else
        {
          throw new Exception($"Unable to merge 2 workspace: [{contextualWorkspace.Workspace.Name}] [{name}]");
        }
      }

      return contextualWorkspace;
    }
  }
}