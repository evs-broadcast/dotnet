namespace Structurizr.DslReader.Parser
{
  public sealed class ShapeParser : ElementorRelationshipStyleParser
  {
    public ShapeParser() : base("shape")
    {
    }

    protected override void SetValue(ElementStyle elementStyle, string value)
    {
      elementStyle.Shape = Enum.Parse<Shape>(value);
    }

    protected override void SetValue(RelationshipStyle relationshipStyle, string value)
    {
      throw new Exception($"{_key} is invalid for RelationshipStyle {relationshipStyle.Tag}");
    }
  }
}