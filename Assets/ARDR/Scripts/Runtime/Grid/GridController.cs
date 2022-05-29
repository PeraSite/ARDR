using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class GridController : SerializedScriptableObject {
		public Vector2Int gridSize = new Vector2Int(10, 10);
		public Grid<Chunk> chunkGrid;

		public List<Vector2Int> defaultEnableChunk = new List<Vector2Int>();

		[Button]
		public void CreateGrid() {
			chunkGrid = new Grid<Chunk>(
				gridSize.x,
				gridSize.y,
				Chunk.cellPerChunk * Chunk.cellSize,
				new Vector3(0, 0, 0),
				CreateGridObject,
				Color.blue);
			chunkGrid.Init();
		}

		private Chunk CreateGridObject(Grid<Chunk> g, int x, int z) {
			return new Chunk(this, x, z, defaultEnableChunk.Contains(new Vector2Int(x, z)));
		}

		[Button]
		public void SetEnableChunk(Vector2Int chunkPos, bool isEnabled) {
			chunkGrid.GetGridObject(chunkPos).IsEnabled = isEnabled;
		}

		[Button]
		public bool CanPlaceAt(PlaceableObjectData objectData, Vector2Int originCellPos, Direction dir) {
			var canPlace = true;
			var gridPositionList = objectData.GetGridPositionList(originCellPos, dir);
			foreach (var subCellPos in gridPositionList) {
				var chunk = GetChunk(subCellPos);
				if (!chunk.IsEnabled)
					return false;
				var localChunkPos = GetLocalChunkPos(subCellPos);
				if (chunk[localChunkPos].IsPlaced()) canPlace = false;
			}
			return canPlace;
		}

		[Button]
		public IPlacedObject PlaceObjectAtSafe(PlaceableObjectData objectData,
			Vector2Int originCellPos, Direction dir) {
			if (!CanPlaceAt(objectData, originCellPos, dir))
				throw new Exception($"Can't place at {originCellPos}");

			var originChunk = GetChunk(originCellPos);
			if (!originChunk.IsEnabled) throw new Exception($"Can't place on disabled chunk: {originChunk}");
			var originLocalChunkPos = GetLocalChunkPos(originCellPos);
			var placedObject = PlacedObjectUtil.Create(
				originChunk,
				originChunk.GetWorldSnappedPosition(objectData, dir, originLocalChunkPos),
				originLocalChunkPos,
				dir,
				objectData
			);

			var gridPositionList = objectData.GetGridPositionList(originCellPos, dir);
			foreach (var subCellPos in gridPositionList) {
				var chunk = GetChunk(subCellPos);
				if (!chunk.IsEnabled) throw new Exception($"Can't place on disabled chunk: {chunk}");
				var localChunkPos = GetLocalChunkPos(subCellPos);
				chunk[localChunkPos].SetPlacedObject(placedObject);
			}

			return placedObject;
		}

		public bool IsValidCellPos(Vector2Int cellPos) {
			return GetChunk(cellPos)?.IsEnabled ?? false;
		}

		public Vector2Int GetCellPos(Vector3 worldPosition) {
			var x = Mathf.FloorToInt((worldPosition - chunkGrid.originWorldPosition).x / Chunk.cellSize);
			var z = Mathf.FloorToInt((worldPosition - chunkGrid.originWorldPosition).z / Chunk.cellSize);
			return new Vector2Int(x, z);
		}

		public Vector2Int GetLocalChunkPos(Vector2Int cellPos) =>
			new Vector2Int(cellPos.x % Chunk.cellPerChunk, cellPos.y % Chunk.cellPerChunk);

		public Chunk GetChunk(Vector2Int cellPos) =>
			chunkGrid.GetGridObject(
				Mathf.FloorToInt((float) cellPos.x / Chunk.cellPerChunk),
				Mathf.FloorToInt((float) cellPos.y / Chunk.cellPerChunk)
			);

		public Vector3? GetSnappedPosition(PlaceableObjectData objectData, Vector3 worldPosition, Direction dir) {
			var cellPos = GetCellPos(worldPosition);
			var chunk = GetChunk(cellPos);
			if (!IsValidCellPos(cellPos)) return null;
			var localChunkPos = GetLocalChunkPos(cellPos);
			return chunk.GetWorldSnappedPosition(objectData, dir, localChunkPos);
		}

		public Vector3? GetSnappedWorldPosition(Vector3 worldPosition) {
			var cellPos = GetCellPos(worldPosition);
			return GetWorldPosition(cellPos);
		}

		public Vector3? GetWorldPosition(Vector2Int cellPos) {
			var chunk = GetChunk(cellPos);
			if (!IsValidCellPos(cellPos)) return null;
			var localChunkPos = GetLocalChunkPos(cellPos);
			return chunk.GetWorldPosition(localChunkPos);
		}
	}
}
