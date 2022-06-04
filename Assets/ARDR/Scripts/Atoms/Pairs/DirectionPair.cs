using System;
using ARDR;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// IPair of type `&lt;Direction&gt;`. Inherits from `IPair&lt;Direction&gt;`.
    /// </summary>
    [Serializable]
    public struct DirectionPair : IPair<Direction>
    {
        public Direction Item1 { get => _item1; set => _item1 = value; }
        public Direction Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private Direction _item1;
        [SerializeField]
        private Direction _item2;

        public void Deconstruct(out Direction item1, out Direction item2) { item1 = Item1; item2 = Item2; }
    }
}