using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace ARDR {
	public abstract class BaseObjectData<T> : SerializedScriptableObject {
		[HorizontalGroup("General Settings/Name")]
		[VerticalGroup("General Settings/Name/Description")]
		[BoxGroup("General Settings", order: -999f)]
		[LabelWidth(60), Required, OdinSerialize]
		public string Name { get; private set; }

		[HorizontalGroup("General Settings/Name", 100)]
		[BoxGroup("General Settings")]
		[HideLabel, InlineProperty, PreviewField(100, ObjectFieldAlignment.Right), OdinSerialize, AssetsOnly]
		public T Object { get; private set; }

		[BoxGroup("General Settings")]
		[VerticalGroup("General Settings/Name/Description")]
		[Title("Description")]
		[HideLabel, MultiLineProperty, OdinSerialize]
		public string Description { get; private set; }
	}
}
