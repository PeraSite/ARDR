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

	[EnableIf("@UnityEngine.Application.isPlaying")]
	[Button]
	public void DeleteSlot(int slot) {
		SaveSystem.DeleteSavedGameInSlot(slot);
	}

	[Button]
	public void PrintSave(int slot) {
		Debug.Log(PlayerPrefs.GetString($"Save{slot}"));
	}

	[Button]
	public SerializedSavedGameData InspectSave(int slot) {
		var rawSavedData = PlayerPrefs.GetString("Save" + slot);
		var bytes = Encoding.UTF8.GetBytes(rawSavedData);
		var data = SerializationUtility.DeserializeValue<SavedGameData>(bytes, DataFormat.JSON);
		if (data == null) return null;
		var serializedData = MakeSerialized(data);
		return serializedData;
	}

	[ShowIf("@PixelCrushers.SaveSystem.hasInstance")]
	[OdinSerialize]
	public SerializedSavedGameData Current {
		get => SaveSystem.hasInstance ? MakeSerialized(SaveSystem.currentSavedGameData) : null;
		set {
			if (SaveSystem.hasInstance) {
				SaveSystem.currentSavedGameData = value.original;
			}
		}
	}

	private static SerializedSavedGameData MakeSerialized(SavedGameData data) {
		return new SerializedSavedGameData {
			original = data,
			version = data.version,
			sceneName = data.sceneName,
			Saved = data.GetAllData().Select(pair => new SerializedSavedGameData.SerializedSaveRecord {
				key = pair.Value.key,
				sceneIndex = pair.Value.sceneIndex,
				data = SerializationUtility.DeserializeValueWeak(Encoding.UTF8.GetBytes(pair.Value.data),
					DataFormat.JSON)
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
