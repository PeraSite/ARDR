using Sirenix.Utilities;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class IntVariableTextBinder : MonoBehaviour {
		public TextMeshProUGUI Text;
		public IntVariable Variable;

		public FloatVariable Multiplier;

		private void OnValidate() {
			if (Text.SafeIsUnityNull()) Text = GetComponent<TextMeshProUGUI>();
		}

		private void OnEnable() {
			Variable.Changed.Register(UpdateUI);
			Multiplier.Changed.Register(UpdateUI);
			UpdateUI();
		}

		private void OnDisable() {
			Variable.Changed.Unregister(UpdateUI);
			Multiplier.Changed.Unregister(UpdateUI);
		}

		private void UpdateUI() {
			var value = Variable.Value;
			if (!Multiplier.SafeIsUnityNull()) {
				value = (int) (value * Multiplier.Value);
			}
			Text.text = value.ToString();
		}
	}
}
