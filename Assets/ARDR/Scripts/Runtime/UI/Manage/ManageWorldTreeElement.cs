using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using TMPro;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class ManageWorldTreeElement : MonoBehaviour {
		[Header("오브젝트")]
		public GameObject NextTag;

		public TextMeshProUGUI NextLevel;

		public TextMeshProUGUI MoneyPerTouch;

		public TextMeshProUGUI UsableChunk;

		public TextMeshProUGUI UpgradeTime;

		public Button BuyButton;
		public TextMeshProUGUI Price;

		[Header("변수")]
		public IntVariable WorldTreeUpgradeLevel;

		public ScriptableObjectCache SOCache;
		public LongVariable Money;

		public WorldTreeUpgradeData CurrentUpgrade =>
			UpgradeData.Find(data => data.Level == WorldTreeUpgradeLevel.Value);

		public List<WorldTreeUpgradeData> UpgradeData => SOCache.Find<WorldTreeUpgradeData>().ToList();

		private void OnEnable() {
			WorldTreeUpgradeLevel.Changed.Register(UpdateUI);
			UpdateUI();
			BuyButton.onClick.AddListener(OnBuy);
		}

		private void OnDisable() {
			WorldTreeUpgradeLevel.Changed.Unregister(UpdateUI);
			BuyButton.onClick.RemoveListener(OnBuy);
		}

		private void OnBuy() {
			if (Money.Value < NextUpgrade.LevelupPrice) {
				Debug.Log("돈이 부족합니다!");
				return;
			}
			Money.Subtract(NextUpgrade.LevelupPrice);
			WorldTreeUpgradeLevel.Add(1);
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
			MoneyPerTouch.text = $"{TMPIcons.Money} {nextUpgrade.MoneyPerTouch}";
			UsableChunk.text = $"{nextUpgrade.MaxChunk}개";
			Price.text = $"{TMPIcons.Money} {nextUpgrade.LevelupPrice}";
			// UpgradeTime.text = $"{nextUpgrade.MaxChunk}개";
		}

		private WorldTreeUpgradeData NextUpgrade =>
			IsMaxLevel() ? CurrentUpgrade : UpgradeData.Find(data => data.Level == WorldTreeUpgradeLevel.Value + 1);

		private bool IsMaxLevel() => CurrentUpgrade.Level == UpgradeData.Count;
	}
}
