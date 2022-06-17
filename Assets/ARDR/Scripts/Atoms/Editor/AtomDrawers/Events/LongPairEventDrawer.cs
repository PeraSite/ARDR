#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `LongPair`. Inherits from `AtomDrawer&lt;LongPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(LongPairEvent))]
    public class LongPairEventDrawer : AtomDrawer<LongPairEvent> { }
}
#endif
