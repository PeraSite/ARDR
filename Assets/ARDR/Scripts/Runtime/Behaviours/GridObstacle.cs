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

#if UNITY_EDITOR
		[Button]
		private void MatchRotation() {
			var rotation = transform.rotation.eulerAngles.y;
			Direction = DirectionUtil.GetDirection(rotation);
		}
#endif
	}
}
