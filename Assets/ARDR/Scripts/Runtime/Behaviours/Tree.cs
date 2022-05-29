using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class Tree : MonoBehaviour, ITouchListener {
		[Header("변수")]
		public IntVariable Money;

		public IntVariable MoneyPerTouch;

		public void OnTouch() {
			Money.Add(MoneyPerTouch);
		}
	}

}
