﻿using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace ARDR {
	public abstract class PlacedObject<TDataType> : SerializedMonoBehaviour, IPlacedObject
		where TDataType : PlaceableObjectData {
		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		[field: OdinSerialize]
		public Chunk Chunk { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		[field: OdinSerialize]
		public PlaceableObjectData BaseData { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		public TDataType Data => (TDataType) BaseData;

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		[field: SerializeField]
		public Vector2Int Position { get; set; }

		[FoldoutGroup("Grid Info", VisibleIf = "!IsInitialized")]
		[field: SerializeField]
		public Direction Direction { get; set; }

		public Transform Transform => transform;

		public bool IsInitialized => Chunk != null;
		public bool IsEditing { get; set; }

		public virtual void Setup(Chunk chunk, PlaceableObjectData placeableObjectData, Vector2Int _origin,
			Direction _direction) {
			Chunk = chunk;
			BaseData = placeableObjectData;
			Position = _origin;
			Direction = _direction;
		}

		public virtual void OnInstantiated(PlaceableObjectData objectData) { }

		public virtual void OnFirstPlaced() { }
		public virtual void OnRemove() {}

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
