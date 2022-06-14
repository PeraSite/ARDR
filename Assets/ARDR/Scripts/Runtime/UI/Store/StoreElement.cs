using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class StoreElement : MonoBehaviour {
		[Header("오브젝트")]
		public Image Icon;

		public TextMeshProUGUI Name;

		public TextMeshProUGUI RequireLevel;

		public TextMeshProUGUI Price;

		public ThemeType Theme;

		public void Init(PlantData Data) {
			Icon.sprite = Data.Display;
			Name.text = Data.Name;
			RequireLevel.text = $"세계수 레벨 {Data.RequireLevel} 필요";
			Price.text = $"{Data.Price}원";
			Theme = Data.Theme;
		}
	}
}
