using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class WorldTree : GridSceneObject, ITouchListener {
		[Header("변수")]
		public LongVariable Money;

		public IntVariable MoneyPerTouch;

		public FloatVariable MoneyPerTouchMultiplier;


		public IntVariable UpgradeLevel;

		public Transform ParentVFX;
		public GameObject CurrentVFX;

		[Header("업그레이드")]
		public ScriptableObjectCache SOCache;

		public WorldTreeUpgradeData CurrentUpgrade => UpgradeData.Find(data => data.Level == UpgradeLevel.Value);

		public List<WorldTreeUpgradeData> UpgradeData => SOCache.Find<WorldTreeUpgradeData>().ToList();

		private void Awake() {
			UpgradeLevel.Changed.Register(OnUpgradeLevelChanged);
		}

		private void OnDisable() {
			UpgradeLevel.Changed.Unregister(OnUpgradeLevelChanged);
		}

		private void OnUpgradeLevelChanged(int newLevel) {
			Destroy(CurrentVFX);
			CurrentVFX = Instantiate(CurrentUpgrade.Model, ParentVFX);
			CurrentVFX.transform.localPosition = Vector3.zero;
			MoneyPerTouch.Value = CurrentUpgrade.MoneyPerTouch;
		}

		public void OnTouch() {
			Money.Add((long) (MoneyPerTouch.Value * MoneyPerTouchMultiplier.Value));
		}
	}
}
