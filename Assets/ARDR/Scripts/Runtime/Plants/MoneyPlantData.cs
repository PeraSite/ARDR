using Sirenix.OdinInspector;

namespace ARDR {
	public class MoneyPlantData : PlaceableObjectData {
		[BoxGroup("Plant Setting")]
		public int MoneyAmount;

		[BoxGroup("Plant Setting")]
		[SuffixLabel("Seconds", overlay: true)]
		public int GiveDelay;
	}
}
