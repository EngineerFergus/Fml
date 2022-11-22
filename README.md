# Fergus' Minimal Language (fml)

Welcome to Fergus' Minimal Language (fml). It is essentially a direct rip off of TOML, but with less features. It is probably more accurate to say that it is a dialect of INI. The plan for fml was to set up a simple configuration language that could map to a ```Dictionary<string, string>``` in C#. Did I succeed in my plan? I'm not too sure. I did quickly learn that trying to make a fully featured programming language was not going to happen in the short amount of time that my attention can stay on any single hobby project.

## Overview

Are you tired of complicated config files? Wish for the old days of simple INI files, but wish there was at least some type of specification? Well, you're not getting that with fml. You will get the simple config file, though. There are no multi line strings or value assignments. There are no types. You get sections and key/value pairs. That's it. That is all you need. Actually, that is all I could figure out in terms of language scanning and parsing, and even then my implementation is hacky at best. Do not use this in production. Please.

## Example fml file

```ini
[app]
name = Image Annotator # inline comment
version = v0.1-beta
data = 1977-01-01

[database]
server = 255.255.255.255
id = 0123456789
# comment about the database
```

## Quick Start

```csharp
FmlMap map = FmlMap.ReadFile("ExampleA.fml");

Dictionary<string, string> globalSettings = map.GlobalSettings;

foreach(var section in map.Sections)
{
    string sectionName = section.Key;
    Dictionary<string, string> sectionSettings = section.Value;
}
```
