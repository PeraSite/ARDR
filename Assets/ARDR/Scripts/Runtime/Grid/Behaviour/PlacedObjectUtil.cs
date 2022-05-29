using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ARDR {
	public static class PlacedObjectUtil {
		public static IPlacedObject Create<T>(Chunk chunk, Vector3 worldPosition, Vector2Int origin,
			Direction direction,
			T placeableObjectData) where T : PlaceableObjectData {
			var placedObjectTransform = Object.Instantiate(placeableObjectData.model, worldPosition,
				Quaternion.Euler(0, direction.GetRotationAngle(), 0));

			if (placedObjectTransform.TryGetComponent<IPlacedObject>(out var placedObject)) {
				placedObject.Setup(chunk, placeableObjectData, origin, direction);
				return placedObject;
			}
			throw new Exception(placeableObjectData.model.name + " doesn't have IPlacedObject!");
		}
	}
}
