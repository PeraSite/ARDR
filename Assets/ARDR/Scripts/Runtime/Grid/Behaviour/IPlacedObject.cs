using UnityEngine;

namespace ARDR {
	public interface IPlacedObject : IGridObject {
		public bool IsEditing { get; set; }

		public void OnInstantiated(PlaceableObjectData objectData);
		public void OnFirstPlaced();
		public void OnRemove() {}

		public void OnEditStart();
		public void OnEditEnd();

		public void Setup(Chunk chunk, PlaceableObjectData placeableObjectData, Vector2Int origin, Direction direction);

		public string RecordData();
		public void ApplyData(string value);
	}
}
