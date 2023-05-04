using Microsoft.Extensions.Logging;
using Structurizr.Dsl.Exceptions;

namespace Structurizr.DslReader.Parser;

  public sealed class SoftwareSystemParser : IParser
{
  private const string SOFTWARE_SYSTEM = "SoftwareSystem";
  private const string PREFIX = "ss_";

  public bool Accept(string line, ParsingContext context)
  {
    return line.Split(" ").FirstOrDefault(s => string.Compare(s, SOFTWARE_SYSTEM, true) == 0) != null;
  }

  public ValueTask<ContextualWorkspace> ParseAsync(string line,int lineNumber, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, ILogger logger)
  {
    var tokens = Tokenizer.Tokenize(line);

    SoftwareSystem softwareSystem;
    if (string.Compare(tokens.GetValueAtOrDefault(0), SOFTWARE_SYSTEM, true) == 0) // softwareSystem = {name} {description}
    {
      softwareSystem = contextualWorkspace.Workspace.Model.AddSoftwareSystem(tokens.GetValueAtOrDefault(2), tokens.GetValueAtOrDefault(3));
      softwareSystem.AddTags(tokens.GetTagsAt(4));
    }
    else
    {
      if (tokens.GetValueAtOrDefault(1) == "=") //{id} = SoftwareSystem {name}
      {
        softwareSystem = contextualWorkspace.Workspace.Model.AddSoftwareSystem(tokens.GetValueAtOrDefault(0), Location.Unspecified, tokens.GetValueAtOrDefault(3), tokens.GetValueAtOrDefault(4));
        softwareSystem.AddTags(tokens.GetTagsAt(5));
      }
      else
        throw new ParsingException(directoryInfo.Name, lineNumber, $"Unable to parse {SOFTWARE_SYSTEM}");
    }

    if (softwareSystem == null)
      throw new Exception($"Dupplicate {SOFTWARE_SYSTEM} id {tokens.GetValueAtOrDefault(0)} at line {line} in file {directoryInfo.Name}/workspace.dsl");

    AssetNamingConvention(contextualWorkspace, directoryInfo, softwareSystem, lineNumber);

    contextualWorkspace.Context.Set(softwareSystem);

    return ValueTask.FromResult(contextualWorkspace);
  }

  private static void AssetNamingConvention(ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo, SoftwareSystem softwareSystem, int lineNumber)
  {
    if (!softwareSystem.Id.StartsWith(PREFIX))
      contextualWorkspace.NamingConventionsError.AddError(directoryInfo.Name, lineNumber, $"SoftwareSystem prefix MUST be {PREFIX} ({softwareSystem.Id})");
  }
}