using System.Text;

namespace Structurizr.DslReader.Parser
{
  public static class Tokenizer
  {
    public static string[] Tokenize(string line)
    {
      var tokens = new List<string>();

      var insideQuotes = false;
      var currentToken = new StringBuilder();

      foreach (var c in line)
      {
        if (c == '"')
        {
          insideQuotes = !insideQuotes;
          currentToken.Append(c);
        }
        else if (c == ' ' && !insideQuotes)
        {
          if (currentToken.Length > 0)
          {
            tokens.Add(currentToken.ToString());
            currentToken.Clear();
          }
        }
        else if(c == '{')
        {
          tokens.Add(currentToken.ToString());
          currentToken.Clear();
          tokens.Add("{");
        }
        else
        {
          currentToken.Append(c);
        }
      }

      if (currentToken.Length > 0)
      {
        tokens.Add(currentToken.ToString());
      }

      return tokens.ToArray();
    }
  }
}