using PeraCore.Runtime;
using Sirenix.Utilities;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class BuildingGhost : MonoSingleton<BuildingGhost> {
		[Header("오브젝트")]
		public RectTransform canvas;

		public RectTransform overlayObject;
		public GridData GridData;
		public Transform parent;
		public Material ghostMaterial;

		[Header("변수")]
		public BoolVariable IsEditingObject;

		public DirectionVariable CurrentDirection;
		public BoolVariable IsDragging;

		[Header("설정")]
		public float floatingAmount = 1f;

		[HideInInspector]
		public Vector2Int lastCellPos;

		private Camera _cam;
		private PlaceableObjectData _currentData;
		private GameObject _ghostObject;

		protected override void Awake() {
			base.Awake();
			_cam = Camera.main;
		}

		private void OnDisable() {
			IsEditingObject.Value = false;
		}

		private void Update() {
			if (!IsEditingObject.Value) return;
			var center = GetCenterWorldPosition();
			overlayObject.anchoredPosition = UnityUtil.WorldToCanvasPosition(canvas, _cam, center);

			var targetPosition = transform.position;
			var dir = CurrentDirection.Value;

			var worldPosition = UnityUtil.GetMouseWorldPosition(_cam);
			if (IsDragging.Value && GridData.GetSnappedPosition(_currentData, worldPosition, dir)
				    .GetValue(out var snappedPosition)) {
				targetPosition = snappedPosition;
				lastCellPos = GridData.GetCellPos(worldPosition);
			}
			targetPosition.y = floatingAmount;
			transform.position = targetPosition;
		}

		public Vector3 GetCenterWorldPosition() {
			var pos = transform.position;
			var (centerX, centerY) = _currentData.GetCenterOffset(CurrentDirection.Value);
			pos += new Vector3(centerX, 0, centerY);
			return pos;
		}

		public void InitGhost(PlaceableObjectData data, Vector2Int cellPos) {
			if (GridData.GetWorldPosition(cellPos).GetValue(out var worldPos)) {
				lastCellPos = cellPos;
				transform.position = worldPos;
			}

			_currentData = data;

			if (!_ghostObject.SafeIsUnityNull()) { //If already there is visual, destroy old ghost
				Destroy(_ghostObject.gameObject);
			}
			parent.gameObject.SetActive(true);
			_ghostObject = Instantiate(data.model, Vector3.zero, Quaternion.identity);
			var placedObject = _ghostObject.GetComponent<IPlacedObject>();
			placedObject.OnInstantiated(data);
			placedObject.OnEditStart();
			placedObject.IsEditing = true;


			var objectTransform = _ghostObject.transform;
			objectTransform.parent = parent;
			objectTransform.localPosition = Vector3.zero;
			objectTransform.localEulerAngles = Vector3.zero;
			_ghostObject.SetLayerRecursive(2);
			_ghostObject.GetComponentsInChildren<MeshRenderer>().ForEach(mr => {
				var length = mr.materials.Length;
				var materialArr = new Material[mr.materials.Length];
				for (var i = 0; i < length; i++) {
					materialArr[i] = ghostMaterial;
				}
				mr.materials = materialArr;
			});
			canvas.gameObject.SetActive(true);
			RotateObject(CurrentDirection.Value);
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
	}
}
