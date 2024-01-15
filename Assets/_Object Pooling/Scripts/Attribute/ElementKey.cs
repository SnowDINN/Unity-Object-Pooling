using UnityEditor;
using UnityEngine;

public class ElementKeyAttribute : PropertyAttribute
{
	public string ElementName;

	public ElementKeyAttribute(string ElementName)
	{
		this.ElementName = ElementName;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ElementKeyAttribute))]
public class ElementKeyDrawer : PropertyDrawer
{
	private SerializedProperty Target;
	protected virtual ElementKeyAttribute Atribute => (ElementKeyAttribute)attribute;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var FullPathName = property.propertyPath + "." + Atribute.ElementName;
		Target = property.serializedObject.FindProperty(FullPathName);

		var newlabel = GetTitle();
		if (string.IsNullOrEmpty(newlabel))
			newlabel = label.text;
		EditorGUI.PropertyField(position, property, new GUIContent(newlabel, label.tooltip), true);
	}

	private string GetTitle()
	{
		return Target.propertyType switch
		{
			SerializedPropertyType.Generic => $"{Target.type}",
			SerializedPropertyType.Integer => $"{Target.intValue}",
			SerializedPropertyType.Boolean => $"{Target.boolValue}",
			SerializedPropertyType.Float => $"{Target.floatValue}",
			SerializedPropertyType.String => $"{Target.stringValue}",
			SerializedPropertyType.Color => $"{Target.colorValue}",
			SerializedPropertyType.ObjectReference => $"{Target.objectReferenceValue.name}",
			SerializedPropertyType.Enum => $"{Target.enumNames[Target.enumValueIndex]}",
			SerializedPropertyType.Vector2 => $"{Target.vector2Value}",
			SerializedPropertyType.Vector3 => $"{Target.vector3Value}",
			SerializedPropertyType.Vector4 => $"{Target.vector4Value}",
			_ => ""
		};
	}
}
#endif