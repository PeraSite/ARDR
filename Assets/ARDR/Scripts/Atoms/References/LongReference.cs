using System;

namespace UnityAtoms
{
    /// <summary>
    /// Reference of type `long`. Inherits from `EquatableAtomReference&lt;long, LongPair, LongConstant, LongVariable, LongEvent, LongPairEvent, LongLongFunction, LongVariableInstancer, AtomCollection, AtomList&gt;`.
    /// </summary>
    [Serializable]
    public sealed class LongReference : EquatableAtomReference<
        long,
        LongPair,
        LongConstant,
        LongVariable,
        LongEvent,
        LongPairEvent,
        LongLongFunction,
        LongVariableInstancer>, IEquatable<LongReference>
    {
        public LongReference() : base() { }
        public LongReference(long value) : base(value) { }
        public bool Equals(LongReference other) { return base.Equals(other); }
    }
}
