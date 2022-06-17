using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `long`. Inherits from `AtomEventReferenceListener&lt;long, LongEvent, LongEventReference, LongUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/Long Event Reference Listener")]
    public sealed class LongEventReferenceListener : AtomEventReferenceListener<
        long,
        LongEvent,
        LongEventReference,
        LongUnityEvent>
    { }
}
