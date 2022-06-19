﻿using System.Collections.Generic;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ARDR {
	public class GridSystem : MonoSingleton<GridSystem> {
		public GridData GridData;

		[OdinSerialize]
		private Vector3 OriginPosition;

		[OdinSerialize]
		private Vector2Int gridSize = new(10, 10);

		[OdinSerialize]
		private List<Vector2Int> defaultEnableChunk = new();

		[OdinSerialize]
		private Dictionary<Vector2Int, List<Vector2Int>> DefaultDisabledCell = new();

		[OdinSerialize]
		private Dictionary<Vector2Int, ThemeType> ThemeData = new();

		protected override void Awake() {
			base.Awake();
			GenerateGrid();
		}

		private void Start() {
			FindSceneGridObject();
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			GridData.chunkGrid = null;
		}

		[Button]
		private void GenerateGrid() {
			GridData.chunkGrid = new Grid<Chunk>(
				gridSize.x,
				gridSize.y,
				Chunk.cellPerChunk * Chunk.cellSize,
				OriginPosition,
				CreateGridObject,
				Color.blue);
			GridData.chunkGrid.Init();
			GridData.OriginPosition = OriginPosition;
		}

		[Button]
		private void GenerateAllEnabledGrid() {
			GridData.chunkGrid = new Grid<Chunk>(
				gridSize.x,
				gridSize.y,
				Chunk.cellPerChunk * Chunk.cellSize,
				OriginPosition,
				(grid, chunkX, chunkZ) => new Chunk(GridData,
					chunkX,
					chunkZ,
					ThemeData.GetOrDefault(new Vector2Int(chunkX, chunkZ)),
					true,
					(cellX, cellZ) => true),
				Color.blue);
			GridData.chunkGrid.Init();
			GridData.OriginPosition = OriginPosition;
		}


		[Button]
		private void FindSceneGridObject(bool shouldDisableObject = true) {
			var gridObjectList = FindObjectsOfType<GridObjectBase>();
			foreach (var gridObject in gridObjectList) {
				var originCellPos = GridData.GetCellPos(gridObject.transform.position);
				var gridPositionList = gridObject.BaseData.GetGridPositionList(originCellPos, gridObject.Direction);

				var originChunk = GridData.GetChunk(originCellPos);
				var localCellPos = GridData.GetLocalChunkPos(originCellPos);

				gridObject.Chunk = originChunk;
				gridObject.Position = localCellPos;

				foreach (var subCellPos in gridPositionList) {
					var chunk = GridData.GetChunk(subCellPos);
					if (chunk == default) {
						Debug.Log("Can't find chunk", gridObject);
						break;
					}
					if (!chunk.IsEnabled) {
						if (shouldDisableObject) {
							gridObject.gameObject.SetActive(false);
							chunk.HiddenObjects.Add(gridObject);
						}
						break;
					}
					var localChunkPos = GridData.GetLocalChunkPos(subCellPos);
					chunk[localChunkPos].SetPlacedObject(gridObject);
				}
			}

		}

		[Button]
		public void UnlockChunk(Vector2Int position) {
			GridData.SetEnableChunk(position, true);
			FindSceneGridObject();
		}

		[Button]
		public void UnlockAllChunk() {
			foreach (var chunk in GridData.chunkGrid.GridArray) {
				chunk.SetEnabled(true);
			}
			FindSceneGridObject();
		}

		private Chunk CreateGridObject(Grid<Chunk> g, int chunkX, int chunkZ) =>
			new Chunk(GridData,
				chunkX,
				chunkZ,
				ThemeData.GetOrDefault(new Vector2Int(chunkX, chunkZ)),
				defaultEnableChunk.Contains(new Vector2Int(chunkX, chunkZ)),
				(cellX, cellZ) => {
					if (DefaultDisabledCell.TryGetValue(new Vector2Int(chunkX, chunkZ), out var disabledCells)) {
						if (disabledCells.Contains(new Vector2Int(cellX, cellZ))) {
							return false;
						}
					}
					return true;
				});

#if UNITY_EDITOR

		public bool OnlyDrawChunkGizmo;
		private void OnDrawGizmos() {
			if (GridData.chunkGrid == null) return;
			GridData.chunkGrid.DrawGizmo(Color.red);
			if (OnlyDrawChunkGizmo) return;
			foreach (var chunk in GridData.chunkGrid.GridArray) {
				chunk.cellGrid?.DrawGizmo(Color.blue);
			}
		}
#endif
	}
}
