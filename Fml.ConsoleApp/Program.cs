// See https://aka.ms/new-console-template for more information
global using Fml;

FmlMap map = FmlMap.ReadFile("ExampleA.fml");

Dictionary<string, string> globalSettings = map.GlobalSettings;

foreach(var section in map.Sections)
{
    string sectionName = section.Key;
    Dictionary<string, string> sectionSettings = section.Value;
}

Console.WriteLine(map);
