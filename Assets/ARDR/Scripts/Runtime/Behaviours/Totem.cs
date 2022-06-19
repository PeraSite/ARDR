using System;
using System.Collections;
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

		[Header("상태")]
		public TotemState State;

		[Header("설정")]
		public int CooldownTime;

		public int ActiveTime;

		private void Start() {
			if (State.IsActive) {
				StartCoroutine(TotemUseCoroutine());
			}
		}

		public void OnTouch() {
			var current = DateTimeOffset.Now.ToUnixTimeSeconds();
			var diff = current - State.lastUsed;
			if (diff < CooldownTime) {
				Toast.Show("토템 재사용 대기 시간입니다.");
				return;
			}
			State.lastUsed = current;
			State.effectEnd = current + ActiveTime;
			ActivateEffect();
			StartCoroutine(TotemUseCoroutine());
		}

		private IEnumerator TotemUseCoroutine() {
			yield return new WaitUntil(() => {
				var current = DateTimeOffset.Now.ToUnixTimeSeconds();
				var target = State.effectEnd;
				return current > target;
			});
			DeactivateEffect();
		}

		private void ActivateEffect() {
			State.IsActive = true;
			Multiplier.Value = 2f;
			Toast.Show($"{name}을 사용했습니다!");
		}

		private void DeactivateEffect() {
			State.IsActive = false;
			Multiplier.Value = 1f;
		}

		public override void OnDiscovered() {
			Toast.Show($"{name}을 발견했습니다!");
		}
	}

	[Serializable]
	public struct TotemState {
		public long lastUsed;
		public bool IsActive;
		public long effectEnd;
	}
}
