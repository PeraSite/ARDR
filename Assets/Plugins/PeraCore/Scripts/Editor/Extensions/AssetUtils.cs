using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace PeraCore.Editor {
	public static class AssetUtils {
		public static List<T> FindAssetsByType<T>() where T : Object {
			var assets = new List<T>();
			var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
			foreach (var guid in guids) {
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
				if (asset != null) {
					assets.Add(asset);
				}
			}
			return assets;
		}

		public static void CreateAsset<TObject>(this TObject asset, string path) where TObject : Object {
			AssetDatabase.CreateAsset(asset, path);
			AssetDatabase.SaveAssets();

			Undo.RegisterCreatedObjectUndo(asset, "Add new object");
		}

		public static void AddSubObject<TObject>(this Object parent, TObject subObject) where TObject : Object {
			var parentPath = AssetDatabase.GetAssetPath(parent);

			AssetDatabase.AddObjectToAsset(subObject, parentPath);
			AssetDatabase.SaveAssets();

			Undo.RegisterCreatedObjectUndo(subObject, "Add new object to parent");
		}

		public static void DeleteObject(this Object subObject) {
			Undo.DestroyObjectImmediate(subObject);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}
