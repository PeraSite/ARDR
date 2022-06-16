using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class CategoryPopup : BaseCategoryPopup<CategoryHeader> {
		public List<GameObject> Objects;

		protected override void OnHeaderChanged(CategoryHeader newHeader) {
			base.OnHeaderChanged(newHeader);
			var showingIndex = Headers.IndexOf(newHeader);
			Objects.ForEach((go, index) => { go.SetActive(index == showingIndex); });
		}
	}
}
