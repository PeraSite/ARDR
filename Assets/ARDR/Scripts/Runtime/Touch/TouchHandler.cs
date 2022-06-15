using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace ARDR {
	public class TouchHandler : MonoBehaviour {
		public LeanScreenQuery ScreenQuery = new(LeanScreenQuery.MethodType.Raycast);

		private void OnEnable() {
			LeanTouch.OnFingerTap += OnFingerTap;
			LeanTouch.OnFingerOld += OnFingerOld;
		}

		private void OnDisable() {
			LeanTouch.OnFingerTap -= OnFingerTap;
			LeanTouch.OnFingerOld -= OnFingerOld;
		}

		private void OnFingerOld(LeanFinger finger) {
			if (finger.IsOverGui) return;
			var result = ScreenQuery.Query<ITouchListener>(gameObject, finger.ScreenPosition);
			result?.OnLongTouch();
		}

		private void OnFingerTap(LeanFinger finger) {
			if (finger.IsOverGui) return;
			var result = ScreenQuery.Query<ITouchListener>(gameObject, finger.ScreenPosition);
			result?.OnTouch();
		}
	}
}
