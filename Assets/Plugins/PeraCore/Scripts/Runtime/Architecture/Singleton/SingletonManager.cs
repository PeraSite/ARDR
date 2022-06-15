using System;
using System.Collections.Generic;
using UnityEngine;

namespace PeraCore.Runtime {
	public static class SingletonManager {
		public static readonly List<Action> list = new();

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInstance() {
			list.ForEach(action => action());
			list.Clear();
		}
	}

}
