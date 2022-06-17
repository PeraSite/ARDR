using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `long`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(LongVariable))]
    public sealed class LongVariableEditor : AtomVariableEditor<long, LongPair> { }
}
