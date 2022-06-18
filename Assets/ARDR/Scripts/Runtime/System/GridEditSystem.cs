using System;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
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
		private string _editingObjectState;

		private Action<IPlacedObject> OnPlaced;
		private Action OnCancelled;
		private Camera _cam;

		protected override void Awake() {
			base.Awake();
			_cam = Camera.main;
		}

		public void SetExistObjectEditMode(IPlacedObject placedObject) {
			IsEditing.Value = true;

			_editingObject = placedObject;
			_editingObjectState = placedObject.RecordData();
			var cellPos = GridData.GetCellPos(placedObject.Transform.position);
			placedObject.Transform.gameObject.SetActive(false);
			SetEditMode(placedObject.BaseData, cellPos);
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data) {
			SetEditMode(data, null, null);
		}

		public void SetEditMode(PlaceableObjectData data, Action<IPlacedObject> onPlaced, Action onCancelled) {
			var worldPosition = UnityUtil.GetScreenCenterWorldPosition(_cam);
			var targetCellPos = GridData.GetCellPos(worldPosition);
			if (!GridData.GetChunk(targetCellPos).IsEnabled) {
				Toast.Show("이 곳엔 놓을 수 없습니다!");
				return;
			}

			IsEditing.Value = true;
			_currentData = data;
			CurrentDirection.Value = Direction.Down;
			OnPlaced = onPlaced;
			OnCancelled = onCancelled;
			GridVisualization.Instance.Show();
			BuildingGhost.Instance.InitGhost(_currentData, targetCellPos);
		}

		[Button]
		public void SetEditMode(PlaceableObjectData data, Vector2Int cellPos) {
			IsEditing.Value = true;
			_currentData = data;
			CurrentDirection.Value = Direction.Down;
			GridVisualization.Instance.Show();
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
				Toast.Show("이 곳에는 설치할 수 없습니다!");
				return;
			}
			var isEditingObject = _editingObject != null;
			if (isEditingObject) { //Destroy old object
				var chunk = _editingObject.Chunk;
				chunk.RemovePlacedObject(_editingObject);
			}

			var placedObject = GridData.PlaceObjectAtSafe(_currentData, lastCellPos, CurrentDirection.Value);

			if (!isEditingObject) { //If first place
				placedObject.OnFirstPlaced();
			} else {
				placedObject.ApplyData(_editingObjectState);
			}
			placedObject.OnEditEnd();
			placedObject.IsEditing = false;
			OnPlaced?.Invoke(placedObject);

			BuildingGhost.Instance.DestroyVisual();
			IsEditing.Value = false;
			_editingObject = null;
			_editingObjectState = "";
			_currentData = null;
			OnPlaced = null;
			OnCancelled = null;

			GridVisualization.Instance.Hide();
		}

		public void CancelEdit() {
			OnCancelled?.Invoke();
			IsEditing.Value = false;
			OnPlaced = null;
			OnCancelled = null;
			BuildingGhost.Instance.DestroyVisual();
			_editingObject?.Transform.gameObject.SetActive(true);
			_editingObject = null;
			_editingObjectState = "";
			GridVisualization.Instance.Hide();

		}
	}
}