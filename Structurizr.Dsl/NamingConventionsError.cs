public sealed class NamingConventionsError
{
  private readonly List<string> _errors = new List<string>();

  public void AddError(string domain, int line, string error) => _errors.Add($"{domain} at line {line} {error}");

  public IReadOnlyCollection<string> Errors => _errors;
}