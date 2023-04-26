namespace Structurizr.Dsl.Exceptions;
[Serializable]
internal class ParsingException : Exception
{
  public ParsingException(string domain, int lineNumber, string error) : base($"{domain} at line {lineNumber} {error}")
  {
  }
}