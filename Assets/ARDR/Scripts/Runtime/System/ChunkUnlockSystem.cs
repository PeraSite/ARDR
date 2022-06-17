using UnityEngine;

namespace ARDR {
	public class ChunkUnlockSystem : MonoBehaviour {
		[Header("설정")]
		public GridData GridData;

		public ChunkUnlockUI UnlockUIPrefab;

		private void Start() {
			foreach (var chunk in GridData.chunkGrid.GridArray) {
				if (!chunk.IsEnabled) {
					var worldPosition = chunk.GetCenterWorldPosition();
					worldPosition.y = 2f;
					var instantiated = Instantiate(UnlockUIPrefab, worldPosition, Quaternion.identity, transform);
					instantiated.Init(chunk);
				}
			}
		}
	}
}
