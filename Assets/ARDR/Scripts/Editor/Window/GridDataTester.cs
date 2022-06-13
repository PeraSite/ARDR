using System.Text;
using ARDR;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

public class GridDataTester : OdinEditorWindow {
	[MenuItem("Tools/GridData Test")]
	private static void OpenWindow() {
		var window = GetWindow<GridDataTester>();
		window.Show();
	}

	public GridData Data;

	[Button]
	private void Serialize() {
		var message = Encoding.UTF8.GetString(SerializationUtility.SerializeValue(Data, DataFormat.JSON));
		Debug.Log(message);
	}

	[Button]
	private GridData DesSerialize(string json) {
		var bytes = Encoding.UTF8.GetBytes(json);
		var value = SerializationUtility.DeserializeValue<GridData>(bytes, DataFormat.JSON);
		return value;
	}
}
