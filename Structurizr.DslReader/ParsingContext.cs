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
    public Element? Element { get; private set; }
    public ElementStyle? ElementStyle { get; private set; }
    public RelationshipStyle? RelationshipStyle { get; private set; }

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
      ArgumentNullException.ThrowIfNull(softwareSystem, nameof(softwareSystem));
      
      ResetAll();
      SoftwareSystem = softwareSystem;
      Element = softwareSystem;
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
      Element = container;
    }

    public void Set(Component component)
    {
      ResetAll();
      Component = component;
      Element = component;
    }

    internal void Set(ElementStyle elementStyle)
    {
      ResetAll();
      ElementStyle = elementStyle;
    }

    internal void Set(RelationshipStyle relationshipStyle)
    {
      ResetAll();
      RelationshipStyle = relationshipStyle;
    }

    private void ResetAll()
    {
      Workspace = null;
      Views = null;
      Model = null;
      SoftwareSystem = null;
      Container = null;
      Component = null;
      Element = null;
      ElementStyle = null;
      RelationshipStyle = null;
    }
  }
}