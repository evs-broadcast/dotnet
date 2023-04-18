namespace Structurizr.DslReader
{
  public sealed class ContextualWorkspace
  {
    public ContextualWorkspace(Workspace workspace)
    {
      Workspace = workspace;
      Context = new ParsingContext();
      Context.Set(workspace);
    }

    public Workspace Workspace { get; set; }
    public ParsingContext Context { get; }
  }
}