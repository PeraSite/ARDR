using UnityEngine;

namespace ARDR {
	public enum Direction {
		Down,
		Left,
		Up,
		Right
	}

	public static class DirectionUtil {
		public static Direction GetNextDir(this Direction direction) {
			return direction switch {
				Direction.Left => Direction.Up,
				Direction.Up => Direction.Right,
				Direction.Right => Direction.Down,
				_ => Direction.Left
			};
		}

		public static int GetRotationAngle(this Direction direction) {
			return direction switch {
				Direction.Left => 90,
				Direction.Up => 180,
				Direction.Right => 270,
				_ => 0
			};
		}

		public static Quaternion GetRotationRotation(this Direction direction) {
			return Quaternion.Euler(0, GetRotationAngle(direction), 0);
		}
	}
}
