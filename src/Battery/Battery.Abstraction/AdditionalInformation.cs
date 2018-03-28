using System;
namespace Canary.Battery.Abstraction
{
    /// <summary>
    /// Addirional information class for cases when
    /// a device might provide more information
    /// </summary>
    public class AdditionalInformation
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public AdditionalInformation(string title, string value, string description = null)
        {
            Title = title;
            Value = value;
            Description = description;
        }
    }
}
