using System;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class GridData : SingletonScriptableObject<GridData> {
		public Grid<Chunk> chunkGrid;
		public Vector3 OriginPosition;

		public event Action onAnyGridUpdate;

		public void InvokeAnyGridUpdate() => onAnyGridUpdate?.Invoke();

		[Button]
		public void SetEnableChunk(Vector2Int chunkPos, bool isEnabled) {
			chunkGrid.GetGridObject(chunkPos).IsEnabled = isEnabled;
		}

		public bool IsEnabled(Vector2Int chunkPos) {
			return chunkGrid.GetGridObject(chunkPos).IsEnabled;
		}

		[Button]
		public bool CanPlaceAt(PlaceableObjectData objectData, Vector2Int originCellPos, Direction dir) {
			var canPlace = true;
			var gridPositionList = objectData.GetGridPositionList(originCellPos, dir);
			foreach (var subCellPos in gridPositionList) {
				var chunk = GetChunk(subCellPos);
				if (!chunk.IsEnabled) return false;
				var localChunkPos = GetLocalChunkPos(subCellPos);
				var cell = chunk[localChunkPos];
				if (cell.IsPlaced()) canPlace = false;
				if (!cell.IsEnabled) canPlace = false;
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

			originChunk.Objects.Add(new Chunk.PlacedObjectInfo
				{Data = objectData, PlacedObject = placedObject, Position = originLocalChunkPos, Direction = dir});
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
			new(cellPos.x % Chunk.cellPerChunk, cellPos.y % Chunk.cellPerChunk);

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


		public Vector3? GetWorldPosition(int cellX, int cellZ) {
			return GetWorldPosition(new Vector2Int(cellX, cellZ));
		}
	}
}
