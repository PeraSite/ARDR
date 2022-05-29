using System;
using TMPro;

namespace ARDR {
	public class TestBuilding : PlacedObject<PlaceableObjectData> {
		public TextMeshPro text;
		public int score;

		private void Start() {
			text.text = score.ToString();
		}

		public void AddScore() {
			score++;
			text.text = score.ToString();
		}

		public override string Id { get; }
		public override Type Type { get; }

		public override string RecordData() {
			return score.ToString();
		}

		public override void ApplyData(string value) {
			var parsedScore = int.Parse(value);
			score = parsedScore;
		}
	}
}
