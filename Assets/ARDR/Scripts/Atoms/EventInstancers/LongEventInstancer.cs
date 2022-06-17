using UnityEngine;

namespace UnityAtoms
{
    /// <summary>
    /// Event Instancer of type `long`. Inherits from `AtomEventInstancer&lt;long, LongEvent&gt;`.
    /// </summary>
    [EditorIcon("atom-icon-sign-blue")]
    [AddComponentMenu("Unity Atoms/Event Instancers/Long Event Instancer")]
    public class LongEventInstancer : AtomEventInstancer<long, LongEvent> { }
}
