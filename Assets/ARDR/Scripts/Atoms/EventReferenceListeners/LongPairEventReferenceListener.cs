using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Reference Listener of type `LongPair`. Inherits from `AtomEventReferenceListener&lt;LongPair, LongPairEvent, LongPairEventReference, LongPairUnityEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-orange")]
    [AddComponentMenu("Unity Atoms/Listeners/LongPair Event Reference Listener")]
    public sealed class LongPairEventReferenceListener : AtomEventReferenceListener<
        LongPair,
        LongPairEvent,
        LongPairEventReference,
        LongPairUnityEvent>
    { }
}
