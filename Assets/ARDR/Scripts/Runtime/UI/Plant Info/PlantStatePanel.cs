using PixelCrushers.Wrappers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class PlantStatePanel : MonoBehaviour {
		[Header("오브젝트")]
		public Image Icon;

		public TextMeshProUGUI Name;
		public TextMeshProUGUI RequireLevel;
		public Image Type;

		public TextMeshProUGUI ThemeText;
		public TextMeshProUGUI MoneyAmount;
		public TextMeshProUGUI GridSize;
		public TextMeshProUGUI NutritionUsage;
		public TextMeshProUGUI MoistureUsage;

		public TextMeshProUGUI CurrentNutrition;
		public TextMeshProUGUI CurrentMoisture;

		public UIPanel Panel;

		private Plant _plant;

		public void Init(Plant plant) {
			var data = plant.Data;
			_plant = plant;
			Icon.sprite = data.Display;
			Name.text = data.Name;
			RequireLevel.text = $"세계수 레벨 {data.RequireLevel} 필요";
			Type.sprite = data.Type.Display;
			ThemeText.text = data.Theme.ToString();

			NutritionUsage.text = $"{TMPIcons.Nutrition} {data.NutritionUsage}";
			MoistureUsage.text = $"{TMPIcons.Moisture} {data.MoistureUsage}";
			GridSize.text = $"{data.gridSize.x}x{data.gridSize.y}";
			UpdateStateUI();
		}

		private void Update() {
			if (!Panel.isOpen) return;
			UpdateStateUI();
		}

		private void UpdateStateUI() {
			CurrentNutrition.text = $"{TMPIcons.Nutrition} {_plant.State.Nutrition}";
			CurrentMoisture.text = $"{TMPIcons.Moisture} {_plant.State.Moisture}";
			MoneyAmount.text = $"{TMPIcons.Money} {CalculateMoneyAmount().ToString()}";
		}

		private int CalculateMoneyAmount() => (int) (_plant.Data.MoneyAmount * _plant.Data.correctionValue[_plant.Chunk.Theme]);
	}
}
