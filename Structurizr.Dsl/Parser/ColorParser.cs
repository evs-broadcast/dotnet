namespace Structurizr.DslReader.Parser
{
  public sealed class ColorParser : ElementorRelationshipStyleParser
  {
    public ColorParser() : base("color")
    {
    }

    protected override void SetValue(ElementStyle elementStyle, string value)
    {
      elementStyle.Color = value;
    }

    protected override void SetValue(RelationshipStyle relationshipStyle, string value)
    {
      relationshipStyle.Color = value;
    }
  }
}