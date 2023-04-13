namespace Structurizr.DslReader.Parser
{
  public sealed class StyleParser : ElementorRelationshipStyleParser
  {
    public StyleParser() : base("style")
    {
    }

    protected override void SetValue(ElementStyle elementStyle, string value)
    {
      throw new Exception($"{_key} is not valid for elementStyle {elementStyle.Tag}");
    }

    protected override void SetValue(RelationshipStyle relationshipStyle, string value)
    {
      relationshipStyle.Style = value;
    }
  }
}