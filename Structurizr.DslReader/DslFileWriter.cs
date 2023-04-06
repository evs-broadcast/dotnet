namespace Structurizr.DslReader
{
  public sealed class DslFileWriter
  {
    public static FileInfo Write(Workspace workspace, DirectoryInfo directoryInfo)
    {
      var workspaceGeneratedFileInfo = new FileInfo(Path.Combine(directoryInfo.Parent.FullName, "structurizr-gen", "workspace.dsl"));

      CreateParents(workspaceGeneratedFileInfo);

      if (workspaceGeneratedFileInfo.Exists) { workspaceGeneratedFileInfo.Delete(); }

      using var writer = workspaceGeneratedFileInfo.CreateText();

      writer.WriteLine($"workspace {workspace.Name} {{");
      writer.WriteLine("model {");
      //writer.WriteLine("!identifiers hierarchical");

      foreach (var softwareSystem in workspace.Model.SoftwareSystems)
      {
        writer.WriteLine($"{softwareSystem.Id} = softwareSystem {Quote(softwareSystem.Name)} {{");
        WriteDescriptionIf(writer, softwareSystem.Description);
        WriteTagsIf(writer, softwareSystem.Tags);

        foreach (var container in softwareSystem.Containers)
        {
          writer.WriteLine($"{container.Id} = container {Quote(container.Name)} {{");
          WriteDescriptionIf(writer, container.Description);
          WriteTagsIf(writer, container.Tags);
          WriteTechnologyIf(writer, container.Technology);

          foreach (var component in container.Components)
          {
            writer.WriteLine($"{component.Id} = component {Quote(component.Name)} {{");
            WriteDescriptionIf(writer, component.Description);
            WriteTagsIf(writer, component.Tags);
            WriteTechnologyIf(writer, component.Technology);
            writer.WriteLine("}");
          }

          writer.WriteLine("}");
        }

        writer.WriteLine("}");
      }

      foreach (var softwareSystem in workspace.Model.SoftwareSystems)
      {
        WriteRelationship(writer, softwareSystem.Relationships);

        foreach (var container in softwareSystem.Containers)
        {
          WriteRelationship(writer, container.Relationships);

          foreach (var component in container.Components)
          {
            WriteRelationship(writer, component.Relationships);
          }
        }
      }

      writer.WriteLine("}");
      writer.WriteLine("views {");
      writer.WriteLine("theme default");
      writer.WriteLine("}");

      writer.WriteLine("}");
      writer.Close();

      return workspaceGeneratedFileInfo;

    }

    private static void WriteRelationship(StreamWriter writer, ISet<Relationship> relationships)
    {
      foreach(var relationship in relationships) 
      {
        writer.WriteLine($"{relationship.SourceId} -> {relationship.DestinationId} {Quote(relationship.Description)} {Quote(relationship.Technology)} {Quote(relationship.Tags)}");
      }
    }

    private static void CreateParents(FileInfo fileInfo)
    {
      Create(fileInfo.Directory);
    }

    private static void Create(DirectoryInfo directoryInfo)
    {
      if (!directoryInfo.Parent.Exists)
        Create(directoryInfo.Parent);
      if (!directoryInfo.Exists)
        directoryInfo.Create();
    }

    private static void WriteTagsIf(StreamWriter writer, string? tags)
    {
      if (!string.IsNullOrWhiteSpace(tags))
        writer.WriteLine($"tags \"{tags}\"");
    }

    private static void WriteDescriptionIf(StreamWriter writer, string? description)
    {
      if (!string.IsNullOrWhiteSpace(description))
        writer.WriteLine($"description \"{description}\"");
    }

    private static void WriteTechnologyIf(StreamWriter writer, string? technology)
    {
      if (!string.IsNullOrWhiteSpace(technology))
        writer.WriteLine($"technology \"{technology}\"");
    }

    private static string Quote(string content)
    {
      return content == null ? "\"\"" : $"\"{content}\"";
    }
  }
}