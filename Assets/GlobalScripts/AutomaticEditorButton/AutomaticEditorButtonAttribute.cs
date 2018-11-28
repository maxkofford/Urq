namespace Urq
{
    using UnityEngine;

    /// <summary>
    /// An attribute for property strings to automatically generate a inspector button while running to call a method with the same name of the string value
    /// </summary>
    public class AutomaticEditorButtonAttribute : PropertyAttribute
    {
        public AutomaticEditorButtonAttribute()
        {
        }
    }
}