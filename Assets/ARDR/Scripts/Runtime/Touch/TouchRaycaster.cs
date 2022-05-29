﻿using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace ARDR {
	public class TouchRaycaster : MonoBehaviour {
		public LeanScreenQuery ScreenQuery = new(LeanScreenQuery.MethodType.Raycast);

		public void SelectScreenPosition(LeanFinger finger) {
			SelectScreenPosition(finger, finger.StartScreenPosition);
		}

		public void SelectScreenPosition(LeanFinger finger, Vector2 screenPosition) {
			var result = ScreenQuery.Query<ITouchListener>(gameObject, screenPosition);
			result?.OnTouch();
		}
	}
}
