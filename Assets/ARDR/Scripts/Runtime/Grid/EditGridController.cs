using Lean.Touch;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class EditGridController : MonoSingleton<EditGridController> {
		public GridData GridData;
		public bool isEditing;
		public PlaceableObjectData currentData;
		public Direction currentDirection;

		private PlacedObject<PlaceableObjectData> _editingObject;

		public void OnLongTouch(LeanFinger finger) {
			if (isEditing) return;
			if (Physics.Raycast(finger.GetRay(), out var info)) {
				Debug.Log("longtouch raycast:" + info.collider?.name + "," + info.collider.tag);
				if (info.collider.CompareTag("Placeable")) {
					if (!info.transform.TryGetComponent<PlacedObject<PlaceableObjectData>>(out var obj)) return;
					_editingObject = obj;
					var cellPos = GridData.GetCellPos(obj.transform.position);
					_editingObject.gameObject.SetActive(false);
					SetEditMode(obj.ObjectData, cellPos);
				}
			}
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data) {
			isEditing = true;
			currentData = data;
			currentDirection = Direction.Down;
			BuildingGhost.Instance.InitGhost(currentData);
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data, Vector2Int cellPos) {
			isEditing = true;
			currentData = data;
			currentDirection = Direction.Down;
			BuildingGhost.Instance.InitGhost(currentData, cellPos);
		}

		[Button]
		public void UpdateNextDirection() {
			currentDirection = currentDirection.GetNextDir();
			BuildingGhost.Instance.RotateObject(currentDirection);
		}

		public void CommitUpdate() {
			var lastCellPos = BuildingGhost.Instance.lastCellPos;
			if (!GridData.CanPlaceAt(currentData, lastCellPos, currentDirection)) {
				Debug.Log("이 곳에는 설치할 수 없습니다!");
				return;
			}
			var isEditingObject = !_editingObject.SafeIsUnityNull();
			if (isEditingObject) { //Destroy old object
				var chunk = _editingObject.Chunk;
				chunk.RemovePlacedObject(_editingObject);
			}

			var placed = GridData.PlaceObjectAtSafe(currentData, lastCellPos, currentDirection);
			if (!isEditingObject) { //If first place
				placed.OnInit();
			}
			isEditing = false;
			BuildingGhost.Instance.DestroyVisual();
		}

		public void CancelEdit() {
			isEditing = false;
			BuildingGhost.Instance.DestroyVisual();
			if (!_editingObject.SafeIsUnityNull()) {
				_editingObject.gameObject.SetActive(true);
			}
		}
	}
}
