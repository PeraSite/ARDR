using System;
using Lean.Touch;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class GridEditSystem : MonoSingleton<GridEditSystem> {
		[Header("변수")]
		public GridData GridData;

		public BoolVariable IsEditing;
		public DirectionVariable CurrentDirection;

		private PlaceableObjectData _currentData;
		private IPlacedObject _editingObject;
		private Transform _editingObjectTransform;
		private string _editingObjectState;

		private Action<IPlacedObject> OnPlaced;
		private Action OnCancelled;

		public void OnLongTouch(LeanFinger finger) {
			if (Physics.Raycast(finger.GetRay(), out var info)) {
				if (!info.collider.CompareTag("Placeable")) return;
				var targetTransform = info.transform;
				if (!targetTransform.TryGetComponent<IPlacedObject>(out var placedObject)) {
					return;
				}

				placedObject.OnEditStart();
				placedObject.IsEditing = true;
				_editingObjectTransform = targetTransform;
				_editingObject = placedObject;
				_editingObjectState = placedObject.RecordData();
				var cellPos = GridData.GetCellPos(targetTransform.position);
				targetTransform.gameObject.SetActive(false);
				SetEditMode(placedObject.BaseData, cellPos);
			}
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data, Action<IPlacedObject> onPlaced = null,
			Action onCancelled = null) {
			IsEditing.Value = true;
			_currentData = data;
			CurrentDirection.Value = Direction.Down;
			OnPlaced = onPlaced;
			OnCancelled = onCancelled;
			Debug.Log(BuildingGhost.Instance);
			BuildingGhost.Instance.InitGhost(_currentData);
		}

		[Button]
		public void Test() {
			Debug.Log(BuildingGhost.Instance);
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data, Vector2Int cellPos) {
			IsEditing.Value = true;
			_currentData = data;
			CurrentDirection.Value = Direction.Down;
			BuildingGhost.Instance.InitGhost(_currentData, cellPos);
		}

		[Button]
		public void UpdateNextDirection() {
			CurrentDirection.Value = CurrentDirection.Value.GetNextDir();
			BuildingGhost.Instance.RotateObject(CurrentDirection.Value);
		}

		public void CommitUpdate() {
			var lastCellPos = BuildingGhost.Instance.lastCellPos;
			if (!GridData.CanPlaceAt(_currentData, lastCellPos, CurrentDirection.Value)) {
				Debug.Log("이 곳에는 설치할 수 없습니다!");
				return;
			}
			var isEditingObject = _editingObject != null;
			if (isEditingObject) { //Destroy old object
				var chunk = _editingObject.Chunk;
				chunk.RemovePlacedObject(_editingObject);
			}

			var placedObject = GridData.PlaceObjectAtSafe(_currentData, lastCellPos, CurrentDirection.Value);

			if (!isEditingObject) { //If first place
				placedObject.OnInit();
			} else {
				placedObject.ApplyData(_editingObjectState);
				placedObject.OnEditEnd();
				placedObject.IsEditing = false;
			}
			OnPlaced?.Invoke(placedObject);

			BuildingGhost.Instance.DestroyVisual();
			IsEditing.Value = false;
			_editingObject = null;
			_editingObjectState = "";
			_editingObjectTransform = null;
			_currentData = null;
			OnPlaced = null;
			OnCancelled = null;
		}

		public void CancelEdit() {
			OnCancelled?.Invoke();
			IsEditing.Value = false;
			OnPlaced = null;
			OnCancelled = null;
			BuildingGhost.Instance.DestroyVisual();
			if (!_editingObjectTransform.SafeIsUnityNull()) {
				_editingObjectTransform.gameObject.SetActive(true);
			}
		}
	}
}
