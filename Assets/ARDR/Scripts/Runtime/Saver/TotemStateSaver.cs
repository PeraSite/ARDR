using PixelCrushers;

namespace ARDR {
	public class TotemStateSaver : Saver {
		private Totem _totem;

		public override void Awake() {
			base.Awake();
			_totem = GetComponent<Totem>();
		}

		public override string RecordData() {
			return SaveSystem.Serialize(_totem.State);
		}

		public override void ApplyData(string s) {
			if (s == null) return;
			_totem.State = SaveSystem.Deserialize<TotemState>(s);
		}
	}
}
