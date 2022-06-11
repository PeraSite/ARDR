using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class Cell {
		public bool IsEnabled;
		public IGridObject PlacedObject;

		private readonly Chunk _chunk;
		private readonly Vector2Int _localChunkPos;

		public Cell(Chunk chunk, int x, int y, bool isEnabled) : this(chunk, new Vector2Int(x, y), isEnabled) { }

		public Cell(Chunk chunk, Vector2Int localChunkPos, bool isEnabled) {
			_chunk = chunk;
			_localChunkPos = localChunkPos;
			PlacedObject = null;
			IsEnabled = isEnabled;
		}

		public override string ToString() {
			return _localChunkPos.x + ", " + _localChunkPos.y + "\n" + PlacedObject?.GetType()?.GetNiceName();
		}

		public void SetPlacedObject(IPlacedObject _placedObject) {
			PlacedObject = _placedObject;
			_chunk.cellGrid.TriggerGridObjectChanged(_localChunkPos);
		}

		public void ClearPlacedObject() {
			PlacedObject?.DestroySelf();
			PlacedObject = null;
			_chunk.cellGrid.TriggerGridObjectChanged(_localChunkPos);
		}

		public bool IsPlaced() {
			return PlacedObject != null;
		}
	}
}
