using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARDR {
	public class PlantData : PlaceableObjectData {
		[BoxGroup("Plant Setting")]
		[LabelText("구매 비용")]
		public int Price;

		[BoxGroup("Plant Setting")]
		[LabelText("초당 재화 수급량")]
		public int MoneyAmount;

		[BoxGroup("Plant Setting")]
		[LabelText("식물 종류")]
		public PlantType Type;

		[BoxGroup("Plant Settings")]
		[LabelText("설치 가능 테마")]
		public ThemeType Theme;

		[BoxGroup("Plant Setting")]
		[LabelText("필요 세계수 레벨")]
		public int RequireLevel;

		[BoxGroup("Plant Setting")]
		[LabelText("테마별 보정값")]
		public Dictionary<ThemeType, double> correctionValue = new();

		[BoxGroup("Plant Setting")]
		[LabelText("5분당 양분 소모량")]
		public int NutritionUsage;

		[BoxGroup("Plant Setting")]
		[LabelText("5분당 수분 소모량")]
		public int MoistureUsage;

		[BoxGroup("Plant Setting")]
		[LabelText("식물 모델링 파일")]
		public GameObject PlantModel;

		[BoxGroup("Plant Setting")]
		[LabelText("식물 높이")]
		public float PlantHeight;
	}

	[Flags]
	public enum ThemeType {
		숲 = 1 << 0,
		늪 = 1 << 1,
		사막 = 1 << 2,
		정글 = 1 << 3,
		타이가 = 1 << 4,
		전체 = 숲 | 늪 | 사막 | 정글 | 타이가,
	}
}
