using Sirenix.Utilities;
using TMPro;
using UnityAtoms;
using UnityEngine;

namespace ARDR {
	public class LongVariableTextBinder : MonoBehaviour {
		public TextMeshProUGUI Text;
		public LongVariable Variable;

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

		private void OnChanged(long value) {
			Text.text = value.ToString();
		}
	}
}
