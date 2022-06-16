using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class StoreElement : SerializedMonoBehaviour {
		[Header("오브젝트")]
		public Image Icon;

		public TextMeshProUGUI Name;

		public TextMeshProUGUI RequireLevel;

		public TextMeshProUGUI Price;

		public Image Type;

		public TextMeshProUGUI ThemeText;

		public TextMeshProUGUI MoneyAmount;

		public Button Button;

		[Header("설정")]
		public ThemeType Theme;

		public void Init(PlantData Data) {
			Icon.sprite = Data.Display;
			Name.text = Data.Name;
			RequireLevel.text = $"세계수 레벨 {Data.RequireLevel} 필요";
			Price.text = $"{TMPIcons.Money} {Data.Price}";
			Theme = Data.Theme;
			Type.sprite = Data.Type.Display;
			ThemeText.text = Data.Theme.ToString();
			MoneyAmount.text = $"{TMPIcons.Money} {Data.MoneyAmount.ToString()}";
		}
	}
}
