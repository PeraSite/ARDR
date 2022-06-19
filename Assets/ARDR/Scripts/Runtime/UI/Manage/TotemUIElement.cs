using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class TotemUIElement : MonoBehaviour {
		[Header("설정")]
		public ThemeType Theme;

		public GridData Grid;

		[Header("오브젝트")]
		public GameObject NotUnlockedContainer;

		private void OnEnable() {
			Grid.onAnyGridUpdate -= UpdateUI;
			Grid.onAnyGridUpdate += UpdateUI;
			UpdateUI();
		}

		private void OnDisable() {
			Grid.onAnyGridUpdate -= UpdateUI;
		}

		[Button]
		private void UpdateUI() {
			var discovered = FindObjectsOfType<Totem>().Any(totem => totem.Theme == Theme);
			for (var i = 0; i < transform.childCount-1; i++) {
				transform.GetChild(i).gameObject.SetActive(discovered);
			}
			NotUnlockedContainer.SetActive(!discovered);
		}
	}
}
