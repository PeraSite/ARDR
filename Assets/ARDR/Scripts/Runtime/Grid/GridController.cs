using Sirenix.OdinInspector;

namespace ARDR {
	public class GridController : SerializedMonoBehaviour {
		public GridGenerator Generator;

		private void Start() {
			Generator.GenerateGrid();
		}

		private void OnDisable() {
			Generator.chunkGrid = null;
		}
	}
}
