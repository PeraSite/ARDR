using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class MoneyPlant : PlacedObject<MoneyPlantData> {
		public IntVariable Money;

		private float _timer;

		private void Update() {
			if (!IsInitialized) return;
			if (IsEditing) return;

			_timer += Time.deltaTime;
			if (_timer >= Data.GiveDelay) {
				Tick();
				_timer = 0f;
			}
		}

		private void Tick() {
			Money.Add(Data.MoneyAmount);
		}
	}
}
