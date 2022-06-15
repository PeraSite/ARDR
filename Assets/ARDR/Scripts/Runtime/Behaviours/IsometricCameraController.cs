using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ARDR {
	public class IsometricCameraController : MonoBehaviour {
		[Header("변수")]
		public BoolVariable IsDragging;

		[Header("설정")]
		public float groundZ;

		public float lerpSpeed;

		private Vector3 _touchStart;
		private float _startZ;
		private Camera _cam;

		private void Awake() {
			_startZ = transform.position.z;
			_cam = Camera.main;
		}

		private void Update() {
			if (IsDragging.Value) return;

			var isHoveringUI = EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId);
			if (isHoveringUI) {
				_touchStart = Vector3.zero;
				return;
			}

			if (Input.GetMouseButtonDown(0)) {
				_touchStart = GetWorldPosition(groundZ);
			}
			if (Input.GetMouseButton(0)) {
				if (_touchStart == Vector3.zero)
					return;
				var direction = _touchStart - GetWorldPosition(groundZ);
				var targetPosition = _cam.transform.position + direction;
				targetPosition.z = _startZ;
				_cam.transform.position = Vector3.Lerp(
					_cam.transform.position,
					targetPosition,
					Time.deltaTime * lerpSpeed
				);
			}
		}

		private Vector3 GetWorldPosition(float z) {
			var mousePos = _cam.ScreenPointToRay(Input.mousePosition);
			var ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
			ground.Raycast(mousePos, out var distance);
			return mousePos.GetPoint(distance);
		}
	}
}
