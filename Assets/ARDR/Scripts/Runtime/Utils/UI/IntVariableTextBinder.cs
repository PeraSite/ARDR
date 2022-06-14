using Sirenix.Utilities;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class IntVariableTextBinder : MonoBehaviour {
		public TextMeshProUGUI Text;
		public IntVariable Variable;

		private void OnValidate() {
			if (Text.SafeIsUnityNull()) Text = GetComponent<TextMeshProUGUI>();
		}

		private void OnEnable() {
			Variable.Changed.Register(OnChanged);
			OnChanged(Variable.Value);
		}

		private void OnDisable() {
			Variable.Changed.Unregister(OnChanged);
		}

		private void OnChanged(int value) {
			Text.text = value.ToString();
		}
	}
}
