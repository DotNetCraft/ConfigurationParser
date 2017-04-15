using System;

namespace DotNetCraft.ConfigurationParser.Attributes
{
    /// <summary>
    /// Use this attribute if you want to ignore property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnorePropertyAttribute: Attribute
    {
    }
}
