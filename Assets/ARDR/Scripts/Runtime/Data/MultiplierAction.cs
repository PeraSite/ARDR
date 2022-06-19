using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class MultiplierAction : IntIntFunction {
		public FloatVariable Multiplier;

		public override int Call(int value) {
			Debug.Log($"original:{value}, multiplier:{Multiplier.Value}");
			return (int) (value * Multiplier.Value);
		}
	}
}
