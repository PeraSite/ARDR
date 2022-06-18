using System.Linq;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
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

		public bool UseTextContent;

		[FormerlySerializedAs("TextShowColor")]
		public Color ContentShowColor = Color.black;

		[FormerlySerializedAs("ColorHideColor")]
		[FormerlySerializedAs("TextHideColor")]
		public Color ContentHideColor = Color.white;

		public Button Button { get; private set; }
		private RectTransform _rect;
		private Image _image;
		private TextMeshProUGUI _text;
		private Image _icon;

		private void Awake() {
			_rect = GetComponent<RectTransform>();
			Button = GetComponent<Button>();
			_image = GetComponent<Image>();
			if (UseTextContent)
				_text = GetComponentInChildren<TextMeshProUGUI>();
			else
				_icon = this.GetComponentsOnlyInChildren<Image>().First();
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
			var targetContentColor = visibility ? ContentShowColor : ContentHideColor;

			_image.color = targetHeaderColor;
			if (UseTextContent)
				_text.color = targetContentColor;
			else
				_icon.color = targetContentColor;

			if (animate) {
				_rect.DOSizeDelta(targetSizeDelta, AnimationTime);
			} else {
				_rect.sizeDelta = targetSizeDelta;
			}
		}
	}
}
