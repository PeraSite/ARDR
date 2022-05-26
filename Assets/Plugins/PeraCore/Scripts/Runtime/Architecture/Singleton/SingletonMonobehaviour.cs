using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace PeraCore.Runtime {
	public class MonoSingleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour {
		protected virtual bool KeepAlive => true;

		public static T Instance =>
			_instance != null ? _instance :
			Application.isPlaying ? Initialize() : null;

		static T _instance;

		static bool _destroyed;

		protected static T Initialize() {
			if (_destroyed) return null;
			if (_instance != null) return _instance;

			if ((_instance = FindObjectOfType<T>()) != null) return _instance;

			var gameObject = new GameObject(typeof(T).Name);

			return gameObject.AddComponent<T>();
		}

		protected virtual void Awake() {
			Debug.Assert(_instance == null,
				"More than one singleton instance instantiated!", this);
			_instance = this as T;

			if (KeepAlive)
				DontDestroyOnLoad(gameObject);
		}

		protected virtual void OnDestroy() {
			_instance = null;
			_destroyed = true;
		}

		protected virtual void OnEnable() {
#if UNITY_EDITOR
			UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
		}

#if UNITY_EDITOR
		static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange stateChange) {
			if (stateChange == UnityEditor.PlayModeStateChange.EnteredEditMode) {
				UnityEditor.EditorApplication.playModeStateChanged -=
					OnPlayModeStateChanged;
				_destroyed = false;
			}
		}
#endif
	}
}
