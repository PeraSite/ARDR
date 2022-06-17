using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `long`. Inherits from `AtomEvent&lt;long&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Long", fileName = "LongEvent")]
    public sealed class LongEvent : AtomEvent<long>
    {
    }
}
