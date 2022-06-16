using System.Collections.Generic;
using ARDR;
using Sheet2SO.Editor;
using UnityEngine;

public class WorldTreeUpgradeDataSerializer : SheetSerializer<WorldTreeUpgradeData> {
	public List<GameObject> Models;

	public override string GetName(List<string> row) => $"세계수 {row[0]}레벨";

	public override void Deserialize(List<string> row, WorldTreeUpgradeData plant) {
		plant.Level = int.Parse(row[0]);
		plant.Model = Models[int.Parse(row[1])-1];
		plant.MoneyPerTouch = int.Parse(row[2]);
		plant.MaxChunk = int.Parse(row[3]);
		plant.LevelupPrice = long.Parse(row[4]);
	}
}
