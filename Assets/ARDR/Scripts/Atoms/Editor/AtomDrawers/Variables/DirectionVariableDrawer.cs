#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `Direction`. Inherits from `AtomDrawer&lt;DirectionVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DirectionVariable))]
    public class DirectionVariableDrawer : VariableDrawer<DirectionVariable> { }
}
#endif
