using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class StoreUI : BaseCategoryPopup<StoreHeader> {
		[Header("오브젝트")]
		public StoreElement ElementPrefab;

		public RectTransform ContentRect;

		public ScrollRect ScrollRect;

		public PlantBuyPopup PlantBuyPopup;

		[Header("설정")]
		public ScriptableObjectCache SOCache;

		private readonly List<StoreElement> _showingElements = new();

		protected override void Start() {
			base.Start();
			_showingElements.ForEach(element => Destroy(element.gameObject));
			_showingElements.Clear();

			PlantBuyPopup.Init();
			SOCache.Find<PlantData>()
				.OrderBy(plant => plant.Price)
				.ForEach(plant => {
					var instantiated = Instantiate(ElementPrefab, ContentRect);
					instantiated.Init(plant);
					instantiated.name = plant.Name;
					instantiated.Button.onClick.AddListener(() => OnBuyButtonPressed(plant));
					_showingElements.Add(instantiated);
				});
		}

		protected override void OnHeaderChanged(StoreHeader newHeader) {
			base.OnHeaderChanged(newHeader);
			ContentRect.anchoredPosition = Vector2.zero;
			ScrollRect.velocity = Vector2.zero;

			_showingElements.ForEach(
				element => element.gameObject.SetActive(newHeader.ThemeType.HasFlag(element.Theme)));
		}

		private void OnBuyButtonPressed(PlantData plantData) {
			PlantBuyPopup.Show(plantData);
		}
	}
}
