using PixelCrushers.Wrappers;
using TMPro;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class PlantBuyPopup : MonoBehaviour {
		[Header("오브젝트")]
		public Image Icon;

		public TextMeshProUGUI Name;

		public TextMeshProUGUI RequireLevel;

		public TextMeshProUGUI Price;

		public Image Type;

		public TextMeshProUGUI ThemeText;

		public TextMeshProUGUI MoneyAmount;

		public TextMeshProUGUI NutritionUsage;
		public TextMeshProUGUI MoistureUsage;
		public TextMeshProUGUI GridSize;

		public Button BuyButton;

		public UIPanel PlantBuyPopupPanel;
		public UIPanel StoreUIPanel;
		public UIPanel HUD;

		[Header("변수")]
		public LongVariable Money;

		private PlantData _data;

		public void Init() {
			BuyButton.onClick.AddListener(OnBuy);
		}

		public void Show(PlantData data) {
			_data = data;
			Icon.sprite = data.Display;
			Name.text = data.Name;
			RequireLevel.text = $"세계수 레벨 {data.RequireLevel} 필요";
			Price.text = $"{TMPIcons.Money} {data.Price}";
			Type.sprite = data.Type.Display;
			ThemeText.text = data.Theme.ToString();
			MoneyAmount.text = $"{TMPIcons.Money} {data.MoneyAmount.ToString()}";

			NutritionUsage.text = $"{TMPIcons.Nutrition} {data.NutritionUsage}";
			MoistureUsage.text = $"{TMPIcons.Moisture} {data.MoistureUsage}";
			GridSize.text = $"{data.gridSize.x}x{data.gridSize.y}";
			PlantBuyPopupPanel.Open();
		}

		private void OnBuy() {
			if (Money.Value < _data.Price) {
				//TODO: 토스트 시스템
				Debug.Log("돈이 부족합니다!");
				return;
			}

			PlantBuyPopupPanel.Close();
			StoreUIPanel.Close();
			HUD.Open();
			GridEditSystem.Instance.SetEditMode(_data, (placedObject) => {
				Money.Subtract(_data.Price);
				Debug.Log($"{placedObject.BaseData.Name}을 설치했습니다!");
			}, () => { Debug.Log("취소되었습니다."); });
		}
	}
}
