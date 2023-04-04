// See https://aka.ms/new-console-template for more information
using Structurizr.DslReader;

Console.WriteLine("Hello, World!");

var fileInfo = new FileInfo(@"C:\Workspace\skylab\structurizr\logging\workspace.dsl");
var reader = new DslFileReader();
var workspace = await reader.ParseAsync(fileInfo);

Console.WriteLine($"Parsing succesfull software sytem:{workspace.Model.SoftwareSystems.Count}");
Console.ReadLine();
