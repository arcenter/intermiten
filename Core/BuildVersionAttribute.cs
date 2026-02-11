using System;

[AttributeUsage(AttributeTargets.Assembly)]
public class BuildVersionAttribute : Attribute
{
    public string Version { get; }
    public BuildVersionAttribute(string version) => Version = version;
}