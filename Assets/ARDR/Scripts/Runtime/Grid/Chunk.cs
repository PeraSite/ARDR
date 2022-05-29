﻿using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class Chunk {
		public const int cellPerChunk = 8;
		public const int cellSize = 5;

		public bool IsEnabled {
			get => _isEnabled;
			set => SetEnabled(value);
		}

		public Vector2Int pos;

		private bool _isEnabled;
		public Grid<CellObject> cellGrid;
		private GridController parent;

		public Chunk(GridController parent, int x, int y, bool isEnabled = true) {
			this.parent = parent;
			pos = new Vector2Int(x, y);
			SetEnabled(isEnabled);
		}

		public void SetEnabled(bool isEnabled) {
			_isEnabled = isEnabled;
			if (IsEnabled && cellGrid == null) InitCell(); //If set enabled, but not initialized cells

			if (!IsEnabled && cellGrid != null) { //If set disabled, but already initialized
				cellGrid.GridArray.Cast<CellObject>().Where(obj => obj.IsPlaced())
					.ForEach(obj => {
						var placedObject = obj.GetPlacedObject();
						obj.SetPlacedObject(null);
						placedObject.DestroySelf();
					});
				cellGrid = null;
			}
		}

		public void InitCell() {
			cellGrid = new Grid<CellObject>(
				cellPerChunk, cellPerChunk, cellSize,
				parent.chunkGrid.GetWorldPosition(pos.x, pos.y),
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
				pos.x * cellPerChunk + chunkLocalPos.x,
				pos.y * cellPerChunk + chunkLocalPos.y
			);
		}

		public void RemovePlacedObject(PlacedObject<PlaceableObjectData> obj) {
			foreach (var localChunkPos in obj.GetGridPositionList()) {
				var cellPos = ToCellPos(localChunkPos);
				var targetChunk = parent.GetChunk(cellPos);
				var targetLocalChunkPos = parent.GetLocalChunkPos(cellPos);

				targetChunk[targetLocalChunkPos].ClearPlacedObject();
			}
		}

		public override string ToString() {
			return pos.ToString();
		}

#endregion
	}
}
