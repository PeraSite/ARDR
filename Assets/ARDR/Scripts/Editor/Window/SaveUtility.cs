using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PixelCrushers;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

public class SaveUtility : OdinEditorWindow {
	private static readonly DeserializationContext context = new() {
		StringReferenceResolver = new ScriptableObjectStringReferenceResolver()
	};

	[MenuItem("Tools/Save Utility")]
	private static void ShowWindow() {
		GetWindow<SaveUtility>().Show();
	}

	[EnableIf("@UnityEngine.Application.isPlaying")]
	[Button]
	public void LoadSlot(int slot) {
		SaveSystem.LoadFromSlot(slot);
	}

	[EnableIf("@UnityEngine.Application.isPlaying")]
	[Button]
	public void SaveToSlot(int slot) {
		SaveSystem.SaveToSlot(slot);
	}

	[Button]
	public void DeleteSlot(int slot) {
		PlayerPrefs.DeleteKey(GetPlayerPrefsKey(slot));
		PlayerPrefs.Save();
	}

	[Button]
	public void PrintSave(int slot) {
		Debug.Log(PlayerPrefs.GetString(GetPlayerPrefsKey(slot)));
	}

	private string GetPlayerPrefsKey(int slotNumber) => "Save" + slotNumber;

	[Button]
	public SerializedSavedGameData InspectSave(int slot) {
		var rawSavedData = PlayerPrefs.GetString(GetPlayerPrefsKey(slot));
		var bytes = Encoding.UTF8.GetBytes(rawSavedData);
		var data = SerializationUtility.DeserializeValue<SavedGameData>(bytes, DataFormat.JSON, context);
		if (data == null) return null;
		var serializedData = MakeSerialized(data);
		return serializedData;
	}

	[ShowIf("@PixelCrushers.SaveSystem.hasInstance")]
	[OdinSerialize]
	public SerializedSavedGameData Current {
		get => SaveSystem.hasInstance ? MakeSerialized(SaveSystem.currentSavedGameData) : null;
		set {
			if (SaveSystem.hasInstance && value != null) {
				SaveSystem.currentSavedGameData = value.original;
			}
		}
	}

	private static SerializedSavedGameData MakeSerialized(SavedGameData data) {
		if (data == null) return null;
		return new SerializedSavedGameData {
			original = data,
			version = data.version,
			sceneName = data.sceneName,
			Saved = data.GetAllData().Select(pair => new SerializedSavedGameData.SerializedSaveRecord {
				key = pair.Value.key,
				sceneIndex = pair.Value.sceneIndex,
				data = SerializationUtility.DeserializeValueWeak(Encoding.UTF8.GetBytes(pair.Value.data),
					DataFormat.JSON, context)
			}).ToList()
		};
	}

	[HideReferenceObjectPicker]
	public class SerializedSavedGameData {
		[NonSerialized]
		public SavedGameData original;

		public int version;

		public string sceneName;

		[HideReferenceObjectPicker]
		public List<SerializedSaveRecord> Saved = new();

		[Serializable]
		public class SerializedSaveRecord {
			public string key;

			public int sceneIndex;

			public object data;
		}
	}
}

public class ScriptableObjectStringReferenceResolver : IExternalStringReferenceResolver {
	public IExternalStringReferenceResolver NextResolver { get; set; }

	public bool CanReference(object value, out string id) {
		if (value is ScriptableObject so) {
			id = so.name;
			return true;
		}

		id = null;
		return false;
	}

	public bool TryResolveReference(string id, out object value) {
		value = null;
		foreach (var guid in AssetDatabase.FindAssets("t:scriptableobject", new[] {"Assets/ARDR/Data"})) {
			var assetPath = AssetDatabase.GUIDToAssetPath(guid);
			var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
			if (asset.name == id) {
				value = asset;
			}
		}
		return value != null;
	}
}
