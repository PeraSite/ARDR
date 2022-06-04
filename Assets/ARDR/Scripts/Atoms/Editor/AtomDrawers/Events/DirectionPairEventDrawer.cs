#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `DirectionPair`. Inherits from `AtomDrawer&lt;DirectionPairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DirectionPairEvent))]
    public class DirectionPairEventDrawer : AtomDrawer<DirectionPairEvent> { }
}
#endif
