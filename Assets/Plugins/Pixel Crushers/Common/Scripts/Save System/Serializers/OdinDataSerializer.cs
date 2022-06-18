using System.Text;
using PeraCore.Runtime;
using PixelCrushers;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;

public class OdinDataSerializer : DataSerializer {
	public ScriptableObjectCache Cache;
	public DataFormat Format;

	private void Awake() {
		Cache.Objects.ForEach(o => {
			Debug.Log($"{o.Key} : {o.Value}");
		});
	}

	public override string Serialize(object data) {
		var context = new SerializationContext {
			StringReferenceResolver = new RuntimeScriptableObjectStringReferenceResolver {Cache = Cache},
		};
		return Encoding.UTF8.GetString(SerializationUtility.SerializeValue(data, Format, context));
	}

	public override T Deserialize<T>(string s, T data = default) {
		var context = new DeserializationContext {
			StringReferenceResolver = new RuntimeScriptableObjectStringReferenceResolver {Cache = Cache},
		};
		var bytes = Encoding.UTF8.GetBytes(s);
		return SerializationUtility.DeserializeValue<T>(bytes, Format, context);
	}
}

public class RuntimeScriptableObjectStringReferenceResolver : IExternalStringReferenceResolver {
	public ScriptableObjectCache Cache;
	public IExternalStringReferenceResolver NextResolver { get; set; }

	public bool CanReference(object value, out string id) {
		if (value is ScriptableObject so) {
			id = so.name;
			return true;
		}

		id = null;
		return false;
	}

	public bool TryResolveReference(string id, out object value) {
		value = Cache.Objects[id];
		return value != null;
	}
}
