using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace PeraCore.Editor {
	[InitializeOnLoad]
	public class VersionManager {
		private static bool AutoIncrease;
		private const string AutoIncreaseMenuName = "Build/Auto Increase Build Version";

		static VersionManager() {
			AutoIncrease = EditorPrefs.GetBool(AutoIncreaseMenuName, true);
		}

		/////////////////////// Settings ///////////////////////

		[InitializeOnLoadMethod]
		private static void CheckVersionLength() {
			var lines = PlayerSettings.bundleVersion.Split('.');
			if (lines.Length >= 3) return;
			PlayerSettings.bundleVersion = "0.0.1";
			PlayerSettings.Android.bundleVersionCode = 1;
		}


		[MenuItem(AutoIncreaseMenuName, false, 1)]
		private static void SetAutoIncrease() {
			AutoIncrease = !AutoIncrease;
			EditorPrefs.SetBool(AutoIncreaseMenuName, AutoIncrease);
			Debug.Log("Auto Increase : " + AutoIncrease);
		}

		[MenuItem(AutoIncreaseMenuName, true)]
		private static bool SetAutoIncreaseValidate() {
			Menu.SetChecked(AutoIncreaseMenuName, AutoIncrease);
			return true;
		}


		[MenuItem("Build/Check Current Version", false, 2)]
		private static void CheckCurrentVersion() {
			CheckVersionLength(); //추가
			Debug.Log("Build v" + PlayerSettings.bundleVersion +
			          " (" + PlayerSettings.Android.bundleVersionCode + ")");
		}

		[PostProcessBuild(1)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
			CheckVersionLength(); //추가
			if (AutoIncrease) IncreaseBuild();
		}

		/////////////////////// Increase ///////////////////////

		[MenuItem("Build/Increase Major Version", false, 51)]
		private static void IncreaseMajor() {
			var lines = PlayerSettings.bundleVersion.Split('.');
			EditVersion(1, -int.Parse(lines[1]), -int.Parse(lines[2]));
		}

		[MenuItem("Build/Increase Minor Version", false, 52)]
		private static void IncreaseMinor() {
			var lines = PlayerSettings.bundleVersion.Split('.');
			EditVersion(0, 1, -int.Parse(lines[2]));
		}

		[MenuItem("Build/Increase Build Version", false, 53)]
		private static void IncreaseBuild() {
			EditVersion(0, 0, 1);
		}

		static void EditVersion(int majorIncr, int minorIncr, int buildIncr) {
			var lines = PlayerSettings.bundleVersion.Split('.');

			var MajorVersion = int.Parse(lines[0]) + majorIncr;
			var MinorVersion = int.Parse(lines[1]) + minorIncr;
			var Build = int.Parse(lines[2]) + buildIncr;

			PlayerSettings.bundleVersion = MajorVersion.ToString("0") + "." +
			                               MinorVersion.ToString("0") + "." +
			                               Build.ToString("0");
			PlayerSettings.Android.bundleVersionCode = MajorVersion * 10000 + MinorVersion * 1000 + Build;
			CheckCurrentVersion();
		}
	}
}
