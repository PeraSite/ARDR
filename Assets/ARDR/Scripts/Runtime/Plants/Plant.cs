using System.Linq;
using PeraCore.Runtime;
using PixelCrushers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class Plant : PlacedObject<PlantData>, ITouchListener {
		[Header("상태")]
		public PlantState State;

		[Header("오브젝트")]
		public GameObject BuildingVisual;

		public Canvas StateCanvas;

		public GameObject MoistureObject;
		public GameObject NutritionObject;

		[Header("설정")]
		public float MoistureNotificationTolerance = 20f;

		public float NutritionNotificationTolerance = 20f;

		public ScriptableObjectCache SOCache;

		[Header("변수")]
		public IntVariable WaterCanLevel;

		public IntVariable FertilizerLevel;

		private BoxCollider _collider;

		private void Awake() {
			_collider = GetComponent<BoxCollider>();
		}

		public override void OnFirstPlaced() {
			base.OnFirstPlaced();
			State = new PlantState {
				Moisture = Random.Range(50, 100),
				Nutrition = Random.Range(50, 100),
			};
		}

		public override void OnInstantiated(PlaceableObjectData data) {
			var plantData = (PlantData) data;
			var center = Vector3.one * Chunk.cellSize / 2 * data.gridSize.x;
			center.y -= 1;
			_collider.center = center;
			var size = Vector3.one * Chunk.cellSize * data.gridSize.x;
			size.y -= 1f;
			size *= 0.5f;
			_collider.size = size;

			StateCanvas.transform.localPosition = center + new Vector3(0, plantData.PlantHeight, 0);
			StateCanvas.enabled = false;

			var plant = Instantiate(plantData.PlantModel, transform);
			var position = Vector3.one * Chunk.cellSize / 2 * data.gridSize.x;
			position.y = 0f;
			BuildingVisual.transform.localScale = new Vector3(data.gridSize.x, 1, data.gridSize.y);
			plant.transform.localPosition = position;
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

		[Button]
		public void OnStateTick() {
			State.Moisture = Mathf.Max(State.Moisture - Data.MoistureUsage, 0);
			State.Nutrition = Mathf.Max(State.Nutrition - Data.NutritionUsage, 0);
			UpdateCanvas();
		}

		private void UpdateCanvas() {
			var shouldShowMoistureNotification = State.Moisture <= MoistureNotificationTolerance;
			var shouldShowNutritionNotification = State.Nutrition <= NutritionNotificationTolerance;

			if (shouldShowMoistureNotification || shouldShowNutritionNotification) {
				StateCanvas.enabled = true;
				MoistureObject.SetActive(shouldShowMoistureNotification);
				NutritionObject.SetActive(shouldShowNutritionNotification);
			} else {
				StateCanvas.enabled = false;
			}
		}

		public void GiveWater() {
			var upgradeData = SOCache.Find<ItemUpgradeData>().First(data => data.Level == WaterCanLevel.Value);
			FindObjectsOfType<Plant>().ForEach(p => {
				var distance =
					Mathf.CeilToInt(Vector3.Distance(p.transform.position, transform.position) / Chunk.cellSize);
				if (distance <= upgradeData.Range.x) {
					p.State.Moisture += upgradeData.AddAmount;
					p.UpdateCanvas();
				}
			});
		}

		public void GiveFertilizer() {
			var upgradeData = SOCache.Find<ItemUpgradeData>().First(data => data.Level == FertilizerLevel.Value);
			FindObjectsOfType<Plant>().ForEach(p => {
				var distance =
					Mathf.CeilToInt(Vector3.Distance(p.transform.position, transform.position) / Chunk.cellSize);
				if (distance <= upgradeData.Range.x) {
					p.State.Nutrition += upgradeData.AddAmount;
					p.UpdateCanvas();
				}
			});
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
			UpdateCanvas();
		}

#endregion
	}
}
