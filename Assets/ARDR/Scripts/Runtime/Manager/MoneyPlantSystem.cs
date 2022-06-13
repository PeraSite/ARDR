using System.Linq;
using PeraCore.Runtime;
using Sirenix.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class MoneyPlantSystem : MonoSingleton<MoneyPlantSystem> {
		[Header("변수")]
		public IntVariable Money;

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
			FindObjectsOfType<MoneyPlant>()
				.Where(plant => !plant.Data.SafeIsUnityNull())
				.ForEach(plant => Money.Add(plant.Data.MoneyAmount));
		}
	}
}
