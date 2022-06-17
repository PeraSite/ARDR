#if UNITY_2019_1_OR_NEWER
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `long`. Inherits from `AtomDrawer&lt;LongValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(LongValueList))]
    public class LongValueListDrawer : AtomDrawer<LongValueList> { }
}
#endif
