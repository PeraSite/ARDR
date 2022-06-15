using Lean.Common;
using Lean.Touch;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class IsometricCameraController : MonoBehaviour {
		[Header("변수")]
		public BoolVariable IsDraggingObject;

		public float lerpSpeed = 20f;

		private LeanScreenQuery ScreenQuery = new(LeanScreenQuery.MethodType.Raycast);
		private Camera _cam;
		private Vector3 _touchStart;

		private void Awake() {
			_cam = GetComponent<Camera>();
		}

		private void OnEnable() {
			LeanTouch.OnFingerDown += OnFingerDown;
			LeanTouch.OnFingerUpdate += OnFingerUpdate;
			LeanTouch.OnFingerUp += OnFingerUp;
		}

		private void OnDisable() {
			LeanTouch.OnFingerDown -= OnFingerDown;
			LeanTouch.OnFingerUpdate -= OnFingerUpdate;
			LeanTouch.OnFingerUp -= OnFingerUp;
		}

		private void OnFingerDown(LeanFinger finger) {
			if (IsDraggingObject.Value) return;
			if (finger.IsOverGui) return;
			_touchStart = GetWorldPosition(finger);
		}

		private void OnFingerUpdate(LeanFinger finger) {
			if (_touchStart == Vector3.zero) return;
			var direction = _touchStart - GetWorldPosition(finger);
			var targetPosition = _cam.transform.position + direction;
			_cam.transform.position = Vector3.Lerp(
				_cam.transform.position,
				targetPosition,
				Time.deltaTime * lerpSpeed
			);
		}

		private void OnFingerUp(LeanFinger finger) {
			_touchStart = Vector3.zero;
		}

		private Vector3 GetWorldPosition(LeanFinger finger) {
			var ground = new Plane(Vector3.up, new Vector3(0, 0, 0));
			var ray = finger.GetRay();
			ground.Raycast(ray, out var distance);
			return ray.GetPoint(distance);
		}
	}
}
