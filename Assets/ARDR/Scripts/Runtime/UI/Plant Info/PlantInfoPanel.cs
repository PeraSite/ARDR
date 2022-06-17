using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class PlantInfoPanel : MonoBehaviour {
		[Header("오브젝트")]
		public Image Icon;

		public TextMeshProUGUI Name;
		public Image Type;
		public TextMeshProUGUI ThemeText;
		public TextMeshProUGUI Description;

		public void Init(Plant plant) {
			var data = plant.Data;
			Icon.sprite = data.Display;
			Name.text = data.Name;
			Type.sprite = data.Type.Display;
			ThemeText.text = data.Theme.ToString();
			Description.text = data.Description;
		}
	}
}
