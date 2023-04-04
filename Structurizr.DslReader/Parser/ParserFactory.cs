namespace Structurizr.DslReader.Parser
{
  public sealed class ParserFactory
  {
    public static IParser[] GetAllParsers()
    {
      var interfaceType = typeof(IParser);
      var all = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(x => x.GetTypes())
        .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
        .Select(x => Activator.CreateInstance(x))
        .Cast<IParser>();
      return all.ToArray();
    }
  }
}