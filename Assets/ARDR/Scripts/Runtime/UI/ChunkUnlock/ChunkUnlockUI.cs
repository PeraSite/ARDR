using System;
using UnityEngine;
using UnityEngine.UI;

namespace ARDR {
	public class ChunkUnlockUI : MonoBehaviour {
		public Button OpenUIButton;

		private Action<Chunk> _onClicked;

		public void Init(Chunk chunk, Action<Chunk> onOpenUIButtonClicked) {
			OpenUIButton.onClick.AddListener(() => onOpenUIButtonClicked(chunk));
		}
	}
}
