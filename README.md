# Fergus' Minimal Language (fml)

Welcome to Fergus' Minimal Language (fml). It is essentially a direct rip off of TOML, but with less features. It is probably more accurate to say that it is a dialect of INI. The plan for fml was to set up a simple configuration language that could map to a ```Dictionary<string, string>``` in C#. I decided I did not want fml to try and make typing decisions like TOML, since that would have been harder for me to write and rather easy to do later in an application using fml.

## Example fml

```ini
[app]
name = Image Annotator
version = v0.1-beta
data = 1977-01-01

[database]
server = 255.255.255.255
id = 0123456789
files = { c:\temp\example.dat, e:\temp\example.dat }
# comment about the database
```
