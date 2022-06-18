using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using TMPro;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class ManageItemElement : MonoBehaviour {
		[Header("오브젝트")]
		public GameObject NextTag;

		public TextMeshProUGUI NextLevel;

		public TextMeshProUGUI GridSize;

		public TextMeshProUGUI AddAmount;

		public Button BuyButton;
		public TextMeshProUGUI Price;

		[Header("설정")]
		public string AddAmountPrefix;

		[Header("변수")]
		public IntVariable ItemLevel;

		public ScriptableObjectCache SOCache;
		public LongVariable Money;

		public ItemUpgradeData CurrentUpgrade => UpgradeData.Find(data => data.Level == ItemLevel.Value);

		public List<ItemUpgradeData> UpgradeData => SOCache.Find<ItemUpgradeData>().ToList();

		private void OnEnable() {
			ItemLevel.Changed.Register(UpdateUI);
			UpdateUI();
			BuyButton.onClick.AddListener(OnBuy);
		}

		private void OnDisable() {
			ItemLevel.Changed.Unregister(UpdateUI);
			BuyButton.onClick.RemoveListener(OnBuy);
		}

		private void OnBuy() {
			if (Money.Value < NextUpgrade.UpgradePrice) {
				Toast.Show("돈이 부족합니다!");
				return;
			}
			Money.Subtract(NextUpgrade.UpgradePrice);
			ItemLevel.Add(1);
		}

		private void UpdateUI() {
			if (IsMaxLevel()) {
				NextLevel.text = "Lv. MAX";
				NextTag.SetActive(false);
				BuyButton.gameObject.SetActive(false);
				return;
			}
			BuyButton.gameObject.SetActive(true);
			NextTag.SetActive(true);
			var upgradeLevel = CurrentUpgrade.Level;
			var nextUpgrade = NextUpgrade;
			NextLevel.text = $"Lv. {upgradeLevel} → <b>Lv. {upgradeLevel + 1}";
			GridSize.text = $"{CurrentUpgrade.Range.x}x{CurrentUpgrade.Range.x}";
			AddAmount.text = $"{AddAmountPrefix} {CurrentUpgrade.AddAmount}";
			Price.text = $"{TMPIcons.Money} {nextUpgrade.UpgradePrice}";
		}

		private ItemUpgradeData NextUpgrade =>
			IsMaxLevel() ? CurrentUpgrade : UpgradeData.Find(data => data.Level == ItemLevel.Value + 1);

		private bool IsMaxLevel() => CurrentUpgrade.Level == UpgradeData.Count;
	}
}
