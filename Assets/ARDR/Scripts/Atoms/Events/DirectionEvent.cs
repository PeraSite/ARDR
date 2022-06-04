using ARDR;
using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event of type `Direction`. Inherits from `AtomEvent&lt;Direction&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-cherry")]
    [CreateAssetMenu(menuName = "Unity Atoms/Events/Direction", fileName = "DirectionEvent")]
    public sealed class DirectionEvent : AtomEvent<Direction>
    {
    }
}
