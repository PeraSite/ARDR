#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `long`. Inherits from `AtomDrawer&lt;LongEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(LongEvent))]
    public class LongEventDrawer : AtomDrawer<LongEvent> { }
}
#endif