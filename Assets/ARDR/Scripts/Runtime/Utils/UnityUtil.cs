﻿using UnityEngine;

namespace ARDR {
	public static class UnityUtil {
		public static void SetLayerRecursive(this GameObject gameObject, int layer) {
			gameObject.layer = layer;
			foreach (Transform child in gameObject.transform) {
				SetLayerRecursive(child.gameObject, layer);
			}
		}

		public static void SetTagRecursive(this GameObject gameObject, string tag) {
			gameObject.tag = tag;
			foreach (Transform child in gameObject.transform) {
				SetTagRecursive(child.gameObject, tag);
			}
		}

		public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position) {
			Vector2 viewport = camera.WorldToViewportPoint(position);

			var sizeDelta = canvas.sizeDelta;
			viewport.x *= sizeDelta.x;
			viewport.y *= sizeDelta.y;

			viewport.x -= sizeDelta.x * canvas.pivot.x;
			viewport.y -= canvas.sizeDelta.y * canvas.pivot.y;

			return viewport;
		}

		public static Vector3 GetMouseWorldPosition(Camera camera) {
			var ray = camera.ScreenPointToRay(Input.mousePosition);
			return Physics.Raycast(ray, out var raycastHit, 999f)
				? raycastHit.point
				: Vector3.zero;
		}
	}
}