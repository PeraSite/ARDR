using System.Collections.Generic;
using PixelCrushers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class Plant : PlacedObject<PlantData>, ITouchListener {
		[Header("설정")]
		public IntVariable MoneyPerSecond;

		public PlantState State;

		public GameObject BuildingVisual;

		private BoxCollider _collider;

		private void Awake() {
			_collider = GetComponent<BoxCollider>();
		}

		public override void OnFirstPlaced() {
			base.OnFirstPlaced();
			MoneyPerSecond.Add(Data.MoneyAmount);
			State = new PlantState {
				Moisture = Random.Range(0, 100),
				Nutrition = Random.Range(0, 100),
			};
		}

		public override void OnInstantiated(PlaceableObjectData data) {
			var plantData = (PlantData) data;
			var center = Vector3.one * Chunk.cellSize / 2 * data.gridSize.x;
			center.y -= 1;
			_collider.center = center;
			var size = Vector3.one * Chunk.cellSize * data.gridSize.x;
			size.y -= 1f;
			size *= 0.7f;
			_collider.size = size;

			var plant = Instantiate(plantData.PlantModel, transform);
			var position = Vector3.one * Chunk.cellSize / 2 * data.gridSize.x;
			position.y = 0f;
			BuildingVisual.transform.localScale = new Vector3(data.gridSize.x, 1, data.gridSize.y);
			plant.transform.localPosition = position;
		}

		public override void OnRemove() {
			if (Data.SafeIsUnityNull()) return;
			MoneyPerSecond.Subtract(Data.MoneyAmount);
		}

		public override void OnEditStart() {
			BuildingVisual.SetActive(true);
		}

		public override void OnEditEnd() {
			BuildingVisual.SetActive(false);
		}

		public void OnTouch() {
			PlantInfoPopup.Instance.Show(this);
		}

		public void OnLongTouch() {
			GridEditSystem.Instance.SetExistObjectEditMode(this);
		}

		public void OnStateTick() {
			State.Moisture = Mathf.Clamp(State.Moisture - Data.MoistureUsage, 0, 100);
			State.Nutrition = Mathf.Clamp(State.Nutrition - Data.NutritionUsage, 0, 100);
		}

		private List<Vector3> WorldPositions = new();

		[Button]
		private void TestPosition() {
			WorldPositions.Clear();
			var originCellPos = GridSystem.Instance.GridData.GetCellPos(transform.position);
			var list = Data.GetGridPositionList(originCellPos, Direction);
			list.ForEach(cellPos => {
				var worldPosition = GridSystem.Instance.GridData.GetWorldPosition(cellPos).GetValueOrDefault();
				Debug.Log($"Cellpos : {cellPos}, world position: {worldPosition}");
				WorldPositions.Add(worldPosition);
			});
		}

		private void OnDrawGizmos() {
			WorldPositions.ForEach(wp => Gizmos.DrawSphere(wp, 1f));
		}

#region Serialziation

		public struct PlantState {
			public int Nutrition;
			public int Moisture;
		}

		public override string RecordData() {
			return SaveSystem.Serialize(State);
		}

		public override void ApplyData(string data) {
			if (data.IsNullOrWhitespace()) return;
			State = SaveSystem.Deserialize<PlantState>(data);
		}

#endregion
	}
}
