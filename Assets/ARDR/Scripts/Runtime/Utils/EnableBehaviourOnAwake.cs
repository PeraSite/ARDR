using System;
using UnityEngine;

namespace ARDR {
	public class EnableBehaviourOnAwake : MonoBehaviour {
		public Behaviour Behaviour;

		private void Awake() {
			Behaviour.enabled = true;
		}
	}
}
