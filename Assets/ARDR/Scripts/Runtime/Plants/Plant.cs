using PixelCrushers;
using Sirenix.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class Plant : PlacedObject<PlantData>, ITouchListener {
		public IntVariable MoneyPerSecond;

		public PlantState State;

		public override void OnInit() {
			base.OnInit();
			Debug.Log("Adding " + Data.MoneyAmount);
			MoneyPerSecond.Add(Data.MoneyAmount);
			State = new PlantState {
				Moisture = Random.Range(0, 100),
				Nutrition = Random.Range(0, 100),
			};
		}

		public override void OnRemove() {
			if (Data.SafeIsUnityNull()) return;
			Debug.Log("Destroy");
			MoneyPerSecond.Subtract(Data.MoneyAmount);
		}

		public void OnTouch() {
			PlantInfoPopup.Instance.Show(this);
		}

		public void OnLongTouch() {
			Debug.Log("longtouch");
			GridEditSystem.Instance.SetExistObjectEditMode(this);
		}

		public void OnStateTick() {
			State.Moisture = Mathf.Clamp(State.Moisture - Data.MoistureUsage, 0, 100);
			State.Nutrition = Mathf.Clamp(State.Nutrition - Data.NutritionUsage, 0, 100);
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
