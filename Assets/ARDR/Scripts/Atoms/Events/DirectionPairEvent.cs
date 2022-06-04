using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `DirectionPair`. Inherits from `AtomEvent&lt;DirectionPair&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/DirectionPair", fileName = "DirectionPairEvent")]
    public sealed class DirectionPairEvent : AtomEvent<DirectionPair>
    {
    }
}
