using System;
using Sirenix.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

public static class App {
	public static bool isEditor = false;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Bootstrap() {
		var app = Object.Instantiate(Resources.Load("App"));
		app.name = "App";
		if (app.SafeIsUnityNull()) {
			throw new ApplicationException("Can't find main bootstrap App");
		}
		Object.DontDestroyOnLoad(app);
	}
}
