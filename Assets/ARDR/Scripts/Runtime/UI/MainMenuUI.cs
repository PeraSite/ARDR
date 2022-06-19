using PixelCrushers;
using UnityEngine;

namespace ARDR {
	public class MainMenuUI : MonoBehaviour {
		public void ChangeGameMode(GameModeBase gameMode) {
			GameModeManager.Instance.HandleStartRequested(gameMode);
		}

		public void ResetAndStart(GameModeBase gameMode) {
			SaveSystem.DeleteSavedGameInSlot(0);
			GameModeManager.Instance.HandleStartRequested(gameMode);
		}
	}
}
