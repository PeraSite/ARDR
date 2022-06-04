using ARDR;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Variable of type `Direction`. Inherits from `AtomVariable&lt;Direction, DirectionPair, DirectionEvent, DirectionPairEvent, DirectionDirectionFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Direction", fileName = "DirectionVariable")]
    public sealed class DirectionVariable : AtomVariable<Direction, DirectionPair, DirectionEvent, DirectionPairEvent, DirectionDirectionFunction>
    {
        protected override bool ValueEquals(Direction other) {
            return _value == other;
        }
    }
}
