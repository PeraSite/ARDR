using Sirenix.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class MoneyPlant : PlacedObject<PlantData>, ITouchListener {
		public IntVariable MoneyPerSecond;

		public override void OnInit() {
			base.OnInit();
			Debug.Log("Adding " + Data.MoneyAmount);
			MoneyPerSecond.Add(Data.MoneyAmount);
		}

		public override void OnRemove() {
			if (Data.SafeIsUnityNull()) return;
			Debug.Log("Destroy");
			MoneyPerSecond.Subtract(Data.MoneyAmount);
		}

		public void OnTouch() {
			Debug.Log("tap");
		}

		public void OnLongTouch() {
			GridEditSystem.Instance.SetExistObjectEditMode(this);
		}
	}
}
