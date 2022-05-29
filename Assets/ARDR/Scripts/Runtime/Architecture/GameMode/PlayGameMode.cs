using System.Collections;
using System.Collections.Generic;
using ARDR;
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
			yield return new WaitForEndOfFrame();
			SaveSystem.ResetGameState();
			_state = GameModeState.STARTED;
			yield return null;
		}

		public IEnumerator OnEnd() {
			_state = GameModeState.ENDING;

			//에디터에서 바로 실제 레벨로 시작했다면 저장 안함
			if (!App.isEditor) {
				SaveSystem.SaveToSlotImmediate(activeSlotID);
			}
			_state = GameModeState.ENDED;
			yield return null;
		}
	}
}
