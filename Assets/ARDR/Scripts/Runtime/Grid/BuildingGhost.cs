using PeraCore.Runtime;
using Sirenix.Utilities;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class BuildingGhost : MonoSingleton<BuildingGhost> {
		public GridData GridData;
		public Transform parent;
		public BoolVariable IsDragging;
		public Material ghostMaterial;
		public Vector2Int lastCellPos;
		public RectTransform canvas;
		public RectTransform overlayObject;
		public float floatingAmount = 1f;

		private static Direction currentDirection => EditGridController.Instance.currentDirection;

		private Camera _cam;
		private PlaceableObjectData _currentData;
		private GameObject _ghostObject;

		protected override void Awake() {
			base.Awake();
			_cam = Camera.main;
		}

		private void Update() {
			if (!EditGridController.Instance.isEditing) return;
			var center = GetCenterWorldPosition();
			overlayObject.anchoredPosition = WorldToCanvasPosition(canvas, _cam, center);
		}

		public void LateUpdate() {
			if (!EditGridController.Instance.isEditing) return;
			var targetPosition = transform.position;

			var dir = currentDirection;

			var worldPosition = GetMouseWorldPosition();
			if (IsDragging.Value && GridData.GetSnappedPosition(_currentData, worldPosition, dir)
				    .GetValue(out var snappedPosition)) {
				targetPosition = snappedPosition;
				Debug.Log($"worldposition: {worldPosition} -> {snappedPosition}");
				lastCellPos = GridData.GetCellPos(worldPosition);
			}
			targetPosition.y = floatingAmount;
			transform.position = targetPosition;
		}

		public Vector3 GetCenterWorldPosition() {
			var pos = transform.position;
			var (centerX, centerY) = _currentData.GetCenterOffset(currentDirection);
			pos += new Vector3(centerX, 0, centerY);
			return pos;
		}

		public PlacedObject<PlaceableObjectData> InitGhost(PlaceableObjectData data) {
			if (!_ghostObject.SafeIsUnityNull()) { //If already there is visual, destroy old ghost
				Destroy(_ghostObject.gameObject);
			}
			parent.gameObject.SetActive(true);
			_ghostObject = Instantiate(data.model, Vector3.zero, Quaternion.identity);
			var objectTransform = _ghostObject.transform;
			objectTransform.parent = parent;
			objectTransform.localPosition = Vector3.zero;
			objectTransform.localEulerAngles = Vector3.zero;
			SetLayerRecursive(_ghostObject, 2);
			_ghostObject.GetComponentsInChildren<MeshRenderer>().ForEach(mr => { mr.material = ghostMaterial; });
			_currentData = data;
			canvas.gameObject.SetActive(true);
			RotateObject(currentDirection);
			return _ghostObject.GetComponent<PlacedObject<PlaceableObjectData>>();
		}

		public PlacedObject<PlaceableObjectData> InitGhost(PlaceableObjectData data, Vector2Int cellPos) {
			if (GridData.GetWorldPosition(cellPos).GetValue(out var worldPos)) {
				lastCellPos = cellPos;
				transform.position = worldPos;
				return InitGhost(data);
			}
			return null;
		}

		public void RotateObject(Direction direction) {
			transform.localRotation = direction.GetRotationRotation();
			var localChunkPos = GridData.GetLocalChunkPos(lastCellPos);
			var chunk = GridData.GetChunk(lastCellPos);
			var snappedPosition = chunk.GetWorldSnappedPosition(_currentData, direction, localChunkPos);
			snappedPosition.y = floatingAmount;
			transform.position = snappedPosition;
		}

		public void DestroyVisual() {
			parent.gameObject.SetActive(false);
			canvas.gameObject.SetActive(false);
			Destroy(_ghostObject.gameObject);
		}

		private void SetLayerRecursive(GameObject targetGameObject, int layer) {
			targetGameObject.layer = layer;
			foreach (Transform child in targetGameObject.transform) {
				SetLayerRecursive(child.gameObject, layer);
			}
		}

		public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position) {
			Vector2 temp = camera.WorldToViewportPoint(position);

			var sizeDelta = canvas.sizeDelta;
			temp.x *= sizeDelta.x;
			temp.y *= sizeDelta.y;

			temp.x -= sizeDelta.x * canvas.pivot.x;
			temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

			return temp;
		}

		public Vector3 GetMouseWorldPosition() {
			var ray = _cam.ScreenPointToRay(Input.mousePosition);
			return Physics.Raycast(ray, out var raycastHit, 999f)
				? raycastHit.point
				: Vector3.zero;
		}
	}
}
