namespace Structurizr.DslReader
{
  public static class StringArrayHelper
  {
    public static string? GetValueAtOrDefault(this string[] array, int ndx)
    {
      return array.Length > ndx ? array[ndx].Trim('"') : null;
    }

    public static string[] GetTagsAt(this string[] array, int startNdx)
    {
      var tag = array.GetValueAtOrDefault(startNdx);
      var tags = new List<string>();

      for(var i=startNdx; tag != null; i++)
      {
        tags.AddRange(tag.Split(',').Select(t=>t.Trim('"')));
        tag = array.GetValueAtOrDefault(i);
      }

      return tags.ToArray();
    }
  }
}