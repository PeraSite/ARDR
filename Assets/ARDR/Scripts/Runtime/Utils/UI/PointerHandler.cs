using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ARDR {
	public class PointerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
		public UnityEvent<PointerEventData> OnDown;
		public UnityEvent<PointerEventData> OnUp;

		public void OnPointerDown(PointerEventData eventData) {
			OnDown.Invoke(eventData);
		}

		public void OnPointerUp(PointerEventData eventData) {
			OnUp.Invoke(eventData);
		}
	}
}
