#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `long`. Inherits from `AtomDrawer&lt;LongConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(LongConstant))]
    public class LongConstantDrawer : VariableDrawer<LongConstant> { }
}
#endif
