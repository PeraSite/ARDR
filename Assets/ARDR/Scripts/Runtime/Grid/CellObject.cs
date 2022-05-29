using UnityEngine;

namespace ARDR {
	public class CellObject {
		private readonly Chunk chunk;
		public Vector2Int localChunkPos;
		private IPlacedObject placedObject;

		public CellObject(Chunk chunk, int x, int y) : this(chunk, new Vector2Int(x, y)) { }

		public CellObject(Chunk chunk, Vector2Int localChunkPos) {
			this.chunk = chunk;
			this.localChunkPos = localChunkPos;
			placedObject = null;
		}

		public override string ToString() {
			return localChunkPos.x + ", " + localChunkPos.y + "\n" + placedObject;
		}

		public void SetPlacedObject(IPlacedObject _placedObject) {
			placedObject = _placedObject;
			chunk.cellGrid.TriggerGridObjectChanged(localChunkPos);
		}

		public void ClearPlacedObject() {
			placedObject?.DestroySelf();
			placedObject = null;
			chunk.cellGrid.TriggerGridObjectChanged(localChunkPos);
		}

		public IPlacedObject GetPlacedObject() {
			return placedObject;
		}

		public bool IsPlaced() {
			return placedObject != null;
		}
	}
}
