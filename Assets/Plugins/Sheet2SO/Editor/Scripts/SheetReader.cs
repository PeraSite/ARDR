using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using PeraCore.Editor;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class SheetReader : CustomScriptableObject {
	[FoldoutGroup("Credentials")]
	public string ServiceAccountID;

	[FoldoutGroup("Credentials")]
	public string PrivateKey;

	[HideReferenceObjectPicker]
	public List<Sheet> Sheets = new();

	[Button(DrawResult = false)]
	public async UniTaskVoid Refresh() {
		SheetAPI.InitService(ServiceAccountID, PrivateKey);

		foreach (var sheetMeta in Sheets) {
			Debug.Log("Fetching data from " + sheetMeta.Id);
			var request = SheetAPI.Service.Spreadsheets.Get(sheetMeta.Id);
			request.IncludeGridData = true;
			var response = await request.ExecuteAsync();
			Debug.Log("Generating scriptable object...");
			foreach (var worksheet in response.Sheets) {
				var worksheetMeta =
					sheetMeta.Worksheets.FirstOrDefault(meta => meta.Name == worksheet.Properties.Title);
				if (worksheetMeta == default) continue;

				var grid = worksheet.Data.SelectMany(data =>
						data.RowData
							.Skip(worksheetMeta.SkipRow)
							.Select(row => row.Values
								.Select(cell => cell.FormattedValue)
								.Where(cellText => !cellText.IsNullOrWhitespace()).ToList()
							).Where(row => !row.IsNullOrEmpty()))
					.ToList();

				//기존에 있는거면 인스턴스 찾아서 넘기기
				//없는거면 새로 만들어서 넘기기

				//Deserialize에서는 주어진 인스턴스의 값을 row 데이터로 채우기
				foreach (var row in grid) {
					var serializer = worksheetMeta.Serializer;
					var assetName = serializer.GetName(row);
					var assetPath = $"{sheetMeta.Path}/{worksheetMeta.PathName}/{assetName}.asset";

					if (!File.Exists(assetPath)) { //존재하지 않는다면
						var newInstance = CreateInstance(serializer.Type);
						newInstance.name = assetName;
						newInstance.CreateAsset(assetPath);
						Debug.Log("Creating new asset at:" + assetPath);
					}
					var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
					serializer.DeserializeWeak(row, asset);
					EditorUtility.SetDirty(asset);
				}
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log("Done!");
		}
	}
}
