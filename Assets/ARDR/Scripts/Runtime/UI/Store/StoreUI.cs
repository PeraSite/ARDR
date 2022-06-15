using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class StoreUI : SerializedMonoBehaviour {
		[Header("오브젝트")]
		public Dictionary<ThemeType, StoreHeader> Headers = new();

		public StoreElement ElementPrefab;

		public RectTransform ContentRect;

		public ScrollRect ScrollRect;

		public PlantBuyPopup PlantBuyPopup;

		[Header("설정")]
		public ScriptableObjectCache SOCache;

		public Dictionary<PlantType, Sprite> PlantTypeSprites = new();

		private readonly List<StoreElement> _showingElements = new();

		private void Start() {
			_showingElements.ForEach(element => Destroy(element.gameObject));
			_showingElements.Clear();

			PlantBuyPopup.Init();

			SOCache.Find<PlantData>()
				.OrderBy(plant => plant.Price)
				.ForEach(data => {
					var instantiated = Instantiate(ElementPrefab, ContentRect);
					instantiated.Init(data, PlantTypeSprites[data.Type]);
					instantiated.name = data.Name;
					instantiated.Button.onClick.AddListener(() => OnBuyButtonPressed(data, PlantTypeSprites[data.Type]));
					_showingElements.Add(instantiated);
				});

			Headers.ForEach(pair => {
				pair.Value.Button.onClick.AddListener(() => {
					Show(pair.Value.Theme);
				});
			});

			Show(ThemeType.전체, false);
		}

		private void Show(ThemeType type, bool animate = true) {
			ContentRect.anchoredPosition = Vector2.zero;
			ScrollRect.velocity = Vector2.zero;

			_showingElements.ForEach(element => element.gameObject.SetActive(type.HasFlag(element.Theme)));
			Headers.ForEach(pair => {
				var (theme, header) = pair;
				header.ToggleHeader(theme == type, animate);
			});
		}

		private void OnBuyButtonPressed(PlantData plantData, Sprite typeSprite) {
			PlantBuyPopup.Show(plantData, typeSprite);
		}
	}
}
