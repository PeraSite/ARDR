using System.Collections.Generic;
using System.Linq;
using PixelCrushers;

namespace ARDR {
	public class GridDataSaver : Saver {
		public GridData Data;

		public override string RecordData() {
			var serialized = Data.chunkGrid.GridArray
				.Cast<Chunk>()
				.Where(chunk => chunk.IsEnabled)
				.Select(chunk => chunk.RecordData()).ToList();
			return SaveSystem.Serialize(serialized);
		}

		public override void ApplyData(string s) {
			if (s == null) return;
			var chunks = SaveSystem.Deserialize<List<Chunk.ChunkState>>(s);
			foreach (var chunkState in chunks) {
				var chunk = Data.chunkGrid.GetGridObject(chunkState.x, chunkState.y);
				chunk.SetEnabled(true);
				chunk.ApplyData(chunkState);
			}
		}

		public override void OnRestartGame() { }
	}
}
