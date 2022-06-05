using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class GridSystem : SerializedMonoBehaviour {
		public GridData Grid;

		public Vector3 OriginPosition;

		public Vector2Int gridSize = new(10, 10);

		public List<Vector2Int> defaultEnableChunk = new();

		public Dictionary<Vector2Int, List<Vector2Int>> DefaultDisabledCell = new();

		[Button]
		public void GenerateGrid() {
			Grid.chunkGrid = new Grid<Chunk>(
				gridSize.x,
				gridSize.y,
				Chunk.cellPerChunk * Chunk.cellSize,
				OriginPosition,
				CreateGridObject,
				Color.blue);
			Grid.chunkGrid.Init();
		}

		private Chunk CreateGridObject(Grid<Chunk> g, int chunkX, int chunkZ) {
			return new Chunk(Grid,
				chunkX,
				chunkZ,
				defaultEnableChunk.Contains(new Vector2Int(chunkX, chunkZ)),
				(cellX, cellZ) => {
					if (DefaultDisabledCell.TryGetValue(new Vector2Int(chunkX, chunkZ), out var disabledCells)) {
						if (disabledCells.Contains(new Vector2Int(cellX, cellZ))) {
							return false;
						}
					}
					return true;
				});
		}

		private void Start() {
			GenerateGrid();
		}

		private void OnDisable() {
			Grid.chunkGrid = null;
		}
	}
}
