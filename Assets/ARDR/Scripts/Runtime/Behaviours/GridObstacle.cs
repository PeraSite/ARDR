using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class GridObstacle : GridObjectBase, ITouchListener {
		[field: SerializeField]
		public override PlaceableObjectData BaseData { get; set; }

		[field: SerializeField]
		public override Direction Direction { get; set; }

		public void OnTouch() {
			ObstacleRemovePopup.Instance.Open(this);
		}

		public override void OnDiscovered() {
			Debug.Log(name + " has been discovered!");

		}

#if UNITY_EDITOR
		[Button]
		private void MatchRotation() {
			var rotation = transform.rotation.eulerAngles.y;
			Direction = DirectionUtil.GetDirection(rotation);
		}
#endif
	}
}
