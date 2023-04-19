namespace structurizr_cli.Configuration
{
    public sealed class CliConfiguration
    {
        public string? BasePath { get; set; } = @"C:\Workspace\skylab\structurizr";
        public bool PushToStructurizr { get; set; } = false;
    }
}