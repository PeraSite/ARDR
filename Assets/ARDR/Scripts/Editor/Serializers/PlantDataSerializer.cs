using System;
using System.Collections.Generic;
using System.Linq;
using ARDR;
using PeraCore.Runtime;
using Sheet2SO.Editor;
using UnityEngine;

public class PlantDataSerializer : SheetSerializer<PlantData> {
	public ScriptableObjectCache SOCache;
	public List<Sprite> Icons = new();
	public List<GameObject> PlantModels = new();

	public override string GetName(List<string> row) => row[1];

	public override void Deserialize(List<string> row, PlantData plant) {
		//식물 이름
		plant.Name = plant.name;
		
		//아이콘
		plant.Display = Icons.First(icon => icon.name == plant.name);
		
		//식물 구매 비용
		plant.Price = int.Parse(row[2]);
		
		//식물의 종류
		plant.Type = SOCache.Find<PlantType>().First(plantType => plantType.Name == row[3]);

		//식물의 크기
		var gridSizeData = row[4].Split("x").Select(int.Parse).ToList();
		plant.gridSize = new Vector2Int {x = gridSizeData[0], y = gridSizeData[1]};

		//초당 재화 수급량
		plant.MoneyAmount = int.Parse(row[5]);

		//필요 세계수 레벨
		plant.RequireLevel = int.Parse(row[6]);

		//보정값
		plant.correctionValue[ThemeType.숲] = double.Parse(row[7]);
		plant.correctionValue[ThemeType.늪] = double.Parse(row[8]);
		plant.correctionValue[ThemeType.사막] = double.Parse(row[9]);
		plant.correctionValue[ThemeType.정글] = double.Parse(row[10]);
		plant.correctionValue[ThemeType.타이가] = double.Parse(row[11]);

		//5분 당 양분 소모량
		plant.NutritionUsage = int.Parse(row[12]);
		
		//5분 당 수분 소모량
		plant.MoistureUsage = int.Parse(row[13]);

		//설치 테마
		if (Enum.TryParse<ThemeType>(row[14], out var themeType)) {
			plant.Theme = themeType;
		}

		plant.Description = row[15];

		plant.PlantModel = PlantModels.Find(go => go.name == plant.name);

	}
}
