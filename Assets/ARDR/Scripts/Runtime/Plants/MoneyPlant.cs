using UnityEngine;

namespace ARDR {
	public class MoneyPlant : PlacedObject<MoneyPlantData> {

		public float _liveTime;

		private void Update() {
			_liveTime += Time.deltaTime;
		}

		public override void ApplyData(string value) {
			if (value == null) return;
			_liveTime = float.Parse(value);
		}

		public override string RecordData() {
			return _liveTime.ToString("F");
		}
	}
}
