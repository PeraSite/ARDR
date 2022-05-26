using System;
using System.Collections.Generic;
using System.Linq;

namespace PeraCore.Runtime {
	public class EnumUtil {
		public static IEnumerable<T> GetValues<T>() {
			return Enum.GetValues(typeof(T)).Cast<T>();
		}
	}
}
