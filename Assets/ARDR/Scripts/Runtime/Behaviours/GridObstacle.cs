using UnityEngine;

namespace ARDR {
	public class GridObstacle : GridObjectBase, ITouchListener {

		public void OnTouch() {
			// Destroy(gameObject);
			Debug.Log("tap!" + gameObject.name);

		}

		public void OnLongTouch() {
			Debug.Log("long touched!" + gameObject.name);
		}
	}
}
