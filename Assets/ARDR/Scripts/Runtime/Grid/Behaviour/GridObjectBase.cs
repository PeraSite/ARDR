using ARDR;
using UnityEngine;

public abstract class GridObjectBase : MonoBehaviour, IGridObject {
	public Chunk Chunk { get; set; }

	public abstract PlaceableObjectData BaseData { get; set; }

	public Vector2Int Position { get; set; }

	public abstract Direction Direction { get; set; }

	public Transform Transform => transform;
}
