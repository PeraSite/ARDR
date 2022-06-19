using System;
using UnityEngine;

namespace ARDR {
	public class Totem : GridObjectBase, ITouchListener {
		[field: SerializeField]
		public override PlaceableObjectData BaseData { get; set; }

		[field: SerializeField]
		public override Direction Direction { get; set; }

		public TotemState State;

		public int CooldownTime;
		public int ActiveTime;

		public void OnTouch() {
			var current = DateTimeOffset.Now.ToUnixTimeSeconds();
			var diff = current - State.lastUsed;
			if (diff < CooldownTime) {
				Toast.Show("토템 재사용 대기 시간입니다.");
				return;
			}
			State.lastUsed = current;
			Toast.Show($"{name} 사용 완료!");
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
