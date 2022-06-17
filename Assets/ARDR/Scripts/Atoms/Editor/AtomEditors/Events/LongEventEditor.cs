#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `long`. Inherits from `AtomEventEditor&lt;long, LongEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(LongEvent))]
    public sealed class LongEventEditor : AtomEventEditor<long, LongEvent> { }
}
#endif
