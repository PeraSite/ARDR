using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public abstract class BaseCategoryPopup<THeader> : SerializedMonoBehaviour where THeader : CategoryHeader {
		[Header("오브젝트")]
		public List<THeader> Headers = new();

		protected virtual void Start() {
			Headers.ForEach(header => { header.Button.onClick.AddListener(() => { OnHeaderChanged(header); }); });
			ToggleHeader(Headers[0], false);
		}

		private void ToggleHeader(CategoryHeader showingHeader, bool animate = true) {
			Headers.ForEach(header => { header.ToggleHeader(showingHeader == header, animate); });
		}

		protected virtual void OnHeaderChanged(THeader newHeader) {
			ToggleHeader(newHeader);
		}
	}
}
