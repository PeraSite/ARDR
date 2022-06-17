#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LongPair`. Inherits from `AtomEventEditor&lt;LongPair, LongPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(LongPairEvent))]
    public sealed class LongPairEventEditor : AtomEventEditor<LongPair, LongPairEvent> { }
}
#endif
