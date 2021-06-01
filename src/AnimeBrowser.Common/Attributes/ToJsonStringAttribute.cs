using System;

namespace AnimeBrowser.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ToJsonStringAttribute : Attribute
    {
        public bool Preserve { get; set; } = true;
        public ToJsonStringAttribute()
        {
        }
    }
}
