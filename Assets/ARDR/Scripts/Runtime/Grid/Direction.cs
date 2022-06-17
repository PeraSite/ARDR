using System;
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

		public static Direction GetDirection(float rotation) {

			return (Mathf.Repeat(rotation, 360)) switch {
				>= 0 and < 90 => Direction.Left,
				>= 90 and < 180 => Direction.Down,
				>= 180 and < 270 => Direction.Right,
				>= 270 and < 360 => Direction.Up,
				_ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
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
