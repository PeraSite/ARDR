using System;
using ARDR;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;

public class GameEditorWindow : OdinMenuEditorWindow {
	[MenuItem("Tools/데이터베이스")]
	private static void OpenWindow() {
		var window = GetWindow<GameEditorWindow>();
		window.name = "데이터베이스";
		window.Show();
	}

	private const string RESOURCE_PATH = "Assets/ARDR/Data/";

	protected override OdinMenuTree BuildMenuTree() {
		var tree = new OdinMenuTree {
			{"식물", null, EditorIcons.Tree},
		};

		void AddAssets(string menuName, string pathName, Type type) {
			tree.AddAllAssetsAtPath(menuName, RESOURCE_PATH + pathName, type, true);
		}

		AddAssets("식물", "Plants", typeof(PlaceableObjectData));

		return tree;
	}
}
