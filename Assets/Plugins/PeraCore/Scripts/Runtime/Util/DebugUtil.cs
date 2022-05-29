using UnityEngine;

public static class DebugUtil {
	public static void Log(object obj) {
		if (!Debug.isDebugBuild) return;
		Debug.Log(obj);
	}

	public static void Log(object obj, Object context) {
		if (!Debug.isDebugBuild) return;
		Debug.Log(obj, context);
	}
}
