using PeraCore.Runtime;
using PixelCrushers;
using TMPro;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class ObstacleRemovePopup : MonoSingleton<ObstacleRemovePopup> {
		[Header("오브젝트")]
		public Image Icon;

		public TextMeshProUGUI Name;
		public Image Type;
		public TextMeshProUGUI Description;
		public TextMeshProUGUI Price;

		[Header("변수")]
		public LongVariable Money;

		public IntVariable WorldTreeUpgradeLevel;

		[Header("패널")]
		public UIPanel HUD;

		public UIPanel Root;

		[Header("설정")]
		public float Multiplier = 0.3f;

		private GridObstacle _obstacle;

		public void Open(GridObstacle obstacle) {
			_obstacle = obstacle;
			var data = (ObstacleObjectData) obstacle.BaseData;
			Icon.sprite = data.Display;
			Name.text = data.Name;
			Type.sprite = data.TypeIcon;
			Description.text = data.Description;
			//TODO : 장애물 제거 금액 계산
			Price.text = $"{TMPIcons.Money} {CalculateObstaclePrice()}";
			HUD.Close();
			Root.Open();
		}

		public void Close() {
			_obstacle = null;
			HUD.Open();
			Root.Close();
		}

		public void Remove() {
			var price = CalculateObstaclePrice();
			if (Money.Value < price) {
				Toast.Show("돈이 부족합니다!");
				return;
			}
			Money.Subtract(price);
			Destroy(_obstacle.gameObject);
			_obstacle.Chunk.RemovePlacedObject(_obstacle);
			Close();
		}

		private int CalculateObstaclePrice() {
			var data = (ObstacleObjectData) _obstacle.BaseData;

			return (int) (data.BasePrice * Multiplier * WorldTreeUpgradeLevel.Value);
		}
	}
}
