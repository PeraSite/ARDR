using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class WorldTreeUpgradeData : SerializedScriptableObject {
		[Header("세계수 정보")]
		public int Level;

		[PreviewField(100f)]
		public GameObject Model;

		public int MoneyPerTouch;

		public int MaxChunk;

		public long LevelupPrice;
	}
}
