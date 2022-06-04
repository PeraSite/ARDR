#if UNITY_2019_1_OR_NEWER
using ARDR;
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `Direction`. Inherits from `AtomEventEditor&lt;Direction, DirectionEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(DirectionEvent))]
    public sealed class DirectionEventEditor : AtomEventEditor<Direction, DirectionEvent> { }
}
#endif
