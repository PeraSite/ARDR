using UnityEngine;

namespace ARDR {
	public class SunOrbit : MonoBehaviour {
		public float OrbitSpeed;

		private Transform _transform;
		public Vector3 Rotation;

		private void Start() {
			_transform = GetComponent<Transform>();
			Rotation = transform.eulerAngles;
		}

		private void Update() {
			Rotation.x = Mathf.Repeat(Rotation.x + OrbitSpeed * Time.deltaTime, 360);
			_transform.eulerAngles = Rotation;
		}
	}
}
