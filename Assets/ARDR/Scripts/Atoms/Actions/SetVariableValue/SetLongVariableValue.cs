using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Set variable value Action of type `long`. Inherits from `SetVariableValue&lt;long, LongPair, LongVariable, LongConstant, LongReference, LongEvent, LongPairEvent, LongVariableInstancer&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-purple")]
    [CreateAssetMenu(menuName = "Unity Atoms/Actions/Set Variable Value/Long", fileName = "SetLongVariableValue")]
    public sealed class SetLongVariableValue : SetVariableValue<
        long,
        LongPair,
        LongVariable,
        LongConstant,
        LongReference,
        LongEvent,
        LongPairEvent,
        LongLongFunction,
        LongVariableInstancer>
    { }
}
