using ARDR;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class GridObjectBase : MonoBehaviour, IGridObject {
	public Chunk Chunk { get; set; }

	public abstract PlaceableObjectData BaseData { get; set; }

	public Vector2Int Position { get; set; }

	public abstract Direction Direction { get; set; }

	public Transform Transform => transform;

	public virtual void OnDiscovered() {

	}

#if UNITY_EDITOR
	[Button]
	private void SnapPosition() {
		var gridData = GridData.Instance;
		var cellPos = gridData.GetCellPos(transform.position);
		var chunk = gridData.GetChunk(cellPos);
		
		var worldOffset = new Vector3(-50, 0, -50);
		var snappedPosition = worldOffset + new Vector3(cellPos.x, 0, cellPos.y) * chunk.cellGrid.cellSize;
		transform.position = snappedPosition;
	}
#endif
}
