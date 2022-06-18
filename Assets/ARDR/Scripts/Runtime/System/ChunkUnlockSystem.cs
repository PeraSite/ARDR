using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ARDR {
	public class ChunkUnlockSystem : MonoBehaviour {
		[Header("설정")]
		public GridData GridData;

		public ChunkUnlockUI UnlockUIPrefab;

		private List<ChunkUnlockUI> _instantiated;

		private void Start() {
			_instantiated = new();
			GridData.onAnyGridUpdate -= UpdateUnlockUI;
			GridData.onAnyGridUpdate += UpdateUnlockUI;
			UpdateUnlockUI();
		}

		private void OnDisable() {
			GridData.onAnyGridUpdate -= UpdateUnlockUI;
		}

		private void UpdateUnlockUI() {
			var chunkGrid = GridData.chunkGrid.GridArray;

			_instantiated.ForEach(obj => Destroy(obj.gameObject));
			_instantiated.Clear();

			foreach (var chunk in chunkGrid) {
				//이미 활성화된 청크라면 무시
				if (chunk.IsEnabled) continue;

				//주변에 활성화된 청크가 있다면
				if (GridData.GetNeighbourChunks(chunk.Position).Any(c => c.IsEnabled)) {
					var worldPosition = chunk.GetCenterWorldPosition();
					worldPosition.y = 2f;
					var instantiated = Instantiate(UnlockUIPrefab, worldPosition, Quaternion.identity, transform);
					instantiated.Init(chunk);
					_instantiated.Add(instantiated);
				}
			}
		}
	}
}
