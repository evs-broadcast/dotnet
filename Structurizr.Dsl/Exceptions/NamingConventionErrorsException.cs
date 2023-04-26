namespace Structurizr.Dsl.Exceptions;

[Serializable]
internal class NamingConventionErrorsException : Exception
{
  public NamingConventionErrorsException(NamingConventionsError namingConventionsError) : base(string.Join(Environment.NewLine, namingConventionsError.Errors))
  {
  }
}