using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace ARDR {
	public class TouchRaycaster : MonoBehaviour {
		private void OnEnable() {
			LeanTouch.OnFingerTap += OnFingerTap;
			LeanTouch.OnFingerOld += OnFingerOld;
		}

		private void OnDisable() {
			LeanTouch.OnFingerTap -= OnFingerTap;
			LeanTouch.OnFingerOld -= OnFingerOld;
		}

		private void OnFingerOld(LeanFinger finger) {
			var result = ScreenQuery.Query<ITouchListener>(gameObject, finger.ScreenPosition);
			result?.OnLongTouch();
		}

		private void OnFingerTap(LeanFinger finger) {
			var result = ScreenQuery.Query<ITouchListener>(gameObject, finger.ScreenPosition);
			result?.OnTouch();
		}

		public LeanScreenQuery ScreenQuery = new(LeanScreenQuery.MethodType.Raycast);
	}
}
