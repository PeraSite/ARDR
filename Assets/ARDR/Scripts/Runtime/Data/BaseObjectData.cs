using System.IO;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ARDR {
	public abstract class BaseObjectData<T> : SerializedScriptableObject {
		[HorizontalGroup("General Settings/Name")]
		[VerticalGroup("General Settings/Name/Description")]
		[BoxGroup("General Settings", order: -999f)]
		[LabelWidth(60), Required, OdinSerialize]
		public string Name { get; set; }

		[HorizontalGroup("General Settings/Name", 100)]
		[BoxGroup("General Settings")]
		[HideLabel, InlineProperty, PreviewField(100, ObjectFieldAlignment.Right), OdinSerialize, AssetsOnly]
		public T Display { get; set; }

		[BoxGroup("General Settings")]
		[VerticalGroup("General Settings/Name/Description")]
		[Title("Description")]
		[HideLabel, MultiLineProperty, OdinSerialize]
		public string Description { get; set; }


#if UNITY_EDITOR
		private void OnValidate() {
			var assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
			Name = Path.GetFileNameWithoutExtension(assetPath);
		}
#endif
	}
}
