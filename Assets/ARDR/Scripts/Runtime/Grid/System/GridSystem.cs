using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class GridSystem : SerializedMonoBehaviour {
		public GridData GridData;

		public Vector3 OriginPosition;

		public Vector2Int gridSize = new(10, 10);

		public List<Vector2Int> defaultEnableChunk = new();

		public Dictionary<Vector2Int, List<Vector2Int>> DefaultDisabledCell = new();

		private void Start() {
			GenerateGrid();
		}

		private void OnDisable() {
			GridData.chunkGrid = null;
		}

		public void GenerateGrid() {
			GridData.chunkGrid = new Grid<Chunk>(
				gridSize.x,
				gridSize.y,
				Chunk.cellPerChunk * Chunk.cellSize,
				OriginPosition,
				CreateGridObject,
				Color.blue);
			GridData.chunkGrid.Init();
			FindSceneGridObject();
		}

		[Button]
		public void FindSceneGridObject() {
			var gridObjectList = FindObjectsOfType<GridObjectBase>();
			foreach (var gridObject in gridObjectList) {
				var cellPos = GridData.GetCellPos(gridObject.transform.position);
				var chunk = GridData.GetChunk(cellPos);
				var localCellPos = GridData.GetLocalChunkPos(cellPos);

				gridObject.Chunk = chunk;
				gridObject.Position = localCellPos;
				gridObject.Direction = Direction.Down;
				chunk.cellGrid.GetGridObject(localCellPos).PlacedObject = gridObject;
			}
		}

		[Button]
		public void UnlockChunk(int chunkX, int chunkZ) {
			GridData.SetEnableChunk(new Vector2Int(chunkX, chunkZ), true);
		}

		private Chunk CreateGridObject(Grid<Chunk> g, int chunkX, int chunkZ) {
			return new Chunk(GridData,
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

#if UNITY_EDITOR
		private void OnDrawGizmos() {
			if (GridData.chunkGrid == null) return;
			GridData.chunkGrid.DrawGizmo(Color.red);
			foreach (var chunk in GridData.chunkGrid.GridArray) {
				chunk.cellGrid?.DrawGizmo(Color.blue);
			}
		}
#endif
	}
}
