namespace Structurizr.DslReader.Parser
{
    public sealed class SoftwareSystemParser : IParser
    {
        private const string SOFTWARE_SYSTEM = "SoftwareSystem";

        public bool Accept(string line)
        {
            return line.Split(" ").FirstOrDefault(s => string.Compare(s, SOFTWARE_SYSTEM, true) == 0) != null;
        }

        public ValueTask<Workspace?> ParseAsync(string line, Workspace? workspace, DirectoryInfo directoryInfo)
        {
            var tokens = line.Split(" ");

            if (string.Compare(tokens[0], SOFTWARE_SYSTEM, true) != 0) // softwareSystem {name} {description}
            {
                workspace.Model.AddSoftwareSystem(tokens[1], tokens[2]);
            }
            else
            {
                if (tokens[1] == "=") //{id} = SoftwareSystem {name}
                {
                    var softwareSystem = new SoftwareSystem();
                    softwareSystem.Id = tokens[0];
                    softwareSystem.Name = tokens[3];

                    workspace.Model.SoftwareSystems.Add(softwareSystem);
                }
                else
                    throw new Exception($"Unable to parse {SOFTWARE_SYSTEM}");
            }

            return ValueTask.FromResult(workspace);
        }
    }
}