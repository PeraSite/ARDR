using System.Linq;
using DG.DemiEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

public class SceneSelectorDrawer : OdinAttributeDrawer<SceneSelectorAttribute, string> {
	protected override void DrawPropertyLayout(GUIContent label) {
		var rect = EditorGUILayout.GetControlRect();
		if (label != null) {
			rect = EditorGUI.PrefixLabel(rect, label);
		}

		var selected = ValueEntry.SmartValue;

		if (GUI.Button(rect, selected, SirenixGUIStyles.DropDownMiniButton)) {
			var scenesGUIDs = AssetDatabase.FindAssets("t:scene");
			var scenesPaths = scenesGUIDs.Select(AssetDatabase.GUIDToAssetPath)
				.Where(path => path.StartsWith("Assets/"))
				.Select(path => path.Replace(".unity", "").Replace("Assets/", ""));

			var selector = new GenericSelector<string>(scenesPaths);
			selector.SelectionConfirmed += list => {
				ValueEntry.SmartValue = list.First().Split("/").Last();
			};
			selector.ShowInPopup(rect.position + new Vector2(0, 20));
		}
	}
}
