using Microsoft.Extensions.Logging;

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

      WriteModel(workspace, writer);
      WriteViews(workspace, writer);
      writer.Close();

      return workspaceGeneratedFileInfo;
    }

    private static void WriteModel(Workspace workspace, StreamWriter writer)
    {
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
    }

    private static void WriteViews(Workspace workspace, StreamWriter writer)
    {
      writer.WriteLine("views {");
      WriteStyles(workspace, writer);
      writer.WriteLine("}");
    }

    private static void WriteStyles(Workspace workspace, StreamWriter writer)
    {
      writer.WriteLine(" styles {");

      foreach(var element in workspace.Views.Configuration.Styles.Elements)
      {
        writer.WriteLine($"element {Quote(element.Tag)} {{");
        WriteIf(writer, "background", element.Background);
        WriteIf(writer, "color", element.Color);
        WriteIf(writer, "shape", element.Shape.ToString());
        writer.WriteLine("}");
      }

      foreach(var relationship in workspace.Views.Configuration.Styles.Relationships)
      {
        writer.WriteLine($"relationship {Quote(relationship.Tag)} {{");
        WriteIf(writer, "style", relationship.Style);
        WriteIf(writer, "color", relationship.Color);
        writer.WriteLine("}");
      }

      WriteThemes(workspace, writer);
      writer.WriteLine("}");
    }

    private static void WriteThemes(Workspace workspace, StreamWriter writer)
    {
      writer.WriteLine("theme default");
      writer.WriteLine("theme https://static.structurizr.com/themes/kubernetes-v0.3/theme.json");
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
      WriteIf(writer,"tags",tags);
    }

    private static void WriteDescriptionIf(StreamWriter writer, string? description)
    {
      WriteIf(writer, "description", description);
    }

    private static void WriteTechnologyIf(StreamWriter writer, string? technology)
    {
      WriteIf(writer, "technology", technology);
    }

    private static void WriteIf(StreamWriter writer,string key, string? value)
    {
      if (!string.IsNullOrWhiteSpace(value))
        writer.WriteLine($"{key} {Quote(value)}");
    }

    private static string Quote(string content)
    {
      return content == null ? "\"\"" : $"\"{content.Trim('"')}\"";
    }
  }
}