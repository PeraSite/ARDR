using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public abstract class PlacedObject<TDataType> : MonoBehaviour, IPlacedObject
		where TDataType : PlaceableObjectData {
		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public Chunk Chunk { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public PlaceableObjectData BaseData { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public TDataType Data => (TDataType) BaseData;

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public Vector2Int Origin { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public Direction Direction { get; set; }

		public bool IsInitialized => Chunk != null;
		public bool IsEditing { get; set; }

		public void Setup(Chunk chunk, PlaceableObjectData placeableObjectData, Vector2Int _origin,
			Direction _direction) {
			Chunk = chunk;
			BaseData = placeableObjectData;
			Origin = _origin;
			Direction = _direction;
		}

		public virtual void OnInteract() { }
		public virtual void OnInit() { }
		public void DestroySelf() => Destroy(gameObject);

		public virtual void OnEditStart() { }
		public virtual void OnEditEnd() { }

		public override string ToString() => Data.Name;

		public virtual string RecordData() {
			return "";
		}

		public virtual void ApplyData(string value) { }
	}
}
