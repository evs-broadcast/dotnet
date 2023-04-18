namespace Structurizr.DslReader.Parser
{
  public sealed class BackgroundParser : ElementorRelationshipStyleParser
  {
    public BackgroundParser() : base("background")
    {
    }

    protected override void SetValue(ElementStyle elementStyle, string value)
    {
      elementStyle.Background = value;
    }

    protected override void SetValue(RelationshipStyle relationshipStyle, string value)
    {
      throw new Exception($"{_key} is invalid for RelationshipStyle {relationshipStyle.Tag}");
    }
  }
}