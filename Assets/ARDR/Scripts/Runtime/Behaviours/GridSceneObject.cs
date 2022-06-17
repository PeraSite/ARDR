using UnityEngine;

namespace ARDR {
	public class GridSceneObject : GridObjectBase {
		[field: SerializeField]
		public override PlaceableObjectData BaseData { get; set; }

	}
}
