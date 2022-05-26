using System;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace PeraCore.Editor {
	public class InlineIconButtonAttribute : Attribute {
		public string Action;

		public string Icon;

		public string ShowIf;

		public InlineIconButtonAttribute(string action, string icon) {
			Action = action;
			Icon = icon;
		}
	}
}
