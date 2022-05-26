using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.ActionResolvers;
using Sirenix.OdinInspector.Editor.ValueResolvers;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace PeraCore.Editor {
	public class InlineIconButtonAttributeDrawer : OdinAttributeDrawer<InlineIconButtonAttribute> {
		private ValueResolver<Texture2D> iconGetter;
		private ActionResolver clickAction;
		private ValueResolver<bool> showIfGetter;
		private bool show = true;

		protected override void Initialize() {
			iconGetter = ValueResolver.Get<Texture2D>(Property, Attribute.Icon);
			clickAction = ActionResolver.Get(Property, Attribute.Action);
			showIfGetter = ValueResolver.Get(Property, Attribute.ShowIf, true);
			show = showIfGetter.GetValue();
		}

		/// <summary>Draws the property.</summary>
		protected override void DrawPropertyLayout(GUIContent label) {
			if (clickAction.HasError) {
				clickAction.DrawError();
				CallNextDrawer(label);
			} else {
				if (Event.current.type == EventType.Layout)
					show = showIfGetter.GetValue();
				if (show) {
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.BeginVertical();
					CallNextDrawer(label);
					EditorGUILayout.EndVertical();
					var icon = iconGetter.GetValue();

					if (GUILayout.Button(icon, EditorStyles.miniButton, GUILayoutOptions.Height(10).Width(10))) {
						Property.RecordForUndo("Click Button");
						clickAction.DoActionForAllSelectionIndices();
					}
					EditorGUILayout.EndHorizontal();
				} else
					CallNextDrawer(label);
			}
		}
	}
}
