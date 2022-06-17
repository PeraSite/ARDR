using System;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `LongPair`. Inherits from `AtomEventReference&lt;LongPair, LongVariable, LongPairEvent, LongVariableInstancer, LongPairEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class LongPairEventReference : AtomEventReference<
        LongPair,
        LongVariable,
        LongPairEvent,
        LongVariableInstancer,
        LongPairEventInstancer>, IGetEvent 
    { }
}
