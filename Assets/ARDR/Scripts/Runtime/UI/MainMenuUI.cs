using UnityEngine;

namespace ARDR {
	public class MainMenuUI : MonoBehaviour {
		public void ChangeGameMode(GameModeBase gameMode) {
			GameModeManager.Instance.HandleStartRequested(gameMode);
		}
	}
}
