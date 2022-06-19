using System.IO;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewSoundEffect", menuName = "Audio/New Sound Effect")]
public class SoundEffectSO : ScriptableObject {
#region config

	private static readonly float SEMITONES_TO_PITCH_CONVERSION_UNIT = 1.05946f;

	[Required]
	public AudioClip[] clips;

	public FloatVariable VolumeMultiplier;

	[MinMaxSlider(0, 3)]
	[BoxGroup("config")]
	public Vector2 volume = new Vector2(0.5f, 0.5f);

	//Pitch / Semitones
	[LabelWidth(100)]
	[HorizontalGroup("config/pitch")]
	public bool useSemitones;

	[HideLabel]
	[ShowIf("useSemitones")]
	[HorizontalGroup("config/pitch")]
	[MinMaxSlider(-10, 10)]
	[OnValueChanged("SyncPitchAndSemitones")]
	public Vector2Int semitones = new Vector2Int(0, 0);

	[HideLabel]
	[HideIf("useSemitones")]
	[MinMaxSlider(0, 3)]
	[HorizontalGroup("config/pitch")]
	[OnValueChanged("SyncPitchAndSemitones")]
	public Vector2 pitch = new Vector2(1, 1);

	[BoxGroup("config")]
	[SerializeField]
	private SoundClipPlayOrder playOrder;

	[DisplayAsString]
	[BoxGroup("config")]
	[SerializeField]
	private int playIndex = 0;

#endregion

#region PreviewCode

#if UNITY_EDITOR
	private AudioSource previewer;

	private void OnEnable() {
		previewer = EditorUtility
			.CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave,
				typeof(AudioSource))
			.GetComponent<AudioSource>();
	}

	private void OnDisable() {
		DestroyImmediate(previewer.gameObject);
	}


	[ButtonGroup("previewControls")]
	[GUIColor(.3f, 1f, .3f)]
	[Button(ButtonSizes.Gigantic)]
	private void PlayPreview() {
		Play(previewer);
	}

	[ButtonGroup("previewControls")]
	[GUIColor(1, .3f, .3f)]
	[Button(ButtonSizes.Gigantic)]
	[EnableIf("@previewer.isPlaying")]
	private void StopPreview() {
		previewer.Stop();
	}
#endif

#endregion

	private void SyncPitchAndSemitones() {
		if (useSemitones) {
			pitch.x = Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, semitones.x);
			pitch.y = Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, semitones.y);
		} else {
			semitones.x = Mathf.RoundToInt(Mathf.Log10(pitch.x) / Mathf.Log10(SEMITONES_TO_PITCH_CONVERSION_UNIT));
			semitones.y = Mathf.RoundToInt(Mathf.Log10(pitch.y) / Mathf.Log10(SEMITONES_TO_PITCH_CONVERSION_UNIT));
		}
	}

	private AudioClip GetAudioClip() {
		// get current clip
		var clip = clips[playIndex >= clips.Length ? 0 : playIndex];

		// find next clip
		switch (playOrder) {
			case SoundClipPlayOrder.in_order:
				playIndex = (playIndex + 1) % clips.Length;
				break;
			case SoundClipPlayOrder.random:
				playIndex = Random.Range(0, clips.Length);
				break;
			case SoundClipPlayOrder.reverse:
				playIndex = (playIndex + clips.Length - 1) % clips.Length;
				break;
		}

		// return clip
		return clip;
	}

	public void Play() => Play(null);

	public AudioSource PlayAndReturn() => Play(null);

	public AudioSource Play(AudioSource audioSourceParam) {
		if (clips.Length == 0) {
			Debug.LogWarning($"Missing sound clips for {name}");
			return null;
		}

		var source = audioSourceParam;
		if (source == null) {
			var _obj = new GameObject("Sound", typeof(AudioSource));
			source = _obj.GetComponent<AudioSource>();
		}

		// set source config:
		source.clip = GetAudioClip();
		source.volume = Random.Range(volume.x, volume.y) * VolumeMultiplier.Value;
		source.pitch = useSemitones
			? Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, Random.Range(semitones.x, semitones.y))
			: Random.Range(pitch.x, pitch.y);

		source.Play();

#if UNITY_EDITOR
		if (source != previewer) {
			Destroy(source.gameObject, source.clip.length / source.pitch);
		}
#else
                Destroy(source.gameObject, source.clip.length / source.pitch);
#endif
		return source;
	}


#if UNITY_EDITOR
	[MenuItem("Assets/SoundEffectSO/From Selected Objects")]
	private static void CreateSOFromAudioClip() {
		foreach (var audio in Selection.GetFiltered<AudioClip>(SelectionMode.Assets)) {
			var path = AssetDatabase.GetAssetPath(audio);
			var directory = Path.GetDirectoryName(path);
			var fileName = Path.GetFileNameWithoutExtension(path);
			var instance = CreateInstance<SoundEffectSO>();
			instance.clips = new[] {audio};

			AssetDatabase.CreateAsset(instance, directory + "\\" + fileName + ".asset");
			AssetDatabase.SaveAssets();

			Undo.RegisterCreatedObjectUndo(instance, "Add new object");
		}
	}

#endif

	private enum SoundClipPlayOrder {
		random,
		in_order,
		reverse
	}
}
