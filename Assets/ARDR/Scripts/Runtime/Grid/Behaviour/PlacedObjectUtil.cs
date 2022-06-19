using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ARDR {
	public static class PlacedObjectUtil {
		public static IPlacedObject Create<T>(Chunk chunk, Vector3 worldPosition, Vector2Int origin,
			Direction direction,
			T placeableObjectData, string state = null) where T : PlaceableObjectData {
			var placedObjectTransform = Object.Instantiate(placeableObjectData.model, worldPosition,
				Quaternion.Euler(0, direction.GetRotationAngle(), 0));
			placedObjectTransform.SetTagRecursive("Placeable");

			if (placedObjectTransform.TryGetComponent<IPlacedObject>(out var placedObject)) {
				placedObjectTransform.name = placeableObjectData.Name;
				placedObject.Setup(chunk, placeableObjectData, origin, direction);
				placedObject.OnInstantiated(placeableObjectData);
				if (state != null)
					placedObject.ApplyData(state);
				return placedObject;
			}
			throw new Exception(placeableObjectData.model.name + " doesn't have IPlacedObject!");
		}
	}
}
