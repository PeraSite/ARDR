using UnityEngine;
using UnityEngine.Events;

namespace ARDR {
	public class TouchListener : MonoBehaviour, ITouchListener {
		public UnityEvent TouchEvent;

		public void OnTouch() {
			TouchEvent.Invoke();
		}
	}
}
