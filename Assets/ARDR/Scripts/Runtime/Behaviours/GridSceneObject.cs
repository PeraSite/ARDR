using UnityEngine;

namespace ARDR {
	public class GridSceneObject : GridObjectBase {
		[field: SerializeField]
		public override PlaceableObjectData BaseData { get; set; }

		[field: SerializeField]
		public override Direction Direction { get; set; }
	}
}
