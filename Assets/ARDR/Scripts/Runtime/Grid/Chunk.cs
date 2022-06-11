using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class Chunk {
		public const int cellPerChunk = 7;
		public const float cellSize = 2.5F;

		public Grid<Cell> cellGrid;

		public bool IsEnabled {
			get => _isEnabled;
			set => SetEnabled(value);
		}

		private Vector2Int chunkPosition;
		private GridData _gridData;
		private bool _isEnabled;
		private Func<int, int, bool> _cellInitializer;

		public Chunk(GridData parent, int x, int y, bool isEnabled,
			Func<int, int, bool> cellInitializer) {
			_gridData = parent;
			chunkPosition = new Vector2Int(x, y);
			_cellInitializer = cellInitializer;
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
			cellGrid.GridArray.Cast<Cell>().Where(cell => cell.IsPlaced())
				.ForEach(cell => cell.ClearPlacedObject());
			cellGrid = null;
		}

		public void InitCell() {
			cellGrid = new Grid<Cell>(
				cellPerChunk, cellPerChunk, cellSize,
				_gridData.chunkGrid.GetWorldPosition(chunkPosition),
				(g, x, z) =>
					new Cell(this, x, z, _cellInitializer(x, z)),
				Color.red
			);
			cellGrid.Init();
		}

#region Util functions

		public Cell this[int x, int z] {
			get => cellGrid.GetGridObject(x, z);
			set => cellGrid.SetGridObject(new Vector2Int(x, z), value);
		}

		public Cell this[Vector2Int localPos] {
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
				var targetChunk = _gridData.GetChunk(cellPos);
				var targetLocalChunkPos = _gridData.GetLocalChunkPos(cellPos);

				targetChunk[targetLocalChunkPos].ClearPlacedObject();
			}
		}

		public override string ToString() {
			return _gridData == null ? "" : chunkPosition.ToString();
		}

#endregion
	}
}
