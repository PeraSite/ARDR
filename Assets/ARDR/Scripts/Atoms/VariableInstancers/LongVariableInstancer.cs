using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Variable Instancer of type `long`. Inherits from `AtomVariableInstancer&lt;LongVariable, LongPair, long, LongEvent, LongPairEvent, LongLongFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-hotpink")]
    [AddComponentMenu("Unity Atoms/Variable Instancers/Long Variable Instancer")]
    public class LongVariableInstancer : AtomVariableInstancer<
        LongVariable,
        LongPair,
        long,
        LongEvent,
        LongPairEvent,
        LongLongFunction>
    { }
}
