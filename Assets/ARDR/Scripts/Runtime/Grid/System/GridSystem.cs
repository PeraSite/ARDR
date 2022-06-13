using System.Collections.Generic;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ARDR {
	public class GridSystem : MonoSingleton<GridSystem> {
		[OdinSerialize]
		private GridData GridData;

		[OdinSerialize]
		private Vector3 OriginPosition;

		[OdinSerialize]
		private Vector2Int gridSize = new(10, 10);

		[OdinSerialize]
		private List<Vector2Int> defaultEnableChunk = new();

		[OdinSerialize]
		private Dictionary<Vector2Int, List<Vector2Int>> DefaultDisabledCell = new();

		protected override void Awake() {
			base.Awake();
			GenerateGrid();
		}

		private void Start() {
			FindSceneGridObject();
		}

		[Button]
		public void GenerateGrid() {
			GridData.chunkGrid = new Grid<Chunk>(
				gridSize.x,
				gridSize.y,
				Chunk.cellPerChunk * Chunk.cellSize,
				OriginPosition,
				CreateGridObject,
				Color.blue);
			GridData.chunkGrid.Init();
		}

		[Button]
		public void FindSceneGridObject() {
			var gridObjectList = FindObjectsOfType<GridObjectBase>();
			foreach (var gridObject in gridObjectList) {
				var cellPos = GridData.GetCellPos(gridObject.transform.position + new Vector3(1.25f, 0f, 1.25f));
				var chunk = GridData.GetChunk(cellPos);
				var localCellPos = GridData.GetLocalChunkPos(cellPos);

				gridObject.Chunk = chunk;
				gridObject.Position = localCellPos;
				gridObject.Direction = Direction.Down;
				chunk.cellGrid.GetGridObject(localCellPos).GridObject = gridObject;
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
