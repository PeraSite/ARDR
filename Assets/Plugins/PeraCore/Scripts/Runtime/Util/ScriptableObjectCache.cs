using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PeraCore.Runtime {
	public class ScriptableObjectCache : SerializedScriptableObject {
		public List<ScriptableObject> Objects = new();
		public string[] ScanPath = { };

		public IEnumerable<T> Find<T>() {
			return Objects.OfType<T>();
		}

#if UNITY_EDITOR

		private static ScriptableObjectCache _instance;

		private static void HandlePlayModeStateChange(PlayModeStateChange state) {
			if (state == PlayModeStateChange.ExitingEditMode) {
				_instance.UpdateCache();
			}
		}

		private void OnEnable() {
			_instance = this;
			if (EditorSettings.enterPlayModeOptionsEnabled) {
				EditorApplication.playModeStateChanged -= HandlePlayModeStateChange;
				EditorApplication.playModeStateChanged += HandlePlayModeStateChange;
			}
		}

		[Button]
		private void UpdateCache() {
			var guids = AssetDatabase.FindAssets("t:scriptableobject", ScanPath);
			Objects.Clear();
			foreach (var guid in guids) {
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
				Objects.Add(asset);
			}
		}
#endif
	}
}
