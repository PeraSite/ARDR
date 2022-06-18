using UnityEngine;

namespace ARDR {
	public class SunOrbit : MonoBehaviour {
		public float OrbitSpeed;

		private Transform _transform;
		private Vector3 Rotation;

		private void Awake() {
			_transform = GetComponent<Transform>();
			Rotation = transform.eulerAngles;
		}

		private void Update() {
			Rotation.x += OrbitSpeed * Time.deltaTime;
			_transform.eulerAngles = Rotation;
		}
	}
}
