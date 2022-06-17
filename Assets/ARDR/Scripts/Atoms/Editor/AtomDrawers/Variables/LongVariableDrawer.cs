#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `long`. Inherits from `AtomDrawer&lt;LongVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(LongVariable))]
    public class LongVariableDrawer : VariableDrawer<LongVariable> { }
}
#endif
