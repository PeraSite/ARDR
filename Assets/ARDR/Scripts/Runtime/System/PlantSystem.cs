using System.Linq;
using PeraCore.Runtime;
using Sirenix.Utilities;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class PlantSystem : MonoSingleton<PlantSystem> {
		[Header("변수")]
		public LongVariable Money;

		public IntVariable MoneyPerSecond;
		public FloatVariable MoneyPerSecondMultiplier;

		[Header("설정")]
		public float MoneyTickCycle = 1f;

		public float StateTickCycle = 5f;

		public GridData GridData;

		private float _moneyTimer;
		private float _stateTimer;

		private void Start() {
			GridData.onAnyGridUpdate -= CalculateMoneyPerSecond;
			GridData.onAnyGridUpdate += CalculateMoneyPerSecond;
			CalculateMoneyPerSecond();
		}

		private void OnDisable() {
			GridData.onAnyGridUpdate -= CalculateMoneyPerSecond;
		}

		private void Update() {
			UpdateMoneyCycle();
			UpdateStateCycle();
		}

		private void UpdateMoneyCycle() {
			_moneyTimer += Time.deltaTime;
			if (_moneyTimer >= MoneyTickCycle) {
				_moneyTimer = 0;
				OnMoneyTick();
			}
		}

		private void UpdateStateCycle() {
			_stateTimer += Time.deltaTime;
			if (_stateTimer >= StateTickCycle) {
				_stateTimer = 0;
				OnStateTick();
			}
		}

		private void OnStateTick() {
			foreach (var chunk in GridData.chunkGrid.GridArray) {
				chunk.Objects
					.Select(placed => placed.PlacedObject)
					.OfType<Plant>()
					.ForEach(plant => plant.OnStateTick());
			}
		}

		private void OnMoneyTick() {
			Money.Add((long) (MoneyPerSecond.Value * MoneyPerSecondMultiplier.Value));
		}

		private void CalculateMoneyPerSecond() {
			MoneyPerSecond.Value = (int) FindObjectsOfType<Plant>()
				.Where(plant => !plant.IsEditing)
				.Sum(plant => plant.Data.MoneyAmount * plant.Data.correctionValue[plant.Chunk.Theme]);
		}
	}
}
