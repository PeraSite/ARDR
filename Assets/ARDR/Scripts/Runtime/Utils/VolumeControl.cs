using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace ARDR {
	public class VolumeControl : MonoBehaviour {
		public AudioSource Audio;
		public FloatVariable Volume;

		private void Update() {
			Audio.volume = Volume.Value;
		}
	}
}
