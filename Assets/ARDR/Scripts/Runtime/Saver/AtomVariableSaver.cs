using System.Collections.Generic;
using System.Linq;
using PixelCrushers;
using Sirenix.Utilities;
using UnityAtoms;

namespace ARDR {
	public class AtomVariableSaver : Saver {
		public List<AtomBaseVariable> Variables;

		public override string RecordData() {
			var varDict = Variables.ToDictionary(
				GetAtomID,
				v => v.BaseValue
			);
			var recordData = SaveSystem.Serialize(varDict);
			return recordData;
		}

		public override void ApplyData(string data) {
			if (data == null) return;

			var varDict = SaveSystem.Deserialize<Dictionary<string, object>>(data);
			foreach (var variable in Variables) {
				var id = GetAtomID(variable);
				if (!varDict.ContainsKey(id)) continue;

				variable.BaseValue = varDict[id];
				variable.NotifyChanged();
			}
		}

		public override void OnRestartGame() {
			foreach (var variable in Variables) {
				variable.Reset(true);
			}
		}

		private static string GetAtomID(AtomBaseVariable v) => v.Id.IsNullOrWhitespace() ? v.name : v.Id;
	}
}
