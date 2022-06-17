using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Constant of type `long`. Inherits from `AtomBaseVariable&lt;long&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-teal")]
    [CreateAssetMenu(menuName = "Unity Atoms/Constants/Long", fileName = "LongConstant")]
    public sealed class LongConstant : AtomBaseVariable<long> { }
}
