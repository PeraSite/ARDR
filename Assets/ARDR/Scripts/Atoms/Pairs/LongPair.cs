using System;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// IPair of type `&lt;long&gt;`. Inherits from `IPair&lt;long&gt;`.
    /// </summary>
    [Serializable]
    public struct LongPair : IPair<long>
    {
        public long Item1 { get => _item1; set => _item1 = value; }
        public long Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private long _item1;
        [SerializeField]
        private long _item2;

        public void Deconstruct(out long item1, out long item2) { item1 = Item1; item2 = Item2; }
    }
}