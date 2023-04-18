namespace structurizr.Cli
{
  public sealed class CliSettings
  {
    public string? BasePath { get; set; } = @"../../../../../structurizr";
    public bool PushToStructurizr { get; set; } = false;
  }
}