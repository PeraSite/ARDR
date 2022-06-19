using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class Totem : GridObjectBase, ITouchListener {
		[field: SerializeField]
		public override PlaceableObjectData BaseData { get; set; }

		[field: SerializeField]
		public override Direction Direction { get; set; }

		[Header("변수")]
		public FloatVariable Multiplier;

		[Header("설정")]
		public ThemeType Theme;

		public int CooldownTime;
		public int ActiveTime;
		public float Power = 2f;

		private Buff _buff;

		private void Awake() {
			_buff = GetComponent<Buff>();
			_buff.OnEffectActivate += OnActivateEffect;
			_buff.OnEffectDeactivate += OnDeactivateEffect;
			_buff.OnCooldown += OnCooldown;
		}

		private void OnDisable() {
			_buff.OnEffectActivate -= OnActivateEffect;
			_buff.OnEffectDeactivate -= OnDeactivateEffect;
			_buff.OnCooldown -= OnCooldown;
		}

		public void OnLongTouch() {
			_buff.StartBuff(CooldownTime, ActiveTime);
		}

		private void OnCooldown() {
			Toast.Show("아직 재사용 대기 시간입니다.");
		}

		private void OnActivateEffect() {
			Multiplier.Value = Power;
			Toast.Show($"{name}을 사용했습니다!");
		}

		private void OnDeactivateEffect() {
			Multiplier.Value = 1f;
		}

		public override void OnDiscovered() {
			Toast.Show($"{name}을 발견했습니다!");
			Chunk.gridData.InvokeAnyGridUpdate();
		}
	}
}
