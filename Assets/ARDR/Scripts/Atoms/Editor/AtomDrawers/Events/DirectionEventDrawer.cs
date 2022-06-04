#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Direction`. Inherits from `AtomDrawer&lt;DirectionEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DirectionEvent))]
    public class DirectionEventDrawer : AtomDrawer<DirectionEvent> { }
}
#endif
