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

		[Header("토템 기능")]
		public int CooldownTime;
		public int ActiveTime;

		[Header("오디오")]
		public SoundEffectSO TouchSFX;
		public SoundEffectSO UseTotemSFX;

		private Buff _buff;

		private void Awake() {
			UpgradeLevel.Changed.Register(OnUpgradeLevelChanged);
			_buff = GetComponent<Buff>();
			_buff.OnEffectActivate += OnActivateEffect;
			_buff.OnEffectDeactivate += OnDeactivateEffect;
			_buff.OnCooldown += OnCooldown;
		}

		private void OnDisable() {
			UpgradeLevel.Changed.Unregister(OnUpgradeLevelChanged);
			_buff.OnEffectActivate -= OnActivateEffect;
			_buff.OnEffectDeactivate -= OnDeactivateEffect;
			_buff.OnCooldown -= OnCooldown;
		}

		private void OnUpgradeLevelChanged(int newLevel) {
			Destroy(CurrentVFX);
			CurrentVFX = Instantiate(CurrentUpgrade.Model, ParentVFX);
			CurrentVFX.transform.localPosition = Vector3.zero;
			MoneyPerTouch.Value = CurrentUpgrade.MoneyPerTouch;
		}

		public void OnTouch() {
			TouchSFX.Play();
			Money.Add((long) (MoneyPerTouch.Value * MoneyPerTouchMultiplier.Value));
		}

		public void OnLongTouch() {
			_buff.StartBuff(CooldownTime, ActiveTime);
		}

		private void OnCooldown() {
			Toast.Show("아직 재사용 대기 시간입니다.");
		}

		private void OnActivateEffect() {
			MoneyPerTouchMultiplier.Value = 2f;
			Toast.Show($"세계수 토템을 사용했습니다!");
			UseTotemSFX.Play();
		}

		private void OnDeactivateEffect() {
			MoneyPerTouchMultiplier.Value = 1f;
		}
	}
}
