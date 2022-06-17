using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class PlaceableObjectData : BaseObjectData<Sprite> {
		[BoxGroup("Placeable Setting")]
		[LabelWidth(60)]
		[AssetSelector(Paths = "Assets/ARDR/Resources/Prefabs/Placeable", Filter = "t:prefab"), AssetsOnly, Required]
		public GameObject model;

		[BoxGroup("Placeable Setting")]
		[LabelWidth(60), Required]
		public Vector2Int gridSize;

		public Vector2Int GetRotationOffset(Direction direction) =>
			direction switch {
				Direction.Left => new Vector2Int(0, gridSize.x),
				Direction.Up => new Vector2Int(gridSize.x, gridSize.y),
				Direction.Right => new Vector2Int(gridSize.y, 0),
				_ => new Vector2Int(0, 0)
			};

		public Vector2 GetCenterOffset(Direction direction) {
			var gridVector = new Vector2(gridSize.x, gridSize.y);
			var (xScaled, yScaled) = gridVector * Chunk.cellSize / 2f;

			return direction switch {
				Direction.Left => new Vector2(xScaled, -yScaled),
				Direction.Up => new Vector2(-xScaled, -yScaled),
				Direction.Right => new Vector2(-xScaled, +yScaled),
				_ => new Vector2(xScaled, yScaled)
			};
		}

		public List<Vector2Int> GetGridPositionList(Vector2Int offset, Direction direction) {
			var gridPositionList = new List<Vector2Int>();
			switch (direction) {
				case Direction.Down:
					for (var x = 0; x < gridSize.x; x++) {
						for (var y = 0; y < gridSize.y; y++) {
							gridPositionList.Add(offset - new Vector2Int(x, y));
						}
					}
					break;
				case Direction.Up:
					for (var x = 0; x < gridSize.x; x++) {
						for (var y = 0; y < gridSize.y; y++) {
							gridPositionList.Add(offset + new Vector2Int(x, y));
						}
					}
					break;
				case Direction.Left:
					for (var x = 0; x < gridSize.y; x++) {
						for (var y = 0; y < gridSize.x; y++) {
							gridPositionList.Add(offset - new Vector2Int(x, y));
						}
					}
					break;
				case Direction.Right:
					for (var x = 0; x < gridSize.y; x++) {
						for (var y = 0; y < gridSize.x; y++) {
							gridPositionList.Add(offset + new Vector2Int(x, y));
						}
					}
					break;
			}
			return gridPositionList;
		}
	}
}
