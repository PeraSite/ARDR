using PeraCore.Runtime;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class MoneyPlantSystem : MonoSingleton<MoneyPlantSystem> {
		[Header("변수")]
		public IntVariable Money;

		public IntVariable MoneyPerSecond;

		[Header("설정")]
		public float TickTime;

		private float _timer;

		private void Update() {
			_timer += Time.deltaTime;
			if (_timer >= TickTime) {
				_timer = 0;
				OnTick();
			}
		}

		private void OnTick() {
			Money.Add(MoneyPerSecond.Value);
		}
	}
}
