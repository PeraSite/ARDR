using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Value List of type `long`. Inherits from `AtomValueList&lt;long, LongEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-piglet")]
    [CreateAssetMenu(menuName = "Unity Atoms/Value Lists/Long", fileName = "LongValueList")]
    public sealed class LongValueList : AtomValueList<long, LongEvent> { }
}
