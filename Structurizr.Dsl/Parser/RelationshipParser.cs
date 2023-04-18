using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class RelationshipParser : IParser
  {
    private const string RELATION = "->";
    public bool Accept(string line, ParsingContext context)
    {
      return line.Contains($"{RELATION} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = line.Split(' ');

      if (tokens.GetValueAtOrDefault(0) == RELATION) /*-> <identifier> [description] [technology] [tags]*/
      {
        var destinationId = tokens.GetValueAtOrDefault(1);
        var destination = contextualWorkspace.Workspace.Model.GetElement(destinationId);
        if (destination == null) throw new Exception($"Unable to find element [{destinationId}]");

        contextualWorkspace.Workspace.Model.AddRelationship(contextualWorkspace.Context.Element, destination, tokens.GetValueAtOrDefault(2), tokens.GetValueAtOrDefault(3), null, tokens.GetTagsAt(4));
      }
      else if (tokens.GetValueAtOrDefault(1) == RELATION) /*<identifier> -> <identifier> [description] [technology] [tags] */
      {
        var sourceId = tokens.GetValueAtOrDefault(0);
        var destinationId = tokens.GetValueAtOrDefault(2);

        var source = contextualWorkspace.Workspace.Model.GetElement(sourceId);
        if (source == null) throw new Exception($"Unable to find element [{source}]");

        var destination = contextualWorkspace.Workspace.Model.GetElement(destinationId);
        if (destination == null) throw new Exception($"Unable to find element [{destinationId}]");

        contextualWorkspace.Workspace.Model.AddRelationship(source, destination, tokens.GetValueAtOrDefault(3), tokens.GetValueAtOrDefault(4), null, tokens.GetTagsAt(5));
      }
      else
        throw new Exception($"unable to parse [{line}]");

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}