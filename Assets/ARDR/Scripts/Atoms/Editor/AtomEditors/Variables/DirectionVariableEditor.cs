using ARDR;
using UnityEditor;

namespace UnityAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `Direction`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(DirectionVariable))]
    public sealed class DirectionVariableEditor : AtomVariableEditor<Direction, DirectionPair> { }
}
