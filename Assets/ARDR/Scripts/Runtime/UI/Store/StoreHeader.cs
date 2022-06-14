using UnityEngine;

namespace ARDR {
	public class StoreHeader : MonoBehaviour {
		[Header("오브젝트")]
		public StoreUI StoreUI;

		[Header("설정")]
		public ThemeType Theme;

		public void Show() {
			StoreUI.Show(Theme);
		}
	}
}
