using Structurizr.DslReader.Parser;
using Xunit;

namespace Structurizr.DslReader.Test
{
  public class TokenizerTest
  {
    [Fact]
    public void Tokenizer_HandleOpenBracketWithoutSpace()
    {
      const string line = "sub_search = softwareSystem \"via-search\"{";
      var tokens = Tokenizer.Tokenize(line);
      Assert.Equal("{", tokens.Last());
      Assert.Equal(5, tokens.Count());
    }

    [Fact]
    public void Tokenizer_HandleDoubleQuotewithspace()
    {
      const string line = "sub_search = softwareSystem \"my system\" tag";
      var tokens = Tokenizer.Tokenize(line);      
      Assert.Equal(5, tokens.Count());
    }
  }
}