using ARDR;
using UnityEngine;

public abstract class GridObjectBase : MonoBehaviour, IGridObject {
	[field: SerializeField]
	public Chunk Chunk { get; set; }

	[field: SerializeField]
	public PlaceableObjectData BaseData { get; set; }

	[field: SerializeField]
	public Vector2Int Position { get; set; }

	[field: SerializeField]
	public Direction Direction { get; set; }
}
