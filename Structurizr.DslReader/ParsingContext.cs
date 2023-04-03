namespace Structurizr.DslReader
{
  public sealed class ParsingContext
  {
    public Workspace? Workspace { get; private set; }
    public ViewSet? Views { get; private set; }
    public Model? Model { get; private set; }
    public SoftwareSystem? SoftwareSystem { get; private set; }
    public Container? Container { get; private set; }
    public Component? Component { get; private set; }

    public void Set(Workspace workspace)
    {
      ResetAll();
      Workspace = workspace;
    }

    public void Set(Model model)
    {
      ResetAll();
      Model = model;
    }

    public void Set(SoftwareSystem softwareSystem)
    {
      ResetAll();
      SoftwareSystem = softwareSystem;
    }

    public void Set(ViewSet views)
    {
      ResetAll();
      Views = views;
    }

    public void Set(Container container)
    {
      ResetAll();
      Container = container;
    }

    public void Set(Component component)
    {
      ResetAll();
      Component = component;
    }

    private void ResetAll()
    {
      Workspace = null;
      Views = null;
      Model = null;
      SoftwareSystem = null;
      Container = null;
      Component = null;
    }
  }
}