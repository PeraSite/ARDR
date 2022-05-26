using UnityEngine;

namespace ARDR {
	public class IsometricCameraController : MonoBehaviour {
		public Camera cam;
		public float groundZ;
		public float lerpSpeed;

		private Vector3 _touchStart;

		private void Update() {
			if (Input.GetMouseButtonDown(0)) {
				_touchStart = GetWorldPosition(groundZ);
			}
			if (Input.GetMouseButton(0)) {
				var direction = _touchStart - GetWorldPosition(groundZ);
				cam.transform.position = Vector3.Lerp(
					cam.transform.position,
					cam.transform.position + direction,
					Time.deltaTime * lerpSpeed
				);
			}
		}

		private Vector3 GetWorldPosition(float z) {
			var mousePos = cam.ScreenPointToRay(Input.mousePosition);
			var ground = new Plane(Vector3.forward, new Vector3(0, 0, z));
			ground.Raycast(mousePos, out var distance);
			return mousePos.GetPoint(distance);
		}
	}
}
