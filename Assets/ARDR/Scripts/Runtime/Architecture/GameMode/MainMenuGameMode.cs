using System.Collections;
using PixelCrushers;

namespace ARDR {
	public class MainMenuGameMode : GameModeBase {
		[SceneSelector]
		public string mainMenuScene;

		public override IEnumerator OnStart() {
			yield return SaveSystem.LoadAdditiveSceneAsync(mainMenuScene);
		}

		public override IEnumerator OnEditorStart() {
			yield return null;
		}

		public override IEnumerator OnEnd() {
			yield return null;
		}
	}
}
