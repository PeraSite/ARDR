using System.Collections.Generic;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class StoreUI : MonoBehaviour {
		[Header("오브젝트")]
		public Dictionary<ThemeType, RectTransform> Headers = new();

		[Header("설정")]
		public ScriptableObjectCache SOCache;


		[ButtonGroup]
		public void ShowAll() { }

		[ButtonGroup]
		public void Show(ThemeType type) { }
	}
}
