using UnityEngine;

namespace ARDR {
	public class Totem : GridObjectBase, ITouchListener {
		[field: SerializeField]
		public override PlaceableObjectData BaseData { get; set; }

		[field: SerializeField]
		public override Direction Direction { get; set; }

		public void OnTouch() {
			Debug.Log("should open totem ui!");
		}

		public override void OnDiscovered() {
			Debug.Log(name + " has been discovered!");
		}
	}
}
