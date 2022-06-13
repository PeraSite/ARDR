using System.Collections;
using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARDR {
	public class PlayGameMode : GameModeBase {
		[SerializeField]
		private GameModeState _state;

		[SceneSelector]
		[Tooltip("처음 게임을 시작했을 때의 위치")]
		public string startScene;

		public int activeSlotID;

		public const int editorSlotID = 999;

		private static bool _doesSceneLoaded;

		public override IEnumerator OnStart() {
			if (_state != GameModeState.ENDED) yield break;
			_state = GameModeState.STARTING;

			if (SaveSystem.storer.HasDataInSlot(activeSlotID)) {
				var data = SaveSystem.storer.RetrieveSavedGameData(activeSlotID);
				yield return SceneManager.LoadSceneAsync(data.sceneName);
				SaveSystem.ApplySavedGameData(data);
			} else {
				yield return SceneManager.LoadSceneAsync(startScene);
				SaveSystem.ResetGameState();
			}

			_state = GameModeState.STARTED;
		}

		public override IEnumerator OnEditorStart() {
			App.isEditor = true;
			yield return new WaitUntil(() => _doesSceneLoaded);
			_state = GameModeState.STARTED;
			yield return null;
		}

		public override IEnumerator OnEnd() {
			_state = GameModeState.ENDING;
			SaveSystem.SaveToSlotImmediate(App.isEditor ? editorSlotID : activeSlotID);
			_state = GameModeState.ENDED;
			yield return null;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void OnAfterSceneLoad() {
			if (App.isEditor) {
				if (SaveSystem.storer.HasDataInSlot(editorSlotID)) {
					var data = SaveSystem.storer.RetrieveSavedGameData(editorSlotID);
					SaveSystem.ApplySavedGameData(data);
				} else {
					SaveSystem.ResetGameState();
				}
			}
			_doesSceneLoaded = true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetVariable() {
			_doesSceneLoaded = false;
		}
	}
}
