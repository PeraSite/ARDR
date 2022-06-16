﻿using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class CategoryHeader : MonoBehaviour {
		[Header("애니메이션 설정")]
		public float ShowHeight = 180f;
		public float HideHeight = 90f;

		public Color HeaderShowColor = Color.white;
		public Color HeaderHideColor = new(0f, 0f, 0f, 0.78f);

		public Color TextShowColor =  Color.black;
		public Color TextHideColor =  Color.white;

		public float AnimationTime = 0.5f;

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
			var targetSizeDelta = new Vector2(_rect.sizeDelta.x, visibility ? ShowHeight : HideHeight);
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
