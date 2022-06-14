using Sirenix.OdinInspector;

namespace ARDR {
	public class MoneyPlantData : PlaceableObjectData {
		[BoxGroup("Plant Setting")]
		public int MoneyAmount;

		[BoxGroup("Plant Setting")]
		[SuffixLabel("Seconds", overlay: true)]
		public int GiveDelay;

		[BoxGroup("Plant Settings")]
		public PlantType Type;
	}

	public enum PlantType {
		숲,
		늪,
		사막,
		정글,
		검은숲
	}
}
