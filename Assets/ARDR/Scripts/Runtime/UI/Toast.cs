using DG.Tweening;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ARDR {
	public class Toast : MonoSingleton<Toast> {
		[Header("오브젝트")]
		public RectTransform Container;

		public TextMeshProUGUI Text;

		[Header("설정")]
		public float ShowY;

		public float HideY;

		public float AnimationTime;

		public float ShowTime;

		[Button]
		private void ShowToast(string message) {
			Container.DOKill(true);
			Text.text = message;
			DOTween.Sequence(Container)
				.Append(Container.DOAnchorPosY(ShowY, AnimationTime))
				.AppendInterval(ShowTime)
				.Append(Container.DOAnchorPosY(HideY, AnimationTime));
		}

		public static void Show(string message) {
			Instance.ShowToast(message);
		}
	}
}
