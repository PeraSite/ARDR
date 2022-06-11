using UnityEditor;
using UnityEngine;

namespace PeraCore.Editor {
	public static class GizmoUtils {
		public static void DrawString(string text, Vector3 worldPosition, Color textColor = default,
			float textSize = 15f, Vector2 anchor = default) {
			if (textColor == default) textColor = Color.red;
			if (anchor == default) anchor = Vector2.zero;

			var view = SceneView.currentDrawingSceneView;
			if (!view)
				return;
			var screenPosition = view.camera.WorldToScreenPoint(worldPosition);
			if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 ||
			    screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0)
				return;
			var pixelRatio = HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x -
			                 HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
			Handles.BeginGUI();
			var style = new GUIStyle(GUI.skin.label) {
				fontSize = (int) textSize,
				normal = new GUIStyleState {textColor = textColor}
			};
			var size = style.CalcSize(new GUIContent(text)) * pixelRatio;
			var alignedPosition =
				((Vector2) screenPosition +
				 size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) +
				Vector2.up * view.camera.pixelHeight;
			GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
			Handles.EndGUI();
		}
	}
}
