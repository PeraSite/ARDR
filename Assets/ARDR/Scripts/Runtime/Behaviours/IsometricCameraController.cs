using DG.Tweening;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class IsometricCameraController : MonoBehaviour {
		[Header("변수")]
		public BoolVariable IsDraggingObject;

		public Transform Transform;
		public Vector3 FocusOffset = new Vector3(14.5f, 0, 14.5f);
		public float lerpSpeed = 20f;

		private Vector3 _touchStart;

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
			var targetPosition = Transform.position + direction;
			Transform.position = Vector3.Lerp(
				Transform.position,
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

		[Button]
		public void FocusTo(Transform target) {
			var newPos = target.position - FocusOffset;
			newPos.y = 3f;
			Transform.position = newPos;
		}

		public void Rotate(float amount = 90f) {
			transform.DOKill(true);
			transform.DORotate(transform.localEulerAngles + new Vector3(0, amount, 0f), 0.5f);
		}
	}
}
