using System.Collections;
using PixelCrushers;
using UnityEngine;

namespace ARDR {
	public class MainMenuGameMode : ScriptableObject, IGameMode {
		[SceneSelector]
		public string mainMenuScene;

		public IEnumerator OnStart() {
			yield return SaveSystem.LoadAdditiveSceneAsync(mainMenuScene);
		}

		public IEnumerator OnEditorStart() {
			yield return null;
		}

		public IEnumerator OnEnd() {
			yield return null;
		}
	}
}
