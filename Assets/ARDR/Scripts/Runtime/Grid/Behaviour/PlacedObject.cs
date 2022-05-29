using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public abstract class PlacedObject<TDataType> : MonoBehaviour, IPlacedObject
		where TDataType : PlaceableObjectData {
		[ShowInInspector]
		[FoldoutGroup("Grid Info")]
		public Chunk Chunk { get; set; }

		[ShowInInspector]
		[FoldoutGroup("Grid Info")]
		public TDataType ObjectData { get; private set; }

		[ShowInInspector]
		[FoldoutGroup("Grid Info")]
		public Vector2Int Origin { get; set; }

		[ShowInInspector]
		[FoldoutGroup("Grid Info")]
		public Direction Direction { get; set; }

		public bool isInitialized => Chunk != null;

		public void Setup(Chunk chunk, PlaceableObjectData placeableObjectData, Vector2Int _origin,
			Direction _direction) {
			Chunk = chunk;
			ObjectData = (TDataType) placeableObjectData;
			Origin = _origin;
			Direction = _direction;
		}

		public Vector3 GetCenterPosition() {
			var pos = transform.position;
			var (centerX, centerY) = ObjectData.GetCenterOffset(Direction);
			pos += new Vector3(centerX, 0, centerY);
			return pos;
		}

		public List<Vector2Int> GetGridPositionList() => ObjectData.GetGridPositionList(Origin, Direction);

		public void DestroySelf() => Destroy(gameObject);

		public virtual void OnInteract() { }
		public virtual void OnInit() { }

		public override string ToString() => ObjectData.Name;

		public abstract string Id { get; }
		public abstract Type Type { get; }

		public abstract string RecordData();
		public abstract void ApplyData(string value);
	}
}
