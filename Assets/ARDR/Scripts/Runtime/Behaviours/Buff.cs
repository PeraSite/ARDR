using System;
using System.Collections;
using PixelCrushers;
using UnityEngine;

namespace ARDR {
	public class Buff : Saver, ITouchListener {
		[Header("상태")]
		public BuffState State;

		public event Action OnCooldown;
		public event Action OnEffectActivate;
		public event Action OnEffectDeactivate;

		public override void Start() {
			base.Start();
			if (State.IsActive) {
				StartCoroutine(TotemUseCoroutine());
			}
		}

		public void StartBuff(long CooldownTime, long ActiveTime) {
			var current = DateTimeOffset.Now.ToUnixTimeSeconds();
			var diff = current - State.lastUsed;
			if (diff < CooldownTime) {
				OnCooldown?.Invoke();
				return;
			}
			State.lastUsed = current;
			State.effectEnd = current + ActiveTime;
			State.IsActive = true;
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
			State.IsActive = false;
		}

		private void ActivateEffect() {
			OnEffectActivate?.Invoke();
		}

		private void DeactivateEffect() {
			OnEffectDeactivate?.Invoke();
		}

#region Serialization

		public override string RecordData() {
			return SaveSystem.Serialize(State);
		}

		public override void ApplyData(string s) {
			if (s == null) return;
			State = SaveSystem.Deserialize<BuffState>(s);
		}

#endregion
	}

	[Serializable]
	public struct BuffState {
		public long lastUsed;
		public bool IsActive;
		public long effectEnd;
	}
}
