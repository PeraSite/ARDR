using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class StoreUI : MonoBehaviour {
		[Header("오브젝트")]
		public Dictionary<ThemeType, RectTransform> Headers = new();

		public StoreElement ElementPrefab;

		public RectTransform Parent;

		[Header("설정")]
		public ScriptableObjectCache SOCache;

		private readonly List<StoreElement> _showingElements = new();

		private void Start() {
			_showingElements.ForEach(element => Destroy(element.gameObject));
			_showingElements.Clear();

			SOCache.Find<PlantData>()
				.OrderBy(plant => plant.Price)
				.ForEach(data => {
					var instantiated = Instantiate(ElementPrefab, Parent);
					instantiated.Init(data);
					instantiated.name = data.Name;
					_showingElements.Add(instantiated);
				});
		}

		[Button]
		public void Show(ThemeType type) {
			_showingElements.ForEach(element => element.gameObject.SetActive(type.HasFlag(element.Theme)));
		}
	}
}
