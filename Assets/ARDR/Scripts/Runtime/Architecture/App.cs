using System;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ARDR {
	public static class App {
		public static bool isEditor = false;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap() {
			var app = Object.Instantiate(Resources.Load<GameObject>("App"));
			app.name = "App";
			if (app.SafeIsUnityNull()) {
				throw new ApplicationException("Can't find main bootstrap App");
			}
			Object.DontDestroyOnLoad(app);
		}
	}
}
