using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class CategoryHeader : MonoBehaviour {
		[Header("애니메이션 설정")]
		public Vector2 ShowPosition;

		public Vector2 HidePosition;

		public bool2 LockPosition;

		public float AnimationTime = 0.5f;

		[Header("색")]
		public Color HeaderShowColor = Color.white;

		public Color HeaderHideColor = new(0f, 0f, 0f, 0.78f);

		public Color TextShowColor = Color.black;
		public Color TextHideColor = Color.white;


		public Button Button { get; private set; }
		private RectTransform _rect;
		private Image _image;
		private TextMeshProUGUI _text;

		private void Awake() {
			_rect = GetComponent<RectTransform>();
			Button = GetComponent<Button>();
			_image = GetComponent<Image>();
			_text = GetComponentInChildren<TextMeshProUGUI>();
		}

		private void OnDisable() {
			_rect.DOKill();
		}

		public void ToggleHeader(bool visibility, bool animate = true) {
			var currentSizeDelta = _rect.sizeDelta;
			var targetSizeDelta = visibility ? ShowPosition : HidePosition;

			if (LockPosition.x)
				targetSizeDelta.x = currentSizeDelta.x;

			if (LockPosition.y)
				targetSizeDelta.y = currentSizeDelta.y;

			var targetHeaderColor = visibility ? HeaderShowColor : HeaderHideColor;
			var targetTextColor = visibility ? TextShowColor : TextHideColor;

			_image.color = targetHeaderColor;
			_text.color = targetTextColor;

			if (animate) {
				_rect.DOSizeDelta(targetSizeDelta, AnimationTime);
			} else {
				_rect.sizeDelta = targetSizeDelta;
			}
		}
	}
}
