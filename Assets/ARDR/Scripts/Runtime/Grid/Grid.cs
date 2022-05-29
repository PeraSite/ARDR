using System;
using UnityEngine;

namespace ARDR {
	public class Grid<TGridObject> {
		public int width { get; private set; }
		public int height { get; private set; }
		public float cellSize { get; private set; }
		public Vector3 originWorldPosition { get; private set; }
		public Func<Grid<TGridObject>, int, int, TGridObject> gridObjectConstructor { get; private set; }
		public TGridObject[,] GridArray { get; private set; }
		public Color? debugColor { get; private set; }

		public Grid(int width, int height, float cellSize, Vector3 originPosition,
			Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, Color? debugColor = null) {
			this.width = width;
			this.height = height;
			this.cellSize = cellSize;
			originWorldPosition = originPosition;
			gridObjectConstructor = createGridObject;

			GridArray = new TGridObject[width, height];
			this.debugColor = debugColor;
		}

		public void Init() {
			for (var x = 0; x < GridArray.GetLength(0); x++) {
				for (var z = 0; z < GridArray.GetLength(1); z++) {
					GridArray[x, z] = gridObjectConstructor(this, x, z);
				}
			}

			if (debugColor.GetValue(out var color)) {
				var debugTextArray = new TextMesh[width, height];

				for (var x = 0; x < GridArray.GetLength(0); x++) {
					for (var z = 0; z < GridArray.GetLength(1); z++) {
						debugTextArray[x, z] = UtilsClass.CreateWorldText(GridArray[x, z]?.ToString(), null,
							GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, color,
							TextAnchor.MiddleCenter, TextAlignment.Center);
						Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), color, 100f);
						Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), color, 100f);
					}
				}
				Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), color, 100f);
				Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), color, 100f);

				OnGridObjectChanged += (sender, eventArgs) => {
					var gridObject = GridArray[eventArgs.x, eventArgs.z];
					debugTextArray[eventArgs.x, eventArgs.z].text = gridObject?.ToString();
				};
			}
		}

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
