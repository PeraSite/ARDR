using System;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference of type `long`. Inherits from `AtomEventReference&lt;long, LongVariable, LongEvent, LongVariableInstancer, LongEventInstancer&gt;`.
    /// </summary>
    [Serializable]
    public sealed class LongEventReference : AtomEventReference<
        long,
        LongVariable,
        LongEvent,
        LongVariableInstancer,
        LongEventInstancer>, IGetEvent 
    { }
}
