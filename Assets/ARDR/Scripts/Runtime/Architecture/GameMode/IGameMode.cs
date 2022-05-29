using System.Collections;
using UnityEngine;

namespace ARDR {
	public interface IGameMode {
		IEnumerator OnStart();
		IEnumerator OnEditorStart();
		IEnumerator OnEnd();
	}

	public abstract class GameModeBase : ScriptableObject, IGameMode {
		public abstract IEnumerator OnStart();
		public abstract IEnumerator OnEditorStart();
		public abstract IEnumerator OnEnd();
	}
}
