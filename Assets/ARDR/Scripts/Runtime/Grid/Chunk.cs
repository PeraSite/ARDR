using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class Chunk {
		public const int cellPerChunk = 7;
		public const float cellSize = 2.5F;

		public Grid<Cell> cellGrid;
		public GridData gridData;

		public bool IsEnabled {
			get => _isEnabled;
			set => SetEnabled(value);
		}

		public Vector2Int Position;
		public readonly List<PlacedObjectInfo> Objects;

		private bool _isEnabled;
		private readonly Func<int, int, bool> _cellInitializer;

		public Chunk(GridData parent, int x, int y, bool isEnabled = true,
			Func<int, int, bool> cellInitializer = null) {
			gridData = parent;
			Position = new Vector2Int(x, y);
			_cellInitializer = cellInitializer ?? ((_, _) => true);
			Objects = new List<PlacedObjectInfo>();
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
			Objects.Clear();
		}

		public void InitCell() {
			cellGrid = new Grid<Cell>(
				cellPerChunk, cellPerChunk, cellSize,
				gridData.chunkGrid.GetWorldPosition(Position),
				(g, x, z) =>
					new Cell(this, x, z, _cellInitializer(x, z)),
				Color.red
			);
			cellGrid.Init();
		}

#region Serialization

		public struct PlacedObjectInfo {
			public PlaceableObjectData Data;
			public IPlacedObject PlacedObject;
			public Vector2Int Position;
			public Direction Direction;
		}

		public struct PlacedObjectState {
			public PlaceableObjectData type;
			public int x;
			public int y;
			public Direction dir;
			public string state;
		}

		public struct ChunkState {
			public int x;
			public int y;
			public List<PlacedObjectState> objects;
		}

		public ChunkState RecordData() {
			return new ChunkState {
				x = Position.x,
				y = Position.y,
				objects = Objects.Select(info => new PlacedObjectState {
					type = info.Data,
					x = info.Position.x,
					y = info.Position.y,
					dir = info.Direction,
					state = info.PlacedObject.RecordData()
				}).ToList()
			};
		}

		public void ApplyData(ChunkState chunkState) {
			var savedObjects = chunkState.objects ?? new List<PlacedObjectState>();
			savedObjects.ForEach(state => {
				var objectData = state.type;

				var cell = this[state.x, state.y];
				if (!cell.IsPlaced()) //없던 오브젝트라면 아예 새로 생성
				{
					gridData.PlaceObjectAtSafe(objectData, ToCellPos(cell.LocalChunkPos), state.dir);
				}
				var gridObject = cell.GridObject;
				if (gridObject is IPlacedObject placedObject) {
					placedObject.ApplyData(state.state);
				}
			});
		}

#endregion

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
				Position.x * cellPerChunk + chunkLocalPos.x,
				Position.y * cellPerChunk + chunkLocalPos.y
			);
		}

		public void RemovePlacedObject(IGridObject obj) {
			foreach (var localChunkPos in obj.GetGridPositionList()) {
				var cellPos = ToCellPos(localChunkPos);
				var targetChunk = gridData.GetChunk(cellPos);
				var targetLocalChunkPos = gridData.GetLocalChunkPos(cellPos);

				var targetIndex =
					targetChunk.Objects.FindIndex(placedObject => placedObject.Position == obj.Position);
				if (targetIndex >= 0)
					Objects.RemoveAt(targetIndex);

				targetChunk[targetLocalChunkPos].ClearPlacedObject();
			}
		}

		public override string ToString() {
			return gridData == null ? "" : Position.ToString();
		}

#endregion
	}
}
