// Copyright (c) 2020 Nementic Games GmbH.

using UnityEditor;
using UnityEngine;

namespace Nementic.SelectionUtility
{
	internal class BoolSetting : Setting<bool>
	{
		public BoolSetting(string key, bool defaultValue = false) : base(key, defaultValue)
		{
		}

		protected override bool DrawProperty(GUIContent label, bool value)
		{
			return EditorGUILayout.Toggle(label, value);
		}

		protected override bool LoadValue()
		{
			return EditorPrefs.GetBool(key, defaultValue);
		}

		protected override void SaveValue(bool value)
		{
			EditorPrefs.SetBool(key, value);
		}
	}
}
