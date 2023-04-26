namespace Structurizr.DslReader
{
  public sealed class ContextualWorkspace
  {
    public ContextualWorkspace(Workspace workspace)
    {
      Workspace = workspace;
      Context = new ParsingContext();
      NamingConventionsError = new NamingConventionsError();
      Context.Set(workspace);
    }

    public Workspace Workspace { get; set; }
    public ParsingContext Context { get; }
    public NamingConventionsError NamingConventionsError { get;}

    public void AddNamingConventionError(string domain, int lineNumber, string error) => NamingConventionsError.AddError(domain, lineNumber, error);
  }
}