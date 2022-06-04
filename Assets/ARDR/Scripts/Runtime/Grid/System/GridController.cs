using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class GridController : SerializedMonoBehaviour {
		public GridData Grid;

		public Vector3 OriginPosition;

		public Vector2Int gridSize = new(10, 10);

		public List<Vector2Int> defaultEnableChunk = new();

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

		private Chunk CreateGridObject(Grid<Chunk> g, int x, int z) {
			return new Chunk(Grid, x, z, defaultEnableChunk.Contains(new Vector2Int(x, z)));
		}

		private void Start() {
			GenerateGrid();
		}

		private void OnDisable() {
			Grid.chunkGrid = null;
		}
	}
}
