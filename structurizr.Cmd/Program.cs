// See https://aka.ms/new-console-template for more information
using Structurizr;
using Structurizr.DslReader;

Console.WriteLine("Hello, World!");

var fileInfo = new FileInfo(@"C:\Workspace\skylab\structurizr\logging\workspace.dsl");
var workspace = await DslFileReader.ParseAsync(fileInfo, null);

fileInfo = new FileInfo(@"C:\Workspace\skylab\structurizr\search\workspace.dsl");
workspace = await DslFileReader.ParseAsync(fileInfo, workspace);

Console.WriteLine($"Parsing succesfull software sytem:{workspace.Model.SoftwareSystems.Count}");



Console.WriteLine("Press 'enter' to exit");
Console.ReadLine();
