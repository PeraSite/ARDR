using System.Collections.Generic;
using PixelCrushers;
using Sirenix.Utilities;

namespace ARDR {
	public class PanelCategoryPopup : BaseCategoryPopup<CategoryHeader> {
		public List<UIPanel> Panels;

		protected override void OnHeaderChanged(CategoryHeader newHeader) {
			base.OnHeaderChanged(newHeader);
			var showingIndex = Headers.IndexOf(newHeader);
			Panels.ForEach((go, index) => {
				go.SetOpen(index == showingIndex);
			});
		}
	}
}
