using System.Collections;
using DG.Tweening;
using PixelCrushers;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class FadeSceneTransitionManager : SceneTransitionManager {
		public CanvasGroup Fade;
		public float AnimationTime;

		public BoolVariable IsFading;

		public override IEnumerator LeaveScene() {
			IsFading.SetValue(true);
			Fade.gameObject.SetActive(true);
			Fade.alpha = 0f;
			yield return Fade.DOFade(1f, AnimationTime).WaitForCompletion();
			IsFading.SetValue(false);
		}

		public override IEnumerator EnterScene() {
			IsFading.SetValue(true);
			yield return Fade.DOFade(0f, AnimationTime).WaitForCompletion();
			Fade.gameObject.SetActive(false);
			IsFading.SetValue(false);
		}
	}
}
