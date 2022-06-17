using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `LongPair`. Inherits from `AtomEvent&lt;LongPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/LongPair", fileName = "LongPairEvent")]
    public sealed class LongPairEvent : AtomEvent<LongPair>
    {
    }
}
