using System.Collections;

namespace ARDR {
	public interface IGameMode {
		IEnumerator OnStart();
		IEnumerator OnEditorStart();
		IEnumerator OnEnd();
	}
}
