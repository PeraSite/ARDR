#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `DirectionPair`. Inherits from `AtomEventEditor&lt;DirectionPair, DirectionPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(DirectionPairEvent))]
    public sealed class DirectionPairEventEditor : AtomEventEditor<DirectionPair, DirectionPairEvent> { }
}
#endif
