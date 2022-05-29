using System.Collections;
using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARDR {
	public class PlayGameMode : ScriptableObject, IGameMode {
		[SerializeField]
		private GameModeState _state;

		[SceneSelector]
		[Tooltip("처음 게임을 시작했을 때의 위치")]
		public string startScene;

		public int activeSlotID;

		public int editorSlotID = 999;

		private static bool _doesSceneLoaded = false;

		public IEnumerator OnStart() {
			if (_state != GameModeState.ENDED) yield break;
			_state = GameModeState.STARTING;

			if (SaveSystem.storer.HasDataInSlot(activeSlotID)) {
				var data = SaveSystem.storer.RetrieveSavedGameData(activeSlotID);
				yield return SceneManager.LoadSceneAsync(data.sceneName);
				// yield return new WaitForEndOfFrame();
				SaveSystem.ApplySavedGameData(data);
			} else {
				yield return SceneManager.LoadSceneAsync(startScene);
				// yield return new WaitForEndOfFrame();
				SaveSystem.ResetGameState();
			}

			_state = GameModeState.STARTED;
		}

		public IEnumerator OnEditorStart() {
			App.isEditor = true;
			yield return new WaitUntil(() => _doesSceneLoaded);
			if (SaveSystem.storer.HasDataInSlot(editorSlotID)) {
				var data = SaveSystem.storer.RetrieveSavedGameData(editorSlotID);
				SaveSystem.ApplySavedGameData(data);
			} else {
				SaveSystem.ResetGameState();
			}
			_state = GameModeState.STARTED;
			yield return null;
		}

		public IEnumerator OnEnd() {
			_state = GameModeState.ENDING;
			SaveSystem.SaveToSlotImmediate(App.isEditor ? editorSlotID : activeSlotID);
			_state = GameModeState.ENDED;
			yield return null;
		}


		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void OnAfterSceneLoad() {
			_doesSceneLoaded = true;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetVariable() {
			_doesSceneLoaded = false;
		}
	}
}
