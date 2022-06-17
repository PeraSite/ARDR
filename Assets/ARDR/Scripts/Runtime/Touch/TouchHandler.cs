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
			var touchListener = GetTouchListener(finger);
			touchListener?.OnLongTouch();
		}

		private void OnFingerTap(LeanFinger finger) {
			var touchListener = GetTouchListener(finger);
			touchListener?.OnTouch();
		}

		private ITouchListener GetTouchListener(LeanFinger finger) {
			if (finger.StartedOverGui) return default;
			var result = ScreenQuery.Query<ITouchListener>(gameObject, finger.ScreenPosition);
			return result;
		}
	}
}
