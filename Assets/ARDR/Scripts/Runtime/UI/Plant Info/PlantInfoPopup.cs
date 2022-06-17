using PeraCore.Runtime;
using PixelCrushers.Wrappers;

namespace ARDR {
	public class PlantInfoPopup : MonoSingleton<PlantInfoPopup> {
		public UIPanel RootPanel;
		public UIPanel HUD;

		public PlantStatePanel PlantStatePanel;
		public PlantInfoPanel PlantInfoPanel;

		private Plant _plant;

		public void Show(Plant plant) {
			_plant = plant;
			HUD.Close();
			RootPanel.Open();
			PlantStatePanel.Init(plant);
			PlantInfoPanel.Init(plant);
		}

		public void Close() {
			RootPanel.Close();
			HUD.Open();
		}
	}
}
