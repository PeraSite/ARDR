using System;
using UnityEngine.Events;

namespace UnityAtoms
{
    /// <summary>
    /// None generic Unity Event of type `long`. Inherits from `UnityEvent&lt;long&gt;`.
    /// </summary>
    [Serializable]
    public sealed class LongUnityEvent : UnityEvent<long> { }
}
