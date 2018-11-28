namespace Urq
{
    using System;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Automatically makes a button on the default inspector at runtime for string properties with the AutomaticEditorButtonAttribute
    /// The button calls the method with the name of the string value when clicked
    /// </summary>
    [CustomPropertyDrawer(typeof(AutomaticEditorButtonAttribute))]
    public class AutomaticEditorButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DrawAutoButton(position, property);
        }

        private void DrawAutoButton(Rect position, SerializedProperty property)
        {
            if (Application.isPlaying)
            {
                if (property.propertyType == SerializedPropertyType.String)
                {
                    Color defaultColor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.blue;
                    string methodName = property.stringValue;                  
                    if (EditorGUI.DropdownButton(position, new GUIContent(methodName), FocusType.Passive))
                    {
                        MethodInfo methodInfo = null;
                        UnityEngine.Object targetObject = null;
                        try
                        {
                            targetObject = property.serializedObject.targetObject;
                            Type targetType = targetObject.GetType();
                            methodInfo = targetType.GetMethod(methodName);
                        }
                        catch
                        {
                            Debug.LogError("Bad Method name for auto inspector button!");
                        }

                        if (methodInfo != null && targetObject != null)
                        {
                            methodInfo.Invoke(targetObject, null);
                        }

                    }
                    GUI.backgroundColor = defaultColor;
                }
            }
            else
            {
                if (property.propertyType != SerializedPropertyType.String)
                {
                    EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), "AutoMethod needs a string property!");
                }
                else
                {
                    EditorGUI.LabelField(new Rect(position.x, position.y, position.width, position.height), "Auto Inspector Button: ");
                    property.stringValue = EditorGUI.TextField(new Rect(position.x + 140, position.y, position.width-160, position.height), property.stringValue);
                }
            }
        }
    }
}