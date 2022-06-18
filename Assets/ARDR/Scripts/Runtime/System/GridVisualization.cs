using System;
using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class GridVisualization : MonoSingleton<GridVisualization> {
		public Dictionary<TilemapSprite, PixelUVCoords> PixelUVCoordsMap;
		public GridData Grid;
		public BoolVariable IsEditing;

		private Mesh mesh;
		private bool updateMesh;
		private Dictionary<TilemapSprite, UVCoords> uvCoordsDictionary;

		protected override void Awake() {
			base.Awake();
			mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = mesh;

			var texture = GetComponent<MeshRenderer>().material.mainTexture;
			float textureWidth = texture.width;
			float textureHeight = texture.height;

			uvCoordsDictionary = new Dictionary<TilemapSprite, UVCoords>();

			foreach (var pair in PixelUVCoordsMap) {
				var (sprite, pixelCoords) = pair;
				uvCoordsDictionary[sprite] = new UVCoords {
					uv00 = new Vector2(pixelCoords.uv00Pixels.x / textureWidth,
						pixelCoords.uv00Pixels.y / textureHeight),
					uv11 = new Vector2(pixelCoords.uv11Pixels.x / textureWidth,
						pixelCoords.uv11Pixels.y / textureHeight),
				};
			}
		}

		public void Show() {
			// gameObject.SetActive(true);
			updateMesh = true;
		}

		public void Hide() {
			// gameObject.SetActive(false);
			updateMesh = true;
		}

		private void Start() {
			UpdateHeatMapVisual();
			Grid.onAnyGridUpdate -= OnChunkChanged;
			Grid.onAnyGridUpdate += OnChunkChanged;
		}

		private void OnDisable() {
			Grid.onAnyGridUpdate -= OnChunkChanged;
		}

		private void OnChunkChanged() {
			updateMesh = true;
		}

		private void LateUpdate() {
			if (updateMesh) {
				updateMesh = false;
				UpdateHeatMapVisual();
			}
		}

		[Button]
		private void UpdateHeatMapVisual() {
			var grid = Grid.chunkGrid;
			var cellPerChunk = grid.width * grid.height * Chunk.cellPerChunk * Chunk.cellPerChunk +
			                   (grid.width * grid.height);
			MeshUtils.CreateEmptyMeshArrays(cellPerChunk,
				out var vertices, out var uv, out var triangles);

			for (var chunkX = 0; chunkX < grid.width; chunkX++) {
				for (var chunkZ = 0; chunkZ < grid.height; chunkZ++) {
					var chunk = grid.GetGridObject(chunkX, chunkZ);
					var chunkIndex = chunkX * grid.height + chunkZ;

					{
						var quadSize = new Vector3(1, 0, 1) * (grid.cellSize + 0.1f);

						var nearEnabled = Grid.GetNeighbourChunks(chunk.Position).Any(c => c.IsEnabled);
						var tilemapSprite = chunk.IsEnabled ? TilemapSprite.None : (nearEnabled ? TilemapSprite.NotEnabled : TilemapSprite.None);

						Vector2 gridUV00, gridUV11;
						if (tilemapSprite == TilemapSprite.None) {
							gridUV00 = Vector2.zero;
							gridUV11 = Vector2.zero;
							quadSize = Vector3.zero;
						} else {
							var uvCoords = uvCoordsDictionary[tilemapSprite];
							gridUV00 = uvCoords.uv00;
							gridUV11 = uvCoords.uv11;
						}
						MeshUtils.AddToMeshArraysXZ(vertices, uv, triangles, chunkIndex,
							grid.GetWorldPosition(chunkX, chunkZ) + quadSize * .5f, 0f, quadSize, gridUV00, gridUV11);
					}

					if (IsEditing.Value) {
						if (chunk.IsEnabled) {
							var cellGrid = chunk.cellGrid;

							for (var cellX = 0; cellX < cellGrid.width; cellX++) {
								for (var cellZ = 0; cellZ < cellGrid.height; cellZ++) {
									var worldCellPos = chunk.ToCellPos(new Vector2Int(cellX, cellZ));

									var index = Chunk.cellPerChunk * Chunk.cellPerChunk +
									            (worldCellPos.x * 25 + worldCellPos.y);
									var quadSize = new Vector3(1, 0, 1) * cellGrid.cellSize;
									var cell = cellGrid.GetGridObject(cellX, cellZ);

									var tilemapSprite = cell.IsPlaced()
										? TilemapSprite.CannotBuild
										: TilemapSprite.CanBuild;

									Vector2 gridUV00, gridUV11;
									if (tilemapSprite == TilemapSprite.None) {
										gridUV00 = Vector2.zero;
										gridUV11 = Vector2.zero;
										quadSize = Vector3.zero;
									} else {
										var uvCoords = uvCoordsDictionary[tilemapSprite];
										gridUV00 = uvCoords.uv00;
										gridUV11 = uvCoords.uv11;
									}

									MeshUtils.AddToMeshArraysXZ(vertices, uv, triangles, index,
										cellGrid.GetWorldPosition(cellX, cellZ) + quadSize * .5f, 0f, quadSize,
										gridUV00,
										gridUV11);
								}
							}
						}
					}
				}
			}
			mesh.vertices = vertices;
			mesh.uv = uv;
			mesh.triangles = triangles;
		}

		public enum TilemapSprite {
			None,
			CanBuild,
			CannotBuild,
			NotEnabled,
		}

		[Serializable]
		public struct PixelUVCoords {
			public Vector2Int uv00Pixels;
			public Vector2Int uv11Pixels;
		}

		private struct UVCoords {
			public Vector2 uv00;
			public Vector2 uv11;
		}
	}
}
