using System.Collections.Generic;
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
		public Dictionary<ThemeType, FloatVariable> ThemeMultiplier = new();

		[Header("설정")]
		public float MoneyTickCycle = 1f;

		public float StateTickCycle = 5f;

		public GridData GridData;

		private float _moneyTimer;
		private float _stateTimer;

		private void Start() {
			GridData.onAnyGridUpdate -= CalculateMoneyPerSecond;
			GridData.onAnyGridUpdate += CalculateMoneyPerSecond;

			ThemeMultiplier.ForEach(pair => pair.Value.Changed.Register(CalculateMoneyPerSecond));
			CalculateMoneyPerSecond();
		}

		private void OnDisable() {
			GridData.onAnyGridUpdate -= CalculateMoneyPerSecond;
			ThemeMultiplier.ForEach(pair => pair.Value.Changed.Unregister(CalculateMoneyPerSecond));
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
			Money.Add(MoneyPerSecond.Value);
		}

		private void CalculateMoneyPerSecond() {
			MoneyPerSecond.Value = (int) FindObjectsOfType<Plant>()
				.Where(plant => !plant.IsEditing)
				.Sum(plant => plant.Data.MoneyAmount * plant.Data.correctionValue[plant.Chunk.Theme] * ThemeMultiplier[plant.Chunk.Theme].Value);
		}
	}
}
