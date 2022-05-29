using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public interface IPlacedObject {
		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Chunk Chunk { get; set; }

		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Vector2Int Origin { get; set; }

		[ShowInInspector]
		[BoxGroup("Grid Info")]
		public Direction Direction { get; set; }

		public void Setup(Chunk chunk, PlaceableObjectData placeableObjectData, Vector2Int origin, Direction direction);

		public void OnInteract();
		public void OnInit();
		public void DestroySelf();

		public string Id { get; }
		public Type Type { get; }

		public string RecordData();
		public void ApplyData(string value);
	}
}
