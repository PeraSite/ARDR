using System.Collections;
using PeraCore.Runtime;
using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARDR {
	public class GameModeManager : MonoSingleton<GameModeManager> {
		public MainMenuGameMode mainMenuMode;
		public PlayGameMode playMode;

		private bool _isSwitching;
		private GameModeBase _currentMode;

		protected override bool KeepAlive => false;

		protected override void Awake() {
			base.Awake();
#if UNITY_EDITOR
			switch (SceneManager.GetActiveScene().buildIndex) {
				//메인메뉴 씬
				case 0:
					_currentMode = mainMenuMode;
					StartCoroutine(_currentMode.OnEditorStart());
					break;
				//게임 플레이 맵 씬
				default:
					_currentMode = playMode;
					StartCoroutine(_currentMode.OnEditorStart());
					break;
			}
#endif
		}

		protected void OnEnable() {
			Application.wantsToQuit -= OnWantsToQuit;
			Application.wantsToQuit += OnWantsToQuit;
		}

		private void OnDisable() {
			Application.wantsToQuit -= OnWantsToQuit;
		}

		private bool OnWantsToQuit() {
			StartCoroutine(_currentMode.OnEnd());
			return true;
		}

		public void HandleStartRequested(GameModeBase mode) {
			StartCoroutine(SwitchModeCoroutine(mode));
		}

		private IEnumerator SwitchModeCoroutine(GameModeBase mode) {
			yield return new WaitUntil(() => !_isSwitching);

			if (_currentMode == mode) yield break;
			// DebugUtil.Log($"Start switching to {mode.GetType().GetNiceName()}");

			_isSwitching = true;
			yield return SaveSystem.sceneTransitionManager.LeaveScene();

			if (_currentMode != null) {
				yield return _currentMode.OnEnd();
			}
			_currentMode = mode;
			yield return _currentMode.OnStart();

			yield return SaveSystem.sceneTransitionManager.EnterScene();

			_isSwitching = false;
			// Debug.Log("End Switching");
		}
	}
}
