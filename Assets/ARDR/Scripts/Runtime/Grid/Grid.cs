using System;
using UnityEngine;
#if UNITY_EDITOR
using PeraCore.Editor;
#endif

namespace ARDR {
	public class Grid<TGridObject> {
		public int width;
		public int height;
		public float cellSize;

		public TGridObject[,] GridArray;

		public Vector3 originWorldPosition;

		private Func<Grid<TGridObject>, int, int, TGridObject> gridObjectConstructor;

		public Grid(int width, int height, float cellSize, Vector3 originPosition,
			Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, Color? debugColor = null) {
			this.width = width;
			this.height = height;
			this.cellSize = cellSize;
			originWorldPosition = originPosition;
			gridObjectConstructor = createGridObject;

			GridArray = new TGridObject[width, height];
		}

		public void Init() {
			for (var x = 0; x < GridArray.GetLength(0); x++) {
				for (var z = 0; z < GridArray.GetLength(1); z++) {
					GridArray[x, z] = gridObjectConstructor(this, x, z);
				}
			}
		}

#if UNITY_EDITOR
		public void DrawGizmo(Color color) {
			Gizmos.color = color;
			for (var x = 0; x < GridArray.GetLength(0); x++) {
				for (var z = 0; z < GridArray.GetLength(1); z++) {
					var worldPosition = GetWorldPosition(x, z);
					worldPosition += new Vector3(cellSize, 0, cellSize) * 0.5f;
					var text = GridArray[x, z].ToString();
					if (text == "") text = $"({x}, {z})";
					GizmoUtils.DrawString(text, worldPosition, color);
					Gizmos.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1));
					Gizmos.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z));
				}
			}
			Gizmos.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height));
			Gizmos.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height));
		}
#endif

		public Vector3 GetWorldPosition(int x, int z) => new Vector3(x, 0, z) * cellSize + originWorldPosition;

		public Vector3 GetWorldPosition(Vector2Int gridPos) =>
			new Vector3(gridPos.x, 0, gridPos.y) * cellSize + originWorldPosition;

		public Vector2Int GetGridPosition(Vector3 worldPosition) {
			var x = Mathf.FloorToInt((worldPosition - originWorldPosition).x / cellSize);
			var z = Mathf.FloorToInt((worldPosition - originWorldPosition).z / cellSize);
			return new Vector2Int(x, z);
		}

		public void SetGridObject(Vector2Int gridPos, TGridObject value) {
			var (x, z) = gridPos;
			if (x < 0 || z < 0 || x >= width || z >= height) return;

			GridArray[x, z] = value;
			TriggerGridObjectChanged(gridPos);
		}

		public void SetGridObject(Vector3 worldPosition, TGridObject value) {
			var gridPos = GetGridPosition(worldPosition);
			SetGridObject(gridPos, value);
		}

		public TGridObject GetGridObject(int x, int z) {
			return x >= 0 && z >= 0 && x < width && z < height ? GridArray[x, z] : default;
		}

		public TGridObject GetGridObject(Vector2Int gridPos) {
			return GetGridObject(gridPos.x, gridPos.y);
		}

		public TGridObject GetGridObject(Vector3 worldPos) {
			var gridPos = GetGridPosition(worldPos);
			return GetGridObject(gridPos);
		}

		public Vector2Int ClampGridPosition(Vector2Int gridPos) {
			return new Vector2Int(
				Mathf.Clamp(gridPos.x, 0, width - 1),
				Mathf.Clamp(gridPos.y, 0, height - 1)
			);
		}


#region Event stuff

		public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

		public class OnGridObjectChangedEventArgs : EventArgs {
			public int x;
			public int z;
		}

		public void TriggerGridObjectChanged(Vector2Int gridPos) {
			OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs {x = gridPos.x, z = gridPos.y});
		}

#endregion
	}
}
