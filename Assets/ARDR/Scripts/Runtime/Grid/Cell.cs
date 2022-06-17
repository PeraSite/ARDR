using Sirenix.Utilities;
using UnityEngine;

namespace ARDR {
	public class Cell {
		public bool IsEnabled;
		public IGridObject GridObject;
		public readonly Vector2Int LocalChunkPos;

		private readonly Chunk _chunk;

		public Cell(Chunk chunk, int x, int y, bool isEnabled) : this(chunk, new Vector2Int(x, y), isEnabled) { }

		public Cell(Chunk chunk, Vector2Int localChunkPos, bool isEnabled) {
			_chunk = chunk;
			LocalChunkPos = localChunkPos;
			GridObject = null;
			IsEnabled = isEnabled;
		}

		public override string ToString() {
			return LocalChunkPos.x + ", " + LocalChunkPos.y + "\n" + GridObject?.GetType()?.GetNiceName();
		}

		public void SetPlacedObject(IGridObject _placedObject) {
			GridObject = _placedObject;
			_chunk.cellGrid.TriggerGridObjectChanged(LocalChunkPos, this);
		}

		public void ClearPlacedObject() {
			GridObject?.DestroySelf();
			GridObject = null;
			_chunk.cellGrid.TriggerGridObjectChanged(LocalChunkPos, this);
		}

		public bool IsPlaced() {
			return GridObject != null;
		}
	}
}
