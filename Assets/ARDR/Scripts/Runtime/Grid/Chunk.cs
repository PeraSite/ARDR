using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class Chunk {
		public const int cellPerChunk = 7;
		public const float cellSize = 2.5F;

		public Grid<CellObject> cellGrid;

		public bool IsEnabled {
			get => _isEnabled;
			set => SetEnabled(value);
		}

		private Vector2Int chunkPosition;
		private GridData _gridController;
		private bool _isEnabled;

		public Chunk(GridData parent, int x, int y, bool isEnabled = true) {
			_gridController = parent;
			chunkPosition = new Vector2Int(x, y);
			SetEnabled(isEnabled);
		}

		public void SetEnabled(bool isEnabled) {
			_isEnabled = isEnabled;
			if (IsEnabled && cellGrid == null) {
				//켜졌는데 안만들어져있으면 만들기
				InitCell();
			}

			if (!IsEnabled && cellGrid != null) {
				//꺼졌는데 만들어져있다면 삭제
				DestroyCell();
			}
		}

		private void DestroyCell() {
			cellGrid.GridArray.Cast<CellObject>().Where(obj => obj.IsPlaced())
				.ForEach(obj => {
					var placedObject = obj.GetPlacedObject();
					obj.SetPlacedObject(null);
					placedObject.DestroySelf();
				});
			cellGrid.Destroy();
			cellGrid = null;
		}

		public void InitCell() {
			cellGrid = new Grid<CellObject>(
				cellPerChunk, cellPerChunk, cellSize,
				_gridController.chunkGrid.GetWorldPosition(chunkPosition),
				(g, x, z) => new CellObject(this, x, z),
				Color.red
			);
			cellGrid.Init();
		}

#region Util functions

		public CellObject this[int x, int z] {
			get => cellGrid.GetGridObject(x, z);
			set => cellGrid.SetGridObject(new Vector2Int(x, z), value);
		}

		public CellObject this[Vector2Int localPos] {
			get => cellGrid.GetGridObject(localPos);
			set => cellGrid.SetGridObject(localPos, value);
		}


		public Vector3 GetWorldSnappedPosition(PlaceableObjectData objectData, Direction dir,
			Vector2Int localChunkPos) {
			var rotationOffset = objectData.GetRotationOffset(dir);
			var placedObjectWorldPosition = cellGrid.GetWorldPosition(localChunkPos.x, localChunkPos.y) +
			                                new Vector3(rotationOffset.x, 0, rotationOffset.y) * cellGrid.cellSize;
			return placedObjectWorldPosition;
		}

		public Vector3 GetWorldPosition(Vector2Int localChunkPos) {
			var placedObjectWorldPosition = cellGrid.GetWorldPosition(localChunkPos.x, localChunkPos.y);
			return placedObjectWorldPosition;
		}

		public Vector2Int ToCellPos(Vector2Int chunkLocalPos) {
			return new Vector2Int(
				chunkPosition.x * cellPerChunk + chunkLocalPos.x,
				chunkPosition.y * cellPerChunk + chunkLocalPos.y
			);
		}

		public void RemovePlacedObject(IPlacedObject obj) {
			foreach (var localChunkPos in obj.GetGridPositionList()) {
				var cellPos = ToCellPos(localChunkPos);
				var targetChunk = _gridController.GetChunk(cellPos);
				var targetLocalChunkPos = _gridController.GetLocalChunkPos(cellPos);

				targetChunk[targetLocalChunkPos].ClearPlacedObject();
			}
		}

		public override string ToString() {
			return chunkPosition.ToString();
		}

#endregion
	}
}
