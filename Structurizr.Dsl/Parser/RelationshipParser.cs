using System.Data;
using Microsoft.Extensions.Logging;

namespace Structurizr.DslReader.Parser
{
  public sealed class RelationshipParser : IParser
  {
    private const string RELATION = "->";
    private static readonly string[] TECH = { "http","https", "WS", "command", "event", "querycommand", "queryevent", "grpc","wf","db", "ZMQ", "inproc" };
    public bool Accept(string line, ParsingContext context)
    {
      return line.Contains($"{RELATION} ", StringComparison.InvariantCultureIgnoreCase);
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
    {
      var tokens = Tokenizer.Tokenize(line);

      Relationship relationship;

      if (tokens.GetValueAtOrDefault(0) == RELATION) /*-> <identifier> [description] [technology] [tags]*/
      {
        var destinationId = tokens.GetValueAtOrDefault(1);
        var destination = contextualWorkspace.Workspace.Model.GetElement(destinationId);
        if (destination == null) throw new Exception($"Unable to find element [{destinationId}]");

        relationship = contextualWorkspace.Workspace.Model.AddRelationship(contextualWorkspace.Context.Element, destination, tokens.GetValueAtOrDefault(2), tokens.GetValueAtOrDefault(3), null, tokens.GetTagsAt(4));
      }
      else if (tokens.GetValueAtOrDefault(1) == RELATION) /*<identifier> -> <identifier> [description] [technology] [tags] */
      {
        var sourceId = tokens.GetValueAtOrDefault(0);
        var destinationId = tokens.GetValueAtOrDefault(2);

        var source = contextualWorkspace.Workspace.Model.GetElement(sourceId);
        if (source == null) throw new Exception($"Unable to find element [{source}]");

        var destination = contextualWorkspace.Workspace.Model.GetElement(destinationId);
        if (destination == null) throw new Exception($"Unable to find element [{destinationId}]");

        relationship = contextualWorkspace.Workspace.Model.AddRelationship(source, destination, tokens.GetValueAtOrDefault(3), tokens.GetValueAtOrDefault(4), null, tokens.GetTagsAt(5));
      }
      else
      {
        throw new Exception($"unable to parse [{line}]");
      }
      
      if (!TECH.Any(t => t == relationship.Technology))
        contextualWorkspace.AddNamingConventionError(directoryInfo.Name, lineNumber, $"Wrong relationship technology [{relationship.Technology}] MUST be {string.Join(" or ", TECH)}");

      relationship.AddTags(relationship.Technology);

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}