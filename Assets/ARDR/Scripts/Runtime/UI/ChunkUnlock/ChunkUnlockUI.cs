using UnityEngine;

namespace ARDR {
	public class ChunkUnlockUI : MonoBehaviour {
		public GridData Grid;

		private Chunk _chunk;
		private Vector2Int _chunkPos;

		public void Init(Chunk chunk) {
			_chunk = chunk;
			_chunkPos = chunk.Position;
		}

		public void Unlock() {
			if (Grid.IsEnabled(_chunkPos)) return;
			//TODO: 해금 가격 체크
			GridSystem.Instance.UnlockChunk(_chunkPos);
			Destroy(gameObject);
		}
	}
}
