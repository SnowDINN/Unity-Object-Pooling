using Anonymous.Pooling;
using UnityEditor;

[CustomEditor(typeof(Installer))]
public class InstallerEditor : Editor
{
	private Installer installer;

	private void OnEnable()
	{
		installer = target as Installer;
	}
	
	
	[MenuItem("Utilities/On Select/Pool Setting", false, 101)]
	public static void OnSelectPoolSetting()
	{
		Selection.activeObject =
			AssetDatabase.LoadMainAssetAtPath(
				"Assets/Utilities/Object Pool/Scripts/Installer/Resources/Object Pooling/Installer.asset");
		EditorGUIUtility.PingObject(Selection.activeObject);
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	}
}