namespace Structurizr.DslReader.Parser
{
  public sealed class CloseBracketParser : IParser
  {
    public bool Accept(string line, ParsingContext context)
    {
      return line == "}";
    }

    public ValueTask<ContextualWorkspace> ParseAsync(string line, ContextualWorkspace contextualWorkspace, DirectoryInfo directoryInfo)
    {
      var context = contextualWorkspace.Context;

      if (context.Model != null) context.Set(contextualWorkspace.Workspace);
      if (context.Views != null) context.Set(contextualWorkspace.Workspace);
      if (context.SoftwareSystem != null) context.Set(context.SoftwareSystem.Model);
      if (context.Container != null) context.Set(context.Container.SoftwareSystem);
      if (context.Component != null) context.Set(context.Component.Container);

      return ValueTask.FromResult(contextualWorkspace);
    }
  }
}