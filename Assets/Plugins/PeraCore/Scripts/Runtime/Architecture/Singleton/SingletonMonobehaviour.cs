using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace PeraCore.Runtime {
	public abstract class MonoSingleton : SerializedMonoBehaviour {
		protected static MonoBehaviour InstanceWeak { get; set; }

		protected virtual bool KeepAlive => true;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetInstance() {
			InstanceWeak = null;
		}
	}

	public abstract class MonoSingleton<T> : MonoSingleton where T : SerializedMonoBehaviour {
		public static T Instance {
			get {
				if (InstanceWeak.SafeIsUnityNull()) {
					InstanceWeak = FindObjectOfType<T>();
				}
				return InstanceWeak as T;
			}
		}

		protected virtual void Awake() {
			if (KeepAlive)
				DontDestroyOnLoad(gameObject);
		}

		protected virtual void OnDestroy() {
			InstanceWeak = null;
		}
	}
}
