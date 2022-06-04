using Lean.Touch;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityAtoms;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class EditGridController : MonoBehaviour {
		[Header("변수")]
		public GridData GridData;

		public BoolVariable IsEditing;
		public DirectionVariable CurrentDirection;

		private PlaceableObjectData _currentData;

		private PlacedObject<PlaceableObjectData> _editingObject;

		public void OnLongTouch(LeanFinger finger) {
			if (IsEditing.Value) return;
			if (Physics.Raycast(finger.GetRay(), out var info)) {
				if (!info.collider.CompareTag("Placeable")) return;
				if (!info.transform.TryGetComponent<PlacedObject<PlaceableObjectData>>(out var placedObject)) return;

				_editingObject = placedObject;
				var cellPos = GridData.GetCellPos(placedObject.transform.position);
				_editingObject.gameObject.SetActive(false);
				SetEditMode(placedObject.ObjectData, cellPos);
			}
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data) {
			IsEditing.Value = true;
			_currentData = data;
			CurrentDirection.Value = Direction.Down;
			BuildingGhost.Instance.InitGhost(_currentData);
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
			var isEditingObject = !_editingObject.SafeIsUnityNull();
			if (isEditingObject) { //Destroy old object
				var chunk = _editingObject.Chunk;
				chunk.RemovePlacedObject(_editingObject);
			}

			var placed = GridData.PlaceObjectAtSafe(_currentData, lastCellPos, CurrentDirection.Value);
			if (!isEditingObject) { //If first place
				placed.OnInit();
			}
			IsEditing.Value = false;
			BuildingGhost.Instance.DestroyVisual();
		}

		public void CancelEdit() {
			IsEditing.Value = false;
			BuildingGhost.Instance.DestroyVisual();
			if (!_editingObject.SafeIsUnityNull()) {
				_editingObject.gameObject.SetActive(true);
			}
		}
	}
}
