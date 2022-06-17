using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Variable of type `long`. Inherits from `EquatableAtomVariable&lt;long, LongPair, LongEvent, LongPairEvent, LongLongFunction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-lush")]
    [CreateAssetMenu(menuName = "Unity Atoms/Variables/Long", fileName = "LongVariable")]
    public sealed class LongVariable : EquatableAtomVariable<long, LongPair, LongEvent, LongPairEvent, LongLongFunction>
    {
        public void Add(int value) => Value += value;

        public void Add(long value) => Value += value;

        /// <summary>
        /// Add variable value to Variable.
        /// </summary>
        /// <param name="variable">Variable with value to add.</param>
        public void Add(AtomBaseVariable<long> variable) => Add(variable.Value);

        /// <summary>
        /// Add variable value to Variable.
        /// </summary>
        /// <param name="variable">Variable with value to add.</param>
        public void Add(AtomBaseVariable<int> variable) => Add(variable.Value);

        /// <summary>
        /// Subtract value from Variable.
        /// </summary>
        /// <param name="value">Value to subtract.</param>
        public void Subtract(long value) => Value -= value;

        /// <summary>
        /// Subtract variable value from Variable.
        /// </summary>
        /// <param name="variable">Variable with value to subtract.</param>
        public void Subtract(AtomBaseVariable<long> variable) => Subtract(variable.Value);

        /// <summary>
        /// Multiply variable by value.
        /// </summary>
        /// <param name="value">Value to multiple by.</param>
        public void MultiplyBy(long value) => Value *= value;

        /// <summary>
        /// Multiply variable by Variable value.
        /// </summary>
        /// <param name="variable">Variable with value to multiple by.</param>
        public void MultiplyBy(AtomBaseVariable<long> variable) => MultiplyBy(variable.Value);

        /// <summary>
        /// Divide Variable by value.
        /// </summary>
        /// <param name="value">Value to divide by.</param>
        public void DivideBy(long value) => Value /= value;

        /// <summary>
        /// Divide Variable by Variable value.
        /// </summary>
        /// <param name="variable">Variable value to divide by.</param>
        public void DivideBy(AtomBaseVariable<long> variable) => DivideBy(variable.Value);
    }
}
