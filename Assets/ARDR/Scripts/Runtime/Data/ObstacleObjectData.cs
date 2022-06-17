using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class ObstacleObjectData : PlaceableObjectData {
		[BoxGroup("Obstacle")]
		public Sprite TypeIcon;

		[BoxGroup("Obstacle")]
		public int BasePrice;
	}
}
