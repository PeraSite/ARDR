using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public interface IGridObject {
		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Chunk Chunk { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public PlaceableObjectData BaseData { get; set; }

		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Vector2Int Position { get; set; }

		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Direction Direction { get; set; }

		[BoxGroup("Grid Info")]
		public Transform Transform { get; }

		public void OnInteract() { }

		public void DestroySelf() { }

		public List<Vector2Int> GetGridPositionList() => BaseData.GetGridPositionList(Position, Direction);
	}
}
