using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public interface IPlacedObject {
		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Chunk Chunk { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public PlaceableObjectData BaseData { get; set; }

		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Vector2Int Origin { get; set; }

		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Direction Direction { get; set; }

		public bool IsEditing { get; set; }

		public void Setup(Chunk chunk, PlaceableObjectData placeableObjectData, Vector2Int origin, Direction direction);

		public void OnInteract();
		public void OnInit();
		public void DestroySelf();

		public void OnEditStart();
		public void OnEditEnd();

		public string RecordData();
		public void ApplyData(string value);

		public List<Vector2Int> GetGridPositionList() => BaseData.GetGridPositionList(Origin, Direction);
	}
}
