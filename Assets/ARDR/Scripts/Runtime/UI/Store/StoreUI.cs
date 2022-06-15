using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class StoreUI : SerializedMonoBehaviour {
		[Header("오브젝트")]
		public Dictionary<ThemeType, StoreHeader> Headers = new();

		public StoreElement ElementPrefab;

		public RectTransform Parent;

		[Header("설정")]
		public ScriptableObjectCache SOCache;

		public Dictionary<PlantType, Sprite> PlantTypeSprites = new();

		private readonly List<StoreElement> _showingElements = new();

		private void Start() {
			_showingElements.ForEach(element => Destroy(element.gameObject));
			_showingElements.Clear();

			SOCache.Find<PlantData>()
				.OrderBy(plant => plant.Price)
				.ForEach(data => {
					var instantiated = Instantiate(ElementPrefab, Parent);
					instantiated.Init(data, PlantTypeSprites[data.Type]);
					instantiated.name = data.Name;
					_showingElements.Add(instantiated);
				});

			Headers.ForEach(pair => {
				pair.Value.Button.onClick.AddListener(() => {
					Show(pair.Value.Theme);
				});
			});

			Show(ThemeType.전체, false);
		}

		[Button]
		public void Show(ThemeType type, bool animate = true) {
			_showingElements.ForEach(element => element.gameObject.SetActive(type.HasFlag(element.Theme)));
			Headers.ForEach(pair => {
				var (theme, header) = pair;
				header.ToggleHeader(theme == type, animate);
			});
		}
	}
}
