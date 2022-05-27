using System;
using Sirenix.Utilities;
using TMPro;
using UnityAtoms.BaseAtoms;
using UnityEngine;

public class IntVariableTextBinder : MonoBehaviour {
	public TextMeshProUGUI Text;
	public IntVariable Variable;

	private void OnValidate() {
		if (Text.SafeIsUnityNull()) Text = GetComponent<TextMeshProUGUI>();
	}

	private void Awake() {
		Variable.Changed.Register(OnChanged);
	}

	private void OnDisable() {
		Variable.Changed.Unregister(OnChanged);
	}

	private void OnChanged(int value) {
		Text.text = value.ToString();
	}
}
