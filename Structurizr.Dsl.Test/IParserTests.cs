using Structurizr.DslReader.Parser;
using Xunit;

namespace Structurizr.DslReader.Test
{
  public class IParserTests
  {
    [Fact]
    public void Accept_CommentParser()
    {
      const string line = "# !identifiers hierarchical";
      var parsers = ParserFactory.GetAllParsers();
      var parser = parsers.FirstOrDefault(p => p.Accept(line, new ParsingContext()));

      Assert.NotNull(parser);
      Assert.IsType<CommentParser>(parser);
    }
  }
}