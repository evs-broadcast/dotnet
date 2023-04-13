namespace Structurizr.DslReader.Parser
{
  public static class Tokenizer
  {
    public static string[] Tokenize(string line)
    {
      var tokens = line.Split(' ').ToList();
      var lastToken = tokens.Last();
      if (lastToken.Length > 1 && lastToken[^1]=='{')
      {
        tokens.Remove(lastToken);
        tokens.Add(lastToken[..^1]);
        tokens.Add("{");
      }
      return tokens.ToArray();
    }
  }
}