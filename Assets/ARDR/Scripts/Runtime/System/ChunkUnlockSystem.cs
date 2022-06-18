﻿using System.Collections.Generic;
using System.Linq;
using PeraCore.Runtime;
using PixelCrushers;
using TMPro;
using UnityAtoms;
using UnityEngine;

namespace ARDR {
	public class ChunkUnlockSystem : MonoSingleton<ChunkUnlockSystem> {
		[Header("설정")]
		public GridData GridData;

		[Header("오브젝트")]
		public ChunkUnlockUI UnlockUIPrefab;

		public UIPanel UnlockPanel;

		public TextMeshProUGUI Price;

		[Header("변수")]
		public LongVariable Money;

		private List<ChunkUnlockUI> _instantiated;
		private Chunk _currentChunk;

		private void Start() {
			_instantiated = new List<ChunkUnlockUI>();
			GridData.onAnyGridUpdate -= UpdateUnlockUI;
			GridData.onAnyGridUpdate += UpdateUnlockUI;
			UpdateUnlockUI();
		}

		private void OnDisable() {
			GridData.onAnyGridUpdate -= UpdateUnlockUI;
		}

		private void UpdateUnlockUI() {
			var chunkGrid = GridData.chunkGrid.GridArray;

			_instantiated.ForEach(obj => Destroy(obj.gameObject));
			_instantiated.Clear();

			foreach (var chunk in chunkGrid) {
				//이미 활성화된 청크라면 무시
				if (chunk.IsEnabled) continue;

				//주변에 활성화된 청크가 있다면
				if (GridData.GetNeighbourChunks(chunk.Position).Any(c => c.IsEnabled)) {
					var worldPosition = chunk.GetCenterWorldPosition();
					worldPosition.y = 2f;
					var instantiated = Instantiate(UnlockUIPrefab, worldPosition, Quaternion.identity, transform);
					instantiated.Init(chunk, OnOpenUIButtonClicked);
					_instantiated.Add(instantiated);
				}
			}
		}

		public void Close() {
			_currentChunk = null;
			UnlockPanel.Close();
		}

		private void OnOpenUIButtonClicked(Chunk chunk) {
			Price.text = $"{TMPIcons.Money} {CalculateChunkPrice()}";
			UnlockPanel.Open();
			_currentChunk = chunk;
		}

		public void OnUnlockButtonClicked() {
			if (_currentChunk == null) return;
			var chunkPos = _currentChunk.Position;
			if (GridData.IsEnabled(chunkPos)) return;

			var price = CalculateChunkPrice();
			if (Money.Value < price) {
				Toast.Show("돈이 부족합니다!");
				return;
			}
			Money.Subtract(price);
			GridSystem.Instance.UnlockChunk(chunkPos);
			UnlockPanel.Close();
		}

		private long CalculateChunkPrice() => 10000;
	}
}