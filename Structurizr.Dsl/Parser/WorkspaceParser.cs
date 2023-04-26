using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class WorkspaceParser : IParser
  {
    private const string WORKSPACE = "workspace";
    private static /*crap*/ readonly List<string> ExtendParsed = new List<string>();

    public bool Accept(string line, ParsingContext context)
    {
      return line.StartsWith(WORKSPACE, StringComparison.InvariantCultureIgnoreCase);
    }

    public async ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      ArgumentNullException.ThrowIfNull(contextualWorkspace, nameof(contextualWorkspace));

      var tokens = line.Split(" ");

      if (string.Compare(tokens[1], "extends", true) == 0)
      {
        var fileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, tokens[2]));
        if (fileInfo.Exists)
        {
          if (!ExtendParsed.Contains(fileInfo.FullName))
          {
            contextualWorkspace = new ContextualWorkspace(await DslFileReader.ParseAsync(fileInfo, contextualWorkspace.Workspace, logger));
            ExtendParsed.Add(fileInfo.FullName);
          }
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