using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sheet2SO.Editor {
	public abstract class SheetSerializer : SerializedScriptableObject {
		public abstract Type Type { get; }

		public abstract string GetName(List<string> row);

		public abstract void DeserializeWeak(List<string> row, ScriptableObject instance);
	}

	public abstract class SheetSerializer<T> : SheetSerializer where T : ScriptableObject {
		public override Type Type => typeof(T);

		public override void DeserializeWeak(List<string> row, ScriptableObject instance) {
			Deserialize(row, instance as T);
		}

		public abstract void Deserialize(List<string> row, T plant);
	}
}
